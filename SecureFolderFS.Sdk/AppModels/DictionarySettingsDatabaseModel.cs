﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SecureFolderFS.Sdk.Models;
using SecureFolderFS.Sdk.Storage;
using SecureFolderFS.Shared.Utils;

namespace SecureFolderFS.Sdk.AppModels
{
    public class DictionarySettingsDatabaseModel : ISettingsDatabaseModel
    {
        protected readonly IAsyncSerializer<Stream> serializer;
        protected readonly SemaphoreSlim semaphore;
        protected readonly ConcurrentDictionary<string, object?> settingsCache;

        public DictionarySettingsDatabaseModel(IAsyncSerializer<Stream> serializer)
        {
            this.serializer = serializer;
            this.semaphore = new(1, 1);
            this.settingsCache = new();
        }

        /// <inheritdoc/>
        public virtual T? GetValue<T>(string key, Func<T?>? defaultValue)
        {
            if (settingsCache.TryGetValue(key, out var value))
                return (T?)value;

            var fallback = defaultValue is not null ? defaultValue() : default;
            settingsCache[key] = fallback;

            return fallback;
        }

        /// <inheritdoc/>
        public virtual bool SetValue<T>(string key, T? value)
        {
            settingsCache[key] = value;
            return true;
        }

        /// <inheritdoc/>
        public virtual async Task<bool> LoadFromFile(IFile file, CancellationToken cancellationToken = default)
        {
            try
            {
                await semaphore.WaitAsync(cancellationToken);
                await using var stream = await file.OpenStreamAsync(FileAccess.Read, FileShare.Read);

                var settings = await serializer.DeserializeAsync<Dictionary<string, object?>?>(stream, cancellationToken);
                if (settings is null)
                    return false;

                settingsCache.Clear();
                foreach (var item in settings)
                {
                    settingsCache[item.Key] = item.Value;
                }

                return true;
            }
            finally
            {
                _ = semaphore.Release();
            }
        }

        /// <inheritdoc/>
        public virtual async Task<bool> SaveToFile(IFile file, CancellationToken cancellationToken = default)
        {
            try
            {
                await semaphore.WaitAsync(cancellationToken);
                await using var stream = await file.OpenStreamAsync(FileAccess.ReadWrite, FileShare.Read);

                var settingsStream = await serializer.SerializeAsync<IDictionary<string, object?>>(settingsCache, cancellationToken);
                await settingsStream.CopyToAsync(stream, cancellationToken);

                return true;
            }
            finally
            {
                _ = semaphore.Release();
            }
        }
    }
}
