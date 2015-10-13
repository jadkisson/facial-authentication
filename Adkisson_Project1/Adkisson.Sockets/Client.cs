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

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Adkisson.Sockets
{
    /// <summary>
    /// Abstract class to provide basic framework for a client. Accepts the protocol, host IP, and host port.
    /// Also provides a static Factory method to return a new client of a given type UDP or TCP.
    /// </summary>
    public abstract class Client : Common, IClient
    {
        protected Client(ITransport protocol, string hostname, int port) : base(protocol)
        {
            IPAddress hostIp;
            if (!IPAddress.TryParse(hostname, out hostIp))
            {
                var host = Dns.GetHostEntry(hostname);
                hostIp = host.AddressList.First(o => o.AddressFamily == AddressFamily.InterNetwork);
            }

            IpEndPoint = new IPEndPoint(hostIp, port);
        }

        /// <summary>
        /// Returns the socket created to establish a connection with a host located at the IpEndPoint property
        /// </summary>
        protected Socket ClientSocket { get; set; }

        /// <summary>
        /// Abstract class to open a socket connection to a remote host. Implemented by the concrete class.
        /// </summary>
        /// <returns></returns>
        public abstract bool Connect();

        /// <summary>
        /// Closes the socket to a remote host. SocketShutdown.Both closes both outbound and inbound communications.
        /// </summary>
        public void Disconnect()
        {
            ClientSocket.Shutdown(SocketShutdown.Both);
        }

        /// <summary>
        /// Abstract class to send a message to the socket held by the ClientSocket property.
        /// Implemented by the concrete class.
        /// </summary>
        /// <param name="message"></param>
        public abstract void Send(string message);
        
        /// <summary>
        /// Static factory class to easily create a new client for a given protocol, target host, and port.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IClient Factory(ITransport protocol, string hostName, int port)
        {
            switch (protocol.ProtocolType)
            {
                case ProtocolType.Udp:
                    return new UdpClient(protocol, hostName,port);
                case ProtocolType.Tcp:
                    return new TcpClient(protocol, hostName, port);
                default:
                    throw new ArgumentException("Unsupported Protocol Type: " + protocol.ProtocolType, "protocol");
            }
        }
    }
}