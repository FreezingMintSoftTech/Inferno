﻿using Inferno.DialogManagement.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Inferno
{
    public class DialogManager : IDialogManager
    {
        private readonly Dispatcher _dispatcher;
        private readonly IWindowManager _windowManager;

        public DialogManager(IWindowManager windowManager)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _windowManager = windowManager;
        }

        public virtual Task<bool?> ShowDialog<TChoice>(IDialogViewModel<TChoice> dialogViewModel, IDialogSettings settings)
        {
            OnUIThread(async () => await _windowManager.ShowDialogAsync(dialogViewModel, null, ConvertToDictionary(settings)));

            return Task.FromResult(dialogViewModel.DialogResult);
        }

        #region MessageBox

        /// <summary>
        /// Provides fine grained control by specifying the buttons as <see cref="ButtonContext&lt;ButtonChoice&gt;"/>.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="dialogType"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public virtual Task<bool?> ShowMessageBox(string title, string message, DialogType dialogType, params ButtonContext<ButtonChoice>[] buttons)
        {
            var messageBoxViewModel =
                new DialogViewModel<ButtonChoice>(
                    title,
                    new MessageBoxViewModel(message), 
                    buttons,
                    dialogType);

            return ShowDialog(messageBoxViewModel, MessageBoxSettings);
        }

        /// <summary>
        /// Buttons are generated by conventions, based on the order of the passed in <see cref="ButtonChoice"/>s.
        /// The left most button is used to confirm and the right most button to cancel.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="dialogType"></param>
        /// <param name="choices"></param>
        /// <returns></returns>
        public virtual Task<bool?> ShowMessageBox(string title, string message, DialogType dialogType, params ButtonChoice[] choices)
        {
            var buttons = ConvertToButtons(choices);

            return ShowMessageBox(title, message, dialogType, buttons);
        }

        protected virtual IDialogSettings MessageBoxSettings => new DialogSettings
        {
            Width = 350,
            Height = 200,
            ResizeMode = ResizeMode.NoResize,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        #endregion MessageBox

        #region Buttons

        /// <summary>
        /// Apply button behavior by UX conventions, ie the left most button is used to confirm and the right most button to cancel.
        /// </summary>
        /// <param name="choices"></param>
        /// <returns></returns>
        internal static ButtonContext<TChoice>[] ConvertToButtons<TChoice>(params TChoice[] choices)
        {
            var result = new ButtonContext<TChoice>[choices.Length];

            if (choices == null || !choices.Any())
            {
                throw new ArgumentException($"{nameof(choices)} is null or empty");
            }
            else if (choices.Length == 1)
            {
                result[0] = new ButtonContext<TChoice>(choices.Single(), true, false);
            }
            else
            {
                for (int i = 0; i < choices.Length; i++)
                {
                    var choice = choices[i];

                    if (i == 0)
                    {
                        result[i] = new ButtonContext<TChoice>(choice, true, false);
                    }
                    else if (i == choices.Length)
                    {
                        result[i] = new ButtonContext<TChoice>(choice, false, true);
                    }
                    else
                    {
                        result[i] = new ButtonContext<TChoice>(choice, false, false);
                    }
                }
            }

            return result;
        }

        #endregion Buttons

        #region DialogSettings

        protected virtual IDictionary<string, object> ConvertToDictionary(IDialogSettings settings)
        {
            var dictionary = new Dictionary<string, object>();

            // Only add non-default values to the dictionary
            AddNonDefault(nameof(Window.Width), settings.Width, dictionary);
            AddNonDefault(nameof(Window.MinWidth), settings.MinWidth, dictionary);
            AddNonDefault(nameof(Window.Height), settings.Height, dictionary);
            AddNonDefault(nameof(Window.MinHeight), settings.MinHeight, dictionary);
            dictionary.Add(nameof(Window.ResizeMode), settings.ResizeMode);
            dictionary.Add(nameof(Window.WindowStartupLocation), settings.WindowStartupLocation);

            return dictionary;
        }

        private static void AddNonDefault(string propertyName, double value, Dictionary<string, object> dictionary)
        {
            if (Math.Abs(value - DialogSettings.Default) > 0.00001) // Compensating for rounding errors
            {
                dictionary.Add(propertyName, value);
            }
        }

        #endregion DialogSettings

        #region Dispatcher

        /// <summary>
        /// Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected void OnUIThread(Action action)
        {
            if (CheckAccess())
                action();
            else
            {
                Exception exception = null;
                Action method = () => {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                };
                _dispatcher.Invoke(method);
                if (exception != null)
                    throw new System.Reflection.TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
            }
        }

        protected bool CheckAccess()
        {
            return _dispatcher == null || _dispatcher.CheckAccess();
        }

        #endregion Dispatcher
    }
}