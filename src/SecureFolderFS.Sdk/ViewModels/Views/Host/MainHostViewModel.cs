﻿using CommunityToolkit.Mvvm.ComponentModel;
using SecureFolderFS.Sdk.Attributes;
using SecureFolderFS.Sdk.Models;
using SecureFolderFS.Sdk.Services;
using SecureFolderFS.Sdk.ViewModels.Controls.VaultList;
using SecureFolderFS.Shared;
using SecureFolderFS.Shared.ComponentModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.ViewModels.Views.Host
{
    [Inject<INavigationService>(Visibility = "public")]
    [Bindable(true)]
    public sealed partial class MainHostViewModel : ObservableObject, IViewDesignation, IAsyncInitialize
    {
        [ObservableProperty] private string? _Title;

        public VaultListViewModel VaultListViewModel { get; }

        public MainHostViewModel(IVaultCollectionModel vaultCollectionModel)
        {
            ServiceProvider = DI.Default;
            VaultListViewModel = new(vaultCollectionModel);
        }

        /// <inheritdoc/>
        public Task InitAsync(CancellationToken cancellationToken = default)
        {
            return VaultListViewModel.InitAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public void OnAppearing()
        {
        }

        /// <inheritdoc/>
        public void OnDisappearing()
        {
        }
    }
}
