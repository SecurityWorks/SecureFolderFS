﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SecureFolderFS.Shared.Utilities;
using System;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.ViewModels.Views.Vault.Login
{
    public sealed partial class MigrationViewModel : BaseLoginViewModel
    {
        [ObservableProperty] private string? _CurrentVersion;
        [ObservableProperty] private string? _NewVersion;

        /// <inheritdoc/>
        public override event EventHandler<EventArgs>? StateChanged;

        public MigrationViewModel()
        {
        }

        public MigrationViewModel(int newVersion)
        {
            _NewVersion = $"Update — Version {newVersion}";
        }

        public MigrationViewModel(int currentVersion, int newVersion)
        {
            _CurrentVersion = $"Version {currentVersion}";
            _NewVersion = $"Version {newVersion}";
        }

        /// <inheritdoc/>
        protected override void SetError(IResult? result)
        {
        }

        [RelayCommand]
        private async Task MigrateAsync()
        {
            // TODO
        }
    }
}
