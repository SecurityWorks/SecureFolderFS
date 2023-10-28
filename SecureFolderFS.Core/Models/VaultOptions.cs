﻿using SecureFolderFS.Core.Cryptography.Enums;

namespace SecureFolderFS.Core.Models
{
    // TODO: Needs docs
    public sealed class VaultOptions
    {
        public required string ContentCipherId { get; init; }

        public required string FileNameCipherId { get; init; }
    }
}
