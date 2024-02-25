﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SecureFolderFS.Sdk.Attributes;
using SecureFolderFS.Sdk.Services;
using SecureFolderFS.Sdk.Storage;
using SecureFolderFS.Sdk.Storage.DirectStorage;
using SecureFolderFS.Sdk.Storage.ModifiableStorage;
using SecureFolderFS.Sdk.Storage.NestedStorage;
using SecureFolderFS.Shared.ComponentModel;
using SecureFolderFS.Shared.Extensions;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.ViewModels.Views.Browser
{
    [Inject<IOverlayService>]
    public partial class BrowserViewModel : ObservableObject
    {
        [ObservableProperty] private IViewable? _CurrentView;

        public BrowserViewModel()
        {
            ServiceProvider = Ioc.Default;
        }

        [RelayCommand]
        protected virtual async Task NewFolderAsync(CancellationToken cancellationToken)
        {
            if (CurrentView is not FolderViewModel { Folder: IModifiableFolder modifiableFolder } folderViewModel)
                return;

            // TODO: Add NewFolderOverlayViewModel
            _ = modifiableFolder;
            var result = await OverlayService.ShowAsync(null);
            if (result is IResult<IFolder> { Successful: true, Value: not null } folderResult)
                folderViewModel.Items.Add(new FolderViewModel(folderResult.Value));
        }

        [RelayCommand]
        protected virtual async Task CopyAsync(IEnumerable items, CancellationToken cancellationToken)
        {
            // TODO: Add CopyOverlayViewModel
            var result = await OverlayService.ShowAsync(null);
            if (result is IResult<IFolder> { Successful: true, Value: IDirectCopy directCopy })
            {
                foreach (INestedStorable item in items)
                    await directCopy.CreateCopyOfAsync(item, default, cancellationToken);
            }
        }

        [RelayCommand]
        protected virtual async Task MoveAsync(IEnumerable items, CancellationToken cancellationToken)
        {
            if (CurrentView is not FolderViewModel { Folder: IModifiableFolder modifiableFolder } folderViewModel)
                return;

            // TODO: Add MoveOverlayViewModel
            var result = await OverlayService.ShowAsync(null);
            if (result is not IResult<IFolder> { Successful: true, Value: IDirectMove directMove })
                return;

            foreach (INestedStorable item in items)
            {
                await directMove.MoveFromAsync(item, modifiableFolder, default, cancellationToken);
                folderViewModel.Items.RemoveMatch(x => x.Inner.Id == item.Id);
            }
        }

        [RelayCommand]
        protected virtual async Task DeleteAsync(IEnumerable items, CancellationToken cancellationToken)
        {
            if (CurrentView is not FolderViewModel { Folder: IModifiableFolder modifiableFolder } folderViewModel)
                return;

            // TODO: Add DeletionOverlayViewModel
            var result = await OverlayService.ShowAsync(null);
            if (!result.Successful)
                return;

            foreach (INestedStorable item in items)
            {
                await modifiableFolder.DeleteAsync(item, default, cancellationToken);
                folderViewModel.Items.RemoveMatch(x => x.Inner.Id == item.Id);
            }
        }
    }
}
