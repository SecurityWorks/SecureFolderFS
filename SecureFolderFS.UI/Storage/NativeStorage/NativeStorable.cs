﻿using SecureFolderFS.Sdk.Storage;
using SecureFolderFS.Sdk.Storage.LocatableStorage;
using SecureFolderFS.Shared.Helpers;

namespace SecureFolderFS.UI.Storage.NativeStorage
{
    /// <inheritdoc cref="IStorable"/>
    public abstract class NativeStorable : ILocatableStorable
    {
        private string? _computedId;

        /// <inheritdoc/>
        public string Path { get; protected set; }

        /// <inheritdoc/>
        public string Name { get; protected set; }

        /// <inheritdoc/>
        public virtual string Id => _computedId ??= ChecksumHelpers.CalculateChecksumForPath(Path);

        protected NativeStorable(string path)
        {
            Path = FormatPath(path);
            Name = System.IO.Path.GetFileName(path);
        }

        /// <inheritdoc/>
        public virtual Task<IFolder?> GetParentAsync(CancellationToken cancellationToken = default)
        {
            var parentPath = System.IO.Path.GetDirectoryName(Path);
            if (string.IsNullOrEmpty(parentPath))
                return Task.FromResult<IFolder?>(null);

            return Task.FromResult<IFolder?>(new NativeFolder(parentPath));
        }

        protected static string FormatPath(string path)
        {
            path = path.Replace("file:///", string.Empty);

            if ('/' != System.IO.Path.DirectorySeparatorChar)
                return path.Replace('/', System.IO.Path.DirectorySeparatorChar);

            if ('\\' != System.IO.Path.DirectorySeparatorChar)
                return path.Replace('\\', System.IO.Path.DirectorySeparatorChar);

            return path;
        }
    }
}