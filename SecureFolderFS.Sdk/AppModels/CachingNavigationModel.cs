﻿using SecureFolderFS.Sdk.Enums;
using SecureFolderFS.Sdk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.AppModels
{
    /// <inheritdoc cref="IStateNavigationModel"/>
    public abstract class CachingNavigationModel : IStateNavigationModel
    {
        /// <inheritdoc/>
        public ICollection<INavigationTarget> Targets { get; }

        /// <inheritdoc/>
        public INavigationTarget? CurrentTarget { get; protected set; }

        protected CachingNavigationModel()
        {
            Targets = new List<INavigationTarget>();
        }

        /// <inheritdoc/>
        public virtual Task<bool> GoBackAsync()
        {
            return Task.FromResult(false);
        }

        /// <inheritdoc/>
        public virtual Task<bool> GoForwardAsync()
        {
            return Task.FromResult(false);
        }

        /// <inheritdoc/>
        public virtual async Task NavigateAsync(INavigationTarget target)
        {
            // Notify the current target that it's being navigated from
            CurrentTarget?.OnNavigatingFrom();

            // Notify the new target that it's been navigated to
            target.OnNavigatingTo(NavigationType.Detached);

            // Start actual navigation
            await BeginNavigationAsync(target, NavigationType.Detached);

            // Update targets
            CurrentTarget = target;
        }

        /// <summary>
        /// Starts the navigation routine.
        /// </summary>
        /// <param name="target">The target to navigate to.</param>
        /// <param name="navigationType">The type of navigation to perform.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. Value is an identifier of type <typeparamref name="T"/> that can be associated with <paramref name="target"/>.</returns>
        protected abstract Task BeginNavigationAsync(INavigationTarget target, NavigationType navigationType);
    }
}
