﻿using System;

namespace Inferno.Core
{
    /// <summary>
    /// EventArgs sent during activation.
    /// </summary>
    public class ActivationEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates whether the sender was initialized in addition to being activated.
        /// </summary>
        public bool WasInitialized { get; set; }
    }
}
