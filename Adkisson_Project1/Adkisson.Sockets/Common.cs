//==========================================================
//
// CS 6027 Networking Project
// by Jeff Adkisson, jadkiss1@students.kennesaw.edu
//
// All code written by Jeff Adkisson
//
// Fall, 2015, KSU
// Dr. Jung
//
//==========================================================

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Adkisson.Sockets
{
    /// <summary>
    /// Abstract class implemented by both Server and Client to provide commmon properties used by both types, including
    /// the ITransport protocol implemented for the process and the IP endpoint to which either the client
    /// is contacting or to which the server is bound.
    /// </summary>
    public abstract class Common
    {
        protected Common(ITransport protocol)
        {
            Debug.Assert(protocol != null);
            Transport = protocol;
        }

        /// <summary>
        /// Helper to return an enum describing the current transport protocol
        /// </summary>
        public ProtocolType ProtocolType
        {
            get { return Transport.ProtocolType; }
        }

        /// <summary>
        /// The ITransport protocol the client or server is using for communication.
        /// </summary>
        protected ITransport Transport { get; set; }

        /// <summary>
        /// For clients, this is the IP target to which we are connecting.
        /// For servers, this is the IP address to which the server is bound (listening).
        /// </summary>
        protected IPEndPoint IpEndPoint { get; set; }
    }
}