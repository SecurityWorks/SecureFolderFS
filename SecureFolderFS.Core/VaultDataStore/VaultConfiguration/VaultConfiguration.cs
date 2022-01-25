﻿using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using SecureFolderFS.Core.Extensions;
using SecureFolderFS.Core.Security.KeyCrypt;
using SecureFolderFS.Core.SecureStore;
using SecureFolderFS.Core.Enums;
using SecureFolderFS.Core.Helpers;

namespace SecureFolderFS.Core.VaultDataStore.VaultConfiguration
{
    [Serializable]
    internal sealed class VaultConfiguration : BaseVaultConfiguration
    {
        public VaultConfiguration(int version, ContentCipherScheme contentCipherScheme, FileNameCipherScheme fileNameCipherScheme, byte[] hmacsha256Mac) 
            : base(version, contentCipherScheme, fileNameCipherScheme, hmacsha256Mac)
        {
        }

        public static VaultConfiguration Load(RawVaultConfiguration rawVaultConfiguration)
        {
            return JsonConvert.DeserializeObject<VaultConfiguration>(rawVaultConfiguration.rawData);
        }

        public override bool Verify(IKeyCryptor keyCryptor, MasterKey masterKey)
        {
            if (Hmacsha256Mac.IsEmpty() || masterKey.IsEmpty() || keyCryptor == null)
            {
                return false;
            }

            using var macKey = masterKey.CreateMacKeyCopy();
            using var hmacSha256Crypt = keyCryptor.HmacSha256Crypt.GetInstance(macKey);

            hmacSha256Crypt.InitializeHMAC();
            hmacSha256Crypt.Update(BitConverter.GetBytes(Version));
            hmacSha256Crypt.Update(BitConverter.GetBytes((uint)FileNameCipherScheme));
            hmacSha256Crypt.DoFinal(BitConverter.GetBytes((uint)ContentCipherScheme));

            return Hmacsha256Mac.SequenceEqual(hmacSha256Crypt.GetHash());
        }

        public override void WriteConfiguration(Stream destinationStream)
        {
            string serialized = JsonConvert.SerializeObject(this, Formatting.Indented);

            StreamHelpers.WriteToStream(serialized, destinationStream);
        }
    }
}