﻿using Sentinel.Core.Command.Interfaces;

namespace Sentinel.Core.Environments
{
    public class ConfigurationEnvironment : IEnvironmentSetup
    {
        public void Cleanup(IShell shell)
        {
            throw new System.NotImplementedException();
        }

        public string[] Setup(IShell shell, string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}