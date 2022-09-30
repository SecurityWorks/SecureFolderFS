﻿namespace SecureFolderFS.Core
{
    public static class Constants
    {
        public const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string CONTENT_FOLDERNAME = "content";

        public const string VAULT_CONFIGURATION_FILENAME = "sfconfig.cfg";

        public const string VAULT_KEYSTORE_FILENAME = "keystore.cfg";

        public const string DIRECTORY_ID_FILENAME = "dirid.iv";

        public const string ENCRYPTED_FILE_EXTENSION = ".sffs";

        public static class FileSystemId
        {
            public const string DOKAN_ID = "DOKANY";
        }

        public static class CipherId
        {
            public const string NONE = "None";

            public const string AES_CTR_HMAC = "AES-CTR_HMAC";

            public const string AES_GCM = "AES-GCM";

            public const string XCHACHA20_POLY1305 = "XChaCha20-Poly1305";

            public const string AES_SIV = "AES-SIV";
        }

        internal static class FileSystem
        {
            public const long INVALID_HANDLE = 0L;

            public const uint FILESYSTEM_SERIAL_NUMBER = 1137196800u;

            public const string FILESYSTEM_NAME = "NTFS";

            public const string UNC_NAME = "securefolderfs";


            internal static class Dokan
            {
                public const int DOKAN_VERSION = 205;

                public const int DOKAN_MAX_VERSION = 210;

                public const uint MAX_COMPONENT_LENGTH = 256;

                public const DokanNet.FileAccess DATA_ACCESS =
                                                          DokanNet.FileAccess.ReadData
                                                        | DokanNet.FileAccess.WriteData
                                                        | DokanNet.FileAccess.AppendData
                                                        | DokanNet.FileAccess.Execute
                                                        | DokanNet.FileAccess.GenericExecute
                                                        | DokanNet.FileAccess.GenericWrite
                                                        | DokanNet.FileAccess.GenericRead;

                public const DokanNet.FileAccess DATA_WRITE_ACCESS =
                                                             DokanNet.FileAccess.WriteData
                                                           | DokanNet.FileAccess.AppendData
                                                           | DokanNet.FileAccess.Delete
                                                           | DokanNet.FileAccess.GenericWrite;
            }
        }

        internal static class IO
        {
            public const int MAX_CACHED_CHUNKS = 6;

            public const int MAX_CACHED_DIRECTORY_IDS = 1000;

            public const int MAX_CACHED_CIPHERTEXT_FILENAMES = 2000;

            public const int MAX_CACHED_CLEARTEXT_FILENAMES = 2000;
        }

        public static class KeyChains
        {
            public const int ENCRYPTIONKEY_LENGTH = 32;

            public const int MACKEY_LENGTH = 32;

            public const int SALT_LENGTH = 16;
        }
    }
}
