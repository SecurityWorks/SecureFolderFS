﻿using SecureFolderFS.Core.Cryptography.SecureStore;
using SecureFolderFS.Shared.Extensions;
using System;
using System.Runtime.CompilerServices;
using static SecureFolderFS.Core.Cryptography.Constants.Security.Chunks.AesCtrHmac;
using static SecureFolderFS.Core.Cryptography.Extensions.ContentCryptExtensions.AesCtrHmacContentExtensions;
using static SecureFolderFS.Core.Cryptography.Extensions.HeaderCryptExtensions.AesCtrHmacHeaderExtensions;

namespace SecureFolderFS.Core.Cryptography.ContentCrypt
{
    /// <inheritdoc cref="IContentCrypt"/>
    public sealed class AesCtrHmacContentCrypt : BaseContentCrypt
    {
        private readonly SecretKey _macKey;

        /// <inheritdoc/>
        public override int ChunkCleartextSize { get; } = CHUNK_CLEARTEXT_SIZE;

        /// <inheritdoc/>
        public override int ChunkCiphertextSize { get; } = CHUNK_CIPHERTEXT_SIZE;

        public AesCtrHmacContentCrypt(SecretKey macKey, CipherProvider cipherProvider)
            : base(cipherProvider)
        {
            _macKey = macKey;
        }

        /// <inheritdoc/>
        public override void EncryptChunk(ReadOnlySpan<byte> cleartextChunk, long chunkNumber, ReadOnlySpan<byte> header, Span<byte> ciphertextChunk)
        {
            // Chunk nonce
            secureRandom.GetBytes(ciphertextChunk.Slice(0, CHUNK_NONCE_SIZE));

            // Encrypt
            cipherProvider.AesCtrCrypt.Encrypt(
                cleartextChunk,
                header.GetHeaderContentKey(),
                ciphertextChunk.Slice(0, CHUNK_NONCE_SIZE),
                ciphertextChunk.Slice(CHUNK_NONCE_SIZE, cleartextChunk.Length));

            // Calculate MAC
            CalculateChunkMac(
                header.GetHeaderNonce(),
                ciphertextChunk.Slice(0, CHUNK_NONCE_SIZE),
                ciphertextChunk.Slice(CHUNK_NONCE_SIZE, cleartextChunk.Length),
                chunkNumber,
                ciphertextChunk.Slice(CHUNK_NONCE_SIZE + cleartextChunk.Length, CHUNK_MAC_SIZE));
        }

        /// <inheritdoc/>
        [SkipLocalsInit]
        public override bool DecryptChunk(ReadOnlySpan<byte> ciphertextChunk, long chunkNumber,
            ReadOnlySpan<byte> header, Span<byte> cleartextChunk)
        {
            // Allocate byte* for MAC
            Span<byte> mac = stackalloc byte[CHUNK_MAC_SIZE];

            // Calculate MAC
            CalculateChunkMac(
                header.GetHeaderNonce(),
                ciphertextChunk.GetChunkNonce(),
                ciphertextChunk.GetChunkPayload(),
                chunkNumber,
                mac);

            // Check MAC
            if (!mac.SequenceEqual(ciphertextChunk.GetChunkMac()))
                return false;

            // Decrypt
            cipherProvider.AesCtrCrypt.Decrypt(
                ciphertextChunk.GetChunkPayload(),
                header.GetHeaderContentKey(),
                ciphertextChunk.GetChunkNonce(),
                cleartextChunk);

            return true;
        }

        private void CalculateChunkMac(ReadOnlySpan<byte> headerNonce, ReadOnlySpan<byte> chunkNonce, ReadOnlySpan<byte> ciphertextPayload, long chunkNumber, Span<byte> chunkMac)
        {
            var beChunkNumber = BitConverter.GetBytes(chunkNumber).AsBigEndian();

            using var hmacSha256 = cipherProvider.GetHmacInstance();
            hmacSha256.InitializeHmac(_macKey);
            hmacSha256.Update(headerNonce);
            hmacSha256.Update(beChunkNumber);
            hmacSha256.Update(chunkNonce);
            hmacSha256.Update(ciphertextPayload);

            hmacSha256.GetHash(chunkMac);
        }
    }
}
