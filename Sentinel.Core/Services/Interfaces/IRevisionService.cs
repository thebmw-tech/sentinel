﻿using Sentinel.Models;

namespace Sentinel.Core.Services.Interfaces
{
    public interface IRevisionService
    {
        RevisionDTO CreateRevisionForEditing();
    }
}