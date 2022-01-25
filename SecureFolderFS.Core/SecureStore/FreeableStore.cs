﻿using System;

namespace SecureFolderFS.Core.SecureStore
{
    internal abstract class FreeableStore<TImplementation> : IDisposable, IEquatable<TImplementation>
        where TImplementation : class
    {
        public override bool Equals(object obj)
        {
            return Equals(obj as TImplementation);
        }

        public abstract TImplementation CreateCopy();

        public abstract bool Equals(TImplementation other);

        public abstract override int GetHashCode();

        protected abstract void SecureFree();

        public void Dispose()
        {
            SecureFree();
        }
    }
}