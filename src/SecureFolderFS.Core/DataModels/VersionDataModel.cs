﻿using System;
using System.Text.Json.Serialization;
using static SecureFolderFS.Core.Constants.Vault;

namespace SecureFolderFS.Core.DataModels
{
    [Serializable]
    public class VersionDataModel
    {
        /// <summary>
        /// Gets the version of the vault.
        /// </summary>
        [JsonPropertyName(Associations.ASSOC_VERSION)]
        public required int Version { get; init; }
    }
}
