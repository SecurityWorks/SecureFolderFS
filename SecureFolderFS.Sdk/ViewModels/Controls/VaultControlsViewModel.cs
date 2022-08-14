﻿using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SecureFolderFS.Sdk.Messages;
using SecureFolderFS.Sdk.Messages.Navigation;
using SecureFolderFS.Sdk.Services;
using SecureFolderFS.Sdk.Storage.LocatableStorage;
using SecureFolderFS.Sdk.ViewModels.Pages.Vault;
using SecureFolderFS.Sdk.ViewModels.Pages.Vault.Dashboard;
using SecureFolderFS.Sdk.ViewModels.Vault;

namespace SecureFolderFS.Sdk.ViewModels.Controls
{
    public sealed partial class VaultControlsViewModel : ObservableObject
    {
        private readonly IMessenger _messenger;
        private readonly VaultViewModel _vaultViewModel;

        private IFileExplorerService FileExplorerService { get; } = Ioc.Default.GetRequiredService<IFileExplorerService>();

        public VaultControlsViewModel(IMessenger messenger, VaultViewModel vaultViewModel)
        {
            _messenger = messenger;
            _vaultViewModel = vaultViewModel;
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task ShowInFileExplorerAsync()
        {
            if (_vaultViewModel.UnlockedVaultModel.UnlockedFolder is not ILocatableFolder rootFolder)
                return;

            await FileExplorerService.OpenInFileExplorerAsync(rootFolder);
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task LockVaultAsync()
        {
            await _vaultViewModel.UnlockedVaultModel.LockAsync();

            var loginPageViewModel = new VaultLoginPageViewModel(_vaultViewModel.VaultModel);
            _ = loginPageViewModel.InitAsync();

            WeakReferenceMessenger.Default.Send(new VaultLockedMessage(_vaultViewModel.VaultModel));
            WeakReferenceMessenger.Default.Send(new NavigationRequestedMessage(loginPageViewModel));
        }

        [RelayCommand]
        private void OpenProperties()
        {
            _messenger.Send(new NavigationRequestedMessage(new VaultPropertiesPageViewModel(_vaultViewModel, _messenger)));
        }
    }
}