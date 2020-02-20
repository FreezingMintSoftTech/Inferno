﻿using Inferno.Core;
using System;
using System.Linq.Expressions;

namespace Inferno
{
    /// <summary>
    /// This interface represents the result of a Bind/OneWayBind and gives
    /// information about the binding. When this object is disposed, it will
    /// destroy the binding it is describing (i.e. most of the time you won't
    /// actually care about this object, just that it is disposable).
    /// </summary>
    /// <typeparam name="TView">The view type.</typeparam>
    /// <typeparam name="TViewModel">The view model type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public interface IReactiveBinding<out TView, out TViewModel, out TValue> : IDisposable
        where TViewModel : class
        where TView : IViewFor
    {
        /// <summary>
        /// Gets the instance of the view model this binding is applied to.
        /// </summary>
        TViewModel ViewModel { get; }

        /// <summary>
        /// Gets an expression representing the propertyon the viewmodel bound to the view.
        /// This can be a child property, for example x.Foo.Bar.Baz in which case
        /// that will be the expression.
        /// </summary>
        Expression ViewModelExpression { get; }

        /// <summary>
        /// Gets the instance of the view this binding is applied to.
        /// </summary>
        TView View { get; }

        /// <summary>
        /// Gets an expression representing the property on the view bound to the viewmodel.
        /// This can be a child property, for example x.Foo.Bar.Baz in which case
        /// that will be the expression.
        /// </summary>
        Expression ViewExpression { get; }

        /// <summary>
        /// Gets an observable representing changed values for the binding.
        /// </summary>
        IObservable<TValue> Changed { get; }

        /// <summary>
        /// Gets the direction of the binding.
        /// </summary>
        BindingDirection Direction { get; }
    }
}
