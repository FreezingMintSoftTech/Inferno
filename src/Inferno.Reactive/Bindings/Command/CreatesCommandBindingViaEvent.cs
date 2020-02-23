﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Inferno
{
    /// <summary>
    /// This binder is the default binder for connecting to arbitrary events.
    /// </summary>
    public class CreatesCommandBindingViaEvent : ICreatesCommandBinding
    {
        // NB: These are in priority order
        private static readonly List<(string name, Type type)> defaultEventsToBind = new List<(string name, Type type)>
        {
            ("Click", typeof(EventArgs)),
            ("TouchUpInside", typeof(EventArgs)),
            ("MouseUp", typeof(EventArgs)),
        };

        /// <inheritdoc/>
        public int GetAffinityForObject(Type type, bool hasEventTarget)
        {
            if (hasEventTarget)
            {
                return 5;
            }

            return defaultEventsToBind.Any(x =>
            {
                var ei = type.GetRuntimeEvent(x.name);
                return ei != null;
            }) ? 3 : 0;
        }

        /// <inheritdoc/>
        public IDisposable BindCommandToObject(ICommand command, object target, IObservable<object> commandParameter)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var type = target.GetType();
            var eventInfo = defaultEventsToBind
                .Select(x => new { EventInfo = type.GetRuntimeEvent(x.name), Args = x.type })
                .FirstOrDefault(x => x.EventInfo != null);

            if (eventInfo == null)
            {
                throw new Exception(
                       $"Couldn't find a default event to bind to on {target.GetType().FullName}, specify an event explicitly");
            }

            var mi = GetType().GetRuntimeMethods().First(x => x.Name == "BindCommandToObject" && x.IsGenericMethod);
            mi = mi.MakeGenericMethod(eventInfo.Args);

            return (IDisposable)mi.Invoke(this, new[] { command, target, commandParameter, eventInfo.EventInfo.Name });
        }

        /// <inheritdoc/>
        public IDisposable BindCommandToObject<TEventArgs>(ICommand command, object target, IObservable<object> commandParameter, string eventName)
        {
            var ret = new CompositeDisposable();

            object latestParameter = null;
            var evt = Observable.FromEventPattern<TEventArgs>(target, eventName);

            ret.Add(commandParameter.Subscribe(x => latestParameter = x));

            ret.Add(evt.Subscribe(ea =>
            {
                if (command.CanExecute(latestParameter))
                {
                    command.Execute(latestParameter);
                }
            }));

            return ret;
        }
    }
}