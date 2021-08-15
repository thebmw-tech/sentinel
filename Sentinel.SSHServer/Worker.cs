using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FxSsh;
using FxSsh.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core.Repository.Interfaces;

namespace Sentinel.SSHServer
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        private readonly ILogger<Worker> logger;

        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var server = new SshServer();

            // TODO load ssh host keys
            server.AddHostKey("ssh-rsa", "BwIAAACkAABSU0EyAAQAAAEAAQADKjiW5UyIad8ITutLjcdtejF4wPA1dk1JFHesDMEhU9pGUUs+HPTmSn67ar3UvVj/1t/+YK01FzMtgq4GHKzQHHl2+N+onWK4qbIAMgC6vIcs8u3d38f3NFUfX+lMnngeyxzbYITtDeVVXcLnFd7NgaOcouQyGzYrHBPbyEivswsnqcnF4JpUTln29E1mqt0a49GL8kZtDfNrdRSt/opeexhCuzSjLPuwzTPc6fKgMc6q4MBDBk53vrFY2LtGALrpg3tuydh3RbMLcrVyTNT+7st37goubQ2xWGgkLvo+TZqu3yutxr1oLSaPMSmf9bTACMi5QDicB3CaWNe9eU73MzhXaFLpNpBpLfIuhUaZ3COlMazs7H9LCJMXEL95V6ydnATf7tyO0O+jQp7hgYJdRLR3kNAKT0HU8enE9ZbQEXG88hSCbpf1PvFUytb1QBcotDy6bQ6vTtEAZV+XwnUGwFRexERWuu9XD6eVkYjA4Y3PGtSXbsvhwgH0mTlBOuH4soy8MV4dxGkxM8fIMM0NISTYrPvCeyozSq+NDkekXztFau7zdVEYmhCqIjeMNmRGuiEo8ppJYj4CvR1hc8xScUIw7N4OnLISeAdptm97ADxZqWWFZHno7j7rbNsq5ysdx08OtplghFPx4vNHlS09LwdStumtUel5oIEVMYv+yWBYSPPZBcVY5YFyZFJzd0AOkVtUbEbLuzRs5AtKZG01Ip/8+pZQvJvdbBMLT1BUvHTrccuRbY03SHIaUM3cTUc=");
            server.AddHostKey("ssh-dss", "BwIAAAAiAABEU1MyAAQAAG+6KQWB+crih2Ivb6CZsMe/7NHLimiTl0ap97KyBoBOs1amqXB8IRwI2h9A10R/v0BHmdyjwe0c0lPsegqDuBUfD2VmsDgrZ/i78t7EJ6Sb6m2lVQfTT0w7FYgVk3J1Deygh7UcbIbDoQ+refeRNM7CjSKtdR+/zIwO3Qub2qH+p6iol2iAlh0LP+cw+XlH0LW5YKPqOXOLgMIiO+48HZjvV67pn5LDubxru3ZQLvjOcDY0pqi5g7AJ3wkLq5dezzDOOun72E42uUHTXOzo+Ct6OZXFP53ZzOfjNw0SiL66353c9igBiRMTGn2gZ+au0jMeIaSsQNjQmWD+Lnri39n0gSCXurDaPkec+uaufGSG9tWgGnBdJhUDqwab8P/Ipvo5lS5p6PlzAQAAACqx1Nid0Ea0YAuYPhg+YolsJ/ce");



            server.ConnectionAccepted += ServerOnConnectionAccepted;

            server.Start();

            logger.LogInformation("SSH Server Started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            logger.LogInformation("Stopping SSH Server");

            server.Stop();
        }

        private void ServerOnConnectionAccepted(object sender, Session e)
        {
            logger.LogInformation($"New Client Connection");

            e.ServiceRegistered += SessionOnServiceRegistered;
            e.KeysExchanged += EOnKeysExchanged;

        }

        private void EOnKeysExchanged(object sender, KeyExchangeArgs e)
        {
            foreach (var algorithm in e.KeyExchangeAlgorithms)
            {
                logger.LogTrace($"Key exchange algorithm: {algorithm}");
            }
        }

        private void SessionOnServiceRegistered(object sender, SshService e)
        {
            var session = (Session)sender;
            logger.LogTrace($"Session {BitConverter.ToString(session.SessionId).Replace("-", "")} requesting {e.GetType().Name}.");

            switch (e)
            {
                case UserauthService userauthService:
                    userauthService.Userauth += UserauthServiceOnUserauth;
                    break;
                case ConnectionService connectionService:
                    connectionService.CommandOpened += ConnectionServiceOnCommandOpened;
                    connectionService.EnvReceived += ConnectionServiceOnEnvReceived;
                    connectionService.PtyReceived += ConnectionServiceOnPtyReceived;
                    connectionService.TcpForwardRequest += ConnectionServiceOnTcpForwardRequest;
                    
                    break;
            }
        }

        


        private void UserauthServiceOnUserauth(object? sender, UserauthArgs e)
        {
            logger.LogInformation($"Attempting to authenticate \"{e.Username}\" using \"{e.AuthMethod}\"");

            using var scope = serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var user = userRepository.Filter(u => u.Username == e.Username).Include(u => u.Keys).FirstOrDefault();
            
            if (user != null)
            {
                logger.LogInformation($"Found user \"{user.Username}\" with id {user.Id:B}");

                switch (e.AuthMethod)
                {
                    case "publickey":
                        e.Result = user.Keys.Any(userKey => userKey.Key.SequenceEqual(e.Key));
                        break;
                    case "password":
                        e.Result = BCrypt.Net.BCrypt.Verify(e.Password, user.Password);
                        break;
                    default:
                        logger.LogWarning($"Missing Auth Method: {e.AuthMethod}");
                        break;
                }
            }

            logger.LogInformation($"Auth Result: {e.Result}");
        }



        private void ConnectionServiceOnCommandOpened(object? sender, CommandRequestedArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ConnectionServiceOnTcpForwardRequest(object? sender, TcpRequestArgs e)
        {
            // Console.WriteLine("Received a request to forward data to {0}:{1}", e.Host, e.Port);
            //
            // var allow = true;  // func(e.Host, e.Port, e.AttachedUserauthArgs);
            //
            // if (!allow)
            //     return;
            //
            // var tcp = new TcpForwardService(e.Host, e.Port, e.OriginatorIP, e.OriginatorPort);
            // e.Channel.DataReceived += (ss, ee) => tcp.OnData(ee);
            // e.Channel.CloseReceived += (ss, ee) => tcp.OnClose();
            // tcp.DataReceived += (ss, ee) => e.Channel.SendData(ee);
            // tcp.CloseReceived += (ss, ee) => e.Channel.SendClose();
            // tcp.Start();
        }

        private void ConnectionServiceOnPtyReceived(object? sender, PtyArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ConnectionServiceOnEnvReceived(object? sender, EnvironmentArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
