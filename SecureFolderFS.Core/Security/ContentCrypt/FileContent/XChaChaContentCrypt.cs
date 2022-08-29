﻿using SecureFolderFS.Core.Security.Cipher;
using SecureFolderFS.Shared.Extensions;
using System;
using System.Runtime.CompilerServices;
using static SecureFolderFS.Core.Constants.Security.Chunks.XChaCha20Poly1305;
using static SecureFolderFS.Core.Constants.Security.Headers.XChaCha20Poly1305;
using static SecureFolderFS.Core.Extensions.SecurityExtensions.Content.XChaChaContentExtensions;
using static SecureFolderFS.Core.Extensions.SecurityExtensions.Header.XChaChaHeaderExtensions;

namespace SecureFolderFS.Core.Security.ContentCrypt.FileContent
{
    /// <inheritdoc cref="IContentCrypt"/>
    internal sealed class XChaChaContentCrypt : BaseContentCrypt
    {
        /// <inheritdoc/>
        public override int ChunkCleartextSize { get; } = CHUNK_CLEARTEXT_SIZE;

        /// <inheritdoc/>
        public override int ChunkCiphertextSize { get; } = CHUNK_CIPHERTEXT_SIZE;

        public XChaChaContentCrypt(ICipherProvider cipherProvider)
            : base(cipherProvider)
        {
        }

        /// <inheritdoc/>
        [SkipLocalsInit]
        public override void EncryptChunk(ReadOnlySpan<byte> cleartextChunk, long chunkNumber, ReadOnlySpan<byte> header, Span<byte> ciphertextChunk)
        {
            // Chunk nonce
            secureRandom.GetBytes(ciphertextChunk.Slice(0, CHUNK_NONCE_SIZE));

            // Big Endian chunk number and file header nonce
            Span<byte> associatedData = stackalloc byte[sizeof(long) + HEADER_NONCE_SIZE];
            Unsafe.As<byte, long>(ref associatedData[0]) = chunkNumber;
            associatedData.Slice(0, sizeof(long)).AsBigEndian();
            header.GetHeaderNonce().CopyTo(associatedData.Slice(sizeof(long)));

            // Encrypt
            cipherProvider.XChaCha20Poly1305Crypt.Encrypt(
                cleartextChunk,
                header.GetHeaderContentKey(),
                ciphertextChunk.Slice(0, CHUNK_NONCE_SIZE),
                ciphertextChunk.Slice(CHUNK_NONCE_SIZE),
                associatedData
            );
        }

        /// <inheritdoc/>
        [SkipLocalsInit]
        public override unsafe bool DecryptChunk(ReadOnlySpan<byte> ciphertextChunk, long chunkNumber, ReadOnlySpan<byte> header, Span<byte> cleartextChunk)
        {
            // Big Endian chunk number and file header nonce
            Span<byte> associatedData = stackalloc byte[sizeof(long) + HEADER_NONCE_SIZE];
            Unsafe.As<byte, long>(ref associatedData[0]) = chunkNumber;
            associatedData.Slice(0, sizeof(long)).AsBigEndian();
            header.GetHeaderNonce().CopyTo(associatedData.Slice(sizeof(long)));

            // Decrypt
            return cipherProvider.XChaCha20Poly1305Crypt.Decrypt(
                ciphertextChunk.GetChunkPayloadWithTag(),
                header.GetHeaderContentKey(),
                ciphertextChunk.GetChunkNonce(),
                cleartextChunk.Slice(0, ciphertextChunk.Length - (CHUNK_NONCE_SIZE + CHUNK_TAG_SIZE)),
                associatedData);
        }
    }
}
