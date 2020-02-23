﻿using System.ComponentModel;

namespace Inferno
{
    /// <summary>
    /// Event arguments for when a property is changing.
    /// </summary>
    /// <typeparam name="TSender">The sender type.</typeparam>
    public class ReactivePropertyChangingEventArgs<TSender> : PropertyChangingEventArgs, IReactivePropertyChangedEventArgs<TSender>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactivePropertyChangingEventArgs{TSender}"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ReactivePropertyChangingEventArgs(TSender sender, string propertyName)
            : base(propertyName)
        {
            Sender = sender;
        }

        /// <summary>
        /// Gets the sender which triggered the Reactive property changed event.
        /// </summary>
        /// <inheritdoc/>
        public TSender Sender { get; }
    }
}