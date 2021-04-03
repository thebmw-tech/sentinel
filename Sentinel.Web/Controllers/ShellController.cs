using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Services;
using Sentinel.Web.Models;

namespace Sentinel.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShellController : ControllerBase
    {
        private readonly CommandInterpreter interpreter;

        public ShellController(CommandInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        [HttpPost("execute")]
        public ActionResult<WebShellExecutionResponse> Execute(WebShellExecutionRequest shellExecutionRequest)
        {
            StringBuilder outStringBuilder = new StringBuilder();
            StringBuilder errorStringBuilder = new StringBuilder();
            var shell = new WebShell(new StringWriter(outStringBuilder), new StringWriter(errorStringBuilder));
            shell.Context = shellExecutionRequest.Context;
            shell.CommandMode = shellExecutionRequest.CommandMode;

            var response = interpreter.Execute(shell, shellExecutionRequest.CommandMode, shellExecutionRequest.Command);

            var executionResponse = new WebShellExecutionResponse()
            {
                CommandMode = shell.CommandMode,
                Context = shell.Context,
                Error = errorStringBuilder.ToString(),
                Output = outStringBuilder.ToString(),
                Return = response
            };

            return Ok(executionResponse);
        }
    }
}
