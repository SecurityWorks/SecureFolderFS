﻿using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using SecureFolderFS.UI.ViewModels;
using SecureFolderFS.WinUI.ViewModels;

namespace SecureFolderFS.WinUI.TemplateSelectors
{
    internal sealed class WizardAuthenticationTemplateSelector : GenericTemplateSelector<ObservableObject>
    {
        public DataTemplate? PasswordTemplate { get; set; }

        public DataTemplate? KeyFileTemplate { get; set; }

        public DataTemplate? WindowsHelloTemplate { get; set; }

        protected override DataTemplate? SelectTemplateCore(ObservableObject? item, DependencyObject container)
        {
            return item switch
            {
                PasswordCreationViewModel => PasswordTemplate,
                KeyFileCreationViewModel => KeyFileTemplate,
                WindowsHelloCreationViewModel => WindowsHelloTemplate,
                _ => base.SelectTemplateCore(item, container)
            };
        }
    }
}
