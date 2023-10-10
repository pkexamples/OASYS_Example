//-----------------------------------------------------------------------
// <copyright file="MismatchedSetupNameException.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------
namespace PhotonKinetics.OASYS.IO
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Exception that is thrown when the ID string of an OASYS.net
    /// <see cref="IDTaggedSetupType"/> object does not match its saved file name.
    /// </summary>
    [Serializable]
    public class MismatchedSetupNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MismatchedSetupNameException"/> class.
        /// </summary>
        public MismatchedSetupNameException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MismatchedSetupNameException"/> class.
        /// </summary>
        /// <param name="message">Sets the Message of the Exception.</param>
        public MismatchedSetupNameException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MismatchedSetupNameException"/> class.
        /// </summary>
        /// <param name="message">Sets the Message of the Exception.</param>
        /// <param name="inner">Sets the InnerException of the Exception.</param>
        public MismatchedSetupNameException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MismatchedSetupNameException"/> class.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> for serialization.</param>
        /// <param name="context">The <see cref="StreamingContext"/> for serialization.</param>
        protected MismatchedSetupNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // This constructor is needed for serialization when an
            // exception propagates from a remoting server to the client.
        }
    }
}
