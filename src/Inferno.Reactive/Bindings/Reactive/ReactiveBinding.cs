﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the ReactiveUI project root for full license information.

// This file may have been changed since it was pulled from the ReactiveUI repository.

using Inferno.Core;
using System;
using System.Linq.Expressions;

namespace Inferno
{
    internal class ReactiveBinding<TView, TViewModel, TValue> : IReactiveBinding<TView, TViewModel, TValue>
        where TViewModel : class
        where TView : IViewFor
    {
        private readonly IDisposable _bindingDisposable;

        public ReactiveBinding(
            TView view,
            TViewModel viewModel,
            Expression viewExpression,
            Expression viewModelExpression,
            IObservable<TValue> changed,
            BindingDirection direction,
            IDisposable bindingDisposable)
        {
            View = view;
            ViewModel = viewModel;
            ViewExpression = viewExpression;
            ViewModelExpression = viewModelExpression;
            Direction = direction;
            Changed = changed;

            _bindingDisposable = bindingDisposable;
        }

        /// <inheritdoc />
        public TViewModel ViewModel { get; }

        /// <inheritdoc />
        public Expression ViewModelExpression { get; }

        /// <inheritdoc />
        public TView View { get; }

        /// <inheritdoc />
        public Expression ViewExpression { get; }

        /// <inheritdoc />
        public IObservable<TValue> Changed { get; }

        /// <inheritdoc />
        public BindingDirection Direction { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of resources inside the class.
        /// </summary>
        /// <param name="isDisposing">If we are disposing managed resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _bindingDisposable?.Dispose();
            }
        }
    }
}
