﻿using System;
using System.Threading.Tasks;
using SecureFolderFS.Shared.Utils;

namespace SecureFolderFS.Sdk.Services
{
    [Obsolete("This service has been deprecated. Use SynchronizationContext instead for thread manipulation.")]
    public interface IThreadingService
    {
        IAwaitable ExecuteOnUiThreadAsync();

        Task ExecuteOnUiThreadAsync(Action action);
    }
}