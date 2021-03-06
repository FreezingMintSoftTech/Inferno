﻿using System.Threading.Tasks;

namespace Inferno
{
    /// <summary>
    /// Denotes an object that can be closed.
    /// </summary>
    public interface IClose
    {
        /// <summary>
        /// Tries to close this instance.
        /// </summary>
        Task TryCloseAsync();

        /// <summary>
        /// Indicates if this instance is closed (ie its lifecycle is terminated).
        /// </summary>
        bool IsClosed { get; }
    }
}