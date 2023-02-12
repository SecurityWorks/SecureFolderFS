﻿using SecureFolderFS.UI.Enums;
using System.ComponentModel;

namespace SecureFolderFS.UI.Helpers
{
    /// <summary>
    /// Represents a helper interface used 
    /// </summary>
    public interface IThemeHelper : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the current theme used by the app.
        /// </summary>
        ThemeType CurrentTheme { get; }

        /// <summary>
        /// Updates the application's theme to specified <paramref name="themeType"/>.
        /// </summary>
        /// <param name="themeType">The theme to set for the app.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetThemeAsync(ThemeType themeType, CancellationToken cancellationToken = default);
    }

    /// <inheritdoc cref="IThemeHelper"/>
    public interface IThemeHelper<out TImplementation> : IThemeHelper
        where TImplementation : IThemeHelper
    {
        /// <summary>
        /// Gets the singleton instance of <typeparamref name="TImplementation"/>.
        /// </summary>
        static abstract TImplementation Instance { get; }
    }
}
