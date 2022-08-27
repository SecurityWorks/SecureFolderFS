﻿using SecureFolderFS.Core.BufferHolders;
using SecureFolderFS.Core.Chunks;
using SecureFolderFS.Core.Chunks.ChunkAccessImpl;
using SecureFolderFS.Core.Streams.Management;
using SecureFolderFS.Shared.Extensions;
using System;
using System.Collections.Generic;

namespace SecureFolderFS.Core.FileSystem.OpenCryptoFiles
{
    internal sealed class OpenCryptFileReceiver : IDisposable
    {
        private readonly ChunkAccessFactory _chunkAccessFactory;
        private readonly Dictionary<string, OpenCryptFile> _openCryptFiles;

        public OpenCryptFileReceiver(ChunkAccessFactory chunkAccessFactory)
        {
            _chunkAccessFactory = chunkAccessFactory;
            _openCryptFiles = new();
        }

        public OpenCryptFile? TryGet(string ciphertextPath)
        {
            _openCryptFiles.TryGetValue(ciphertextPath, out var openCryptFile);
            return openCryptFile;
        }

        public OpenCryptFile Create(string ciphertextPath, CleartextHeaderBuffer fileHeader)
        {
            var ciphertextStreamsManager = new CiphertextStreamsManager();
            var chunkAccess = GetChunkAccess(ciphertextStreamsManager, fileHeader);

            var openCryptFile = new OpenCryptFile(ciphertextPath, CloseCryptFile, ciphertextStreamsManager, fileHeader, chunkAccess);
            _openCryptFiles.TryAdd(ciphertextPath, openCryptFile);

            return openCryptFile;
        }

        private void CloseCryptFile(string ciphertextPath)
        {
            if (_openCryptFiles.TryGetValue(ciphertextPath, out var openCryptFile))
            {
                openCryptFile?.Dispose();
                _openCryptFiles.Remove(ciphertextPath);
            }
        }

        private IChunkAccess GetChunkAccess(CiphertextStreamsManager ciphertextStreamsManager, CleartextHeaderBuffer fileHeader)
        {
            var reader = _chunkAccessFactory.GetChunkReader(ciphertextStreamsManager, fileHeader);
            var writer = _chunkAccessFactory.GetChunkWriter(ciphertextStreamsManager, fileHeader);

            return _chunkAccessFactory.GetChunkAccess(reader, writer);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _openCryptFiles.Values.DisposeCollection();
            _openCryptFiles.Clear();
        }
    }
}
