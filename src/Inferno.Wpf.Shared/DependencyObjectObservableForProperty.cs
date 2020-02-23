﻿using Inferno.Core.Logging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;

namespace Inferno
{
    /// <summary>
    /// Creates a observable for a property if available that is based on a DependencyProperty.
    /// </summary>
    public class DependencyObjectObservableForProperty : ICreatesObservableForProperty
    {
        private readonly ILogger _logger;

        public DependencyObjectObservableForProperty(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public int GetAffinityForObject(Type type, string propertyName, bool beforeChanged = false)
        {
            if (!typeof(DependencyObject).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
            {
                return 0;
            }

            return GetDependencyProperty(type, propertyName) != null ? 4 : 0;
        }

        /// <inheritdoc/>
        public IObservable<IObservedChange<object, object>> GetNotificationForProperty(object sender, System.Linq.Expressions.Expression expression, string propertyName, bool beforeChanged = false, bool suppressWarnings = false)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            var type = sender.GetType();
            var dpd = DependencyPropertyDescriptor.FromProperty(GetDependencyProperty(type, propertyName), type);

            if (dpd == null)
            {
                if (!suppressWarnings)
                {
                    _logger.LogWarning(sender, "Couldn't find dependency property " + propertyName + " on " + type.Name);
                }

                throw new NullReferenceException("Couldn't find dependency property " + propertyName + " on " + type.Name);
            }

            return Observable.Create<IObservedChange<object, object>>(subj =>
            {
                var handler = new EventHandler((o, e) =>
                {
                    subj.OnNext(new ObservedChange<object, object>(sender, expression));
                });

                dpd.AddValueChanged(sender, handler);
                return Disposable.Create(() => dpd.RemoveValueChanged(sender, handler));
            });
        }

        private static DependencyProperty GetDependencyProperty(Type type, string propertyName)
        {
            var fi = type.GetTypeInfo().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(x => x.Name == propertyName + "Property" && x.IsStatic);

            if (fi != null)
            {
                return (DependencyProperty)fi.GetValue(null);
            }

            return null;
        }
    }
}