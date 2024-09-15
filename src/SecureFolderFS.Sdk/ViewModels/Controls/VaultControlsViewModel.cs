﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SecureFolderFS.Sdk.Attributes;
using SecureFolderFS.Sdk.Extensions;
using SecureFolderFS.Sdk.Messages;
using SecureFolderFS.Sdk.Services;
using SecureFolderFS.Sdk.ViewModels.Views.Vault;
using SecureFolderFS.Shared;
using SecureFolderFS.Shared.ComponentModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.ViewModels.Controls
{
    [Inject<IFileExplorerService>]
    [Bindable(true)]
    public sealed partial class VaultControlsViewModel : ObservableObject
    {
        private readonly INavigator _vaultNavigator;
        private readonly INavigator _dashboardNavigator;
        private readonly UnlockedVaultViewModel _unlockedVaultViewModel;
        private VaultPropertiesViewModel? _propertiesViewModel;

        public VaultControlsViewModel(INavigator vaultNavigator, INavigator dashboardNavigator, UnlockedVaultViewModel unlockedVaultViewModel, VaultPropertiesViewModel? propertiesViewModel = null)
        {
            _vaultNavigator = vaultNavigator;
            _dashboardNavigator = dashboardNavigator;
            _unlockedVaultViewModel = unlockedVaultViewModel;
            _propertiesViewModel = propertiesViewModel;
            ServiceProvider = DI.Default;
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task RevealFolderAsync(CancellationToken cancellationToken)
        {
            await FileExplorerService.TryOpenInFileExplorerAsync(_unlockedVaultViewModel.StorageRoot.Inner, cancellationToken);
        }

        [RelayCommand]
        private async Task LockVaultAsync()
        {
            // Lock vault
            await _unlockedVaultViewModel.DisposeAsync();

            // Prepare login page
            var loginPageViewModel = new VaultLoginViewModel(_unlockedVaultViewModel.VaultModel, _vaultNavigator);
            _ = loginPageViewModel.InitAsync();

            // Navigate away
            if (_vaultNavigator is INavigationService navigationService)
                await navigationService.TryNavigateAndForgetAsync(loginPageViewModel);
            else
                await _vaultNavigator.NavigateAsync(loginPageViewModel);

            WeakReferenceMessenger.Default.Send(new VaultLockedMessage(_unlockedVaultViewModel.VaultModel));
        }

        [RelayCommand]
        private async Task OpenPropertiesAsync(CancellationToken cancellationToken)
        {
            if (_propertiesViewModel is null)
            {
                _propertiesViewModel = new(_unlockedVaultViewModel);
                await _propertiesViewModel.InitAsync(cancellationToken);
            }

            await _dashboardNavigator.NavigateAsync(_propertiesViewModel);
        }
    }
}
