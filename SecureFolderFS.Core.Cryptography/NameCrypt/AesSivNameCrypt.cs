﻿using SecureFolderFS.Core.Cryptography.SecureStore;
using System;

namespace SecureFolderFS.Core.Cryptography.NameCrypt
{
    /// <inheritdoc cref="INameCrypt"/>
    internal sealed class AesSivNameCrypt : BaseNameCrypt
    {
        public AesSivNameCrypt(SecretKey macKey, SecretKey encryptionKey, CipherProvider cipherProvider)
            : base(macKey, encryptionKey, cipherProvider)
        {
        }

        /// <inheritdoc/>
        protected override byte[] EncryptFileName(ReadOnlySpan<byte> cleartextFileNameBuffer, ReadOnlySpan<byte> directoryId)
        {
            return cipherProvider.AesSivCrypt.Encrypt(cleartextFileNameBuffer, encryptionKey, macKey, directoryId);
        }

        /// <inheritdoc/>
        protected override byte[]? DecryptFileName(ReadOnlySpan<byte> ciphertextFileNameBuffer, ReadOnlySpan<byte> directoryId)
        {
            return cipherProvider.AesSivCrypt.Decrypt(ciphertextFileNameBuffer, encryptionKey, macKey, directoryId);
        }
    }
}
