﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the ReactiveUI project root for full license information.

// This file may have been changed since it was pulled from the ReactiveUI repository.

using System;
using System.Runtime.Serialization;

namespace Inferno
{
    /// <summary>
    /// Indicates that an object implementing <see cref="IHandleObservableErrors"/> has caused an error and nothing is attached
    /// to <see cref="IHandleObservableErrors.ThrownExceptions"/> to handle that error.
    /// </summary>
    [Serializable]
    public class UnhandledErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledErrorException"/> class.
        /// </summary>
        public UnhandledErrorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledErrorException"/> class.
        /// </summary>
        /// <param name="message">
        /// The exception message.
        /// </param>
        public UnhandledErrorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledErrorException"/> class.
        /// </summary>
        /// <param name="message">
        /// The exception message.
        /// </param>
        /// <param name="innerException">
        /// The exception that caused this exception.
        /// </param>
        public UnhandledErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledErrorException"/> class.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The serialization context.</param>
        protected UnhandledErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}