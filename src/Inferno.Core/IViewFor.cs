﻿namespace Inferno.Core
{
    /// <summary>
    /// This base class is mostly used by the Framework. Implement <see cref="IViewFor{T}"/>
    /// instead.
    /// </summary>
    public interface IViewFor
    {
        /// <summary>
        /// Gets or sets the View Model associated with the View.
        /// </summary>
        object ViewModel { get; set; }
    }

    /// <summary>
    /// Implement this interface on your Views to support Routing and Binding.
    /// </summary>
    /// <typeparam name="T">The type of ViewModel.</typeparam>
    public interface IViewFor<T> : IViewFor where T : class
    {
        /// <summary>
        /// Gets or sets the ViewModel corresponding to this specific View. This should be
        /// a DependencyProperty if you're using XAML.
        /// </summary>
        new T ViewModel { get; set; }
    }
}