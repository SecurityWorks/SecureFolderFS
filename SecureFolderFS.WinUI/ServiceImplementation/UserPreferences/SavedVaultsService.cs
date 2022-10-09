﻿using SecureFolderFS.Sdk.AppModels;
using SecureFolderFS.Sdk.Services.UserPreferences;
using SecureFolderFS.Sdk.Storage.ModifiableStorage;
using SecureFolderFS.WinUI.Serialization;
using System.Collections.Generic;

namespace SecureFolderFS.WinUI.ServiceImplementation.UserPreferences
{
    /// <inheritdoc cref="ISavedVaultsService"/>
    internal sealed class SavedVaultsService : SingleFileSettingsModel, ISavedVaultsService
    {
        public SavedVaultsService(IModifiableFolder? settingsFolder)
            : base(new SavedVaultsStreamSerializer())
        {
            SettingsFolder = settingsFolder;
        }

        /// <inheritdoc/>
        protected override string? SettingsStorageName { get; } = Constants.LocalSettings.SAVED_VAULTS_FILENAME;

        /// <inheritdoc/>
        public List<string>? VaultPaths
        {
            get => GetSetting<List<string>?>(null);
            set => SetSetting(value);
        }
    }
}
