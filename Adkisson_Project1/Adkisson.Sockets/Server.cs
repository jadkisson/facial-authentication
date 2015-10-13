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
using System.Net;
using System.Net.Sockets;

namespace Adkisson.Sockets
{
    /// <summary>
    /// Abstract class to provide basic framework for a server. Accepts the protocol, host IP, and port.
    /// Also provides a static Factory method to return a new server of a given type UDP or TCP.
    /// </summary>
    public abstract class Server : Common, IServer
    {
        protected Server(ITransport protocol, IPAddress hostIpAddress, int port) : base(protocol)
        {
            HostIpAddress = hostIpAddress;
            Port = port;

            IpEndPoint = new IPEndPoint(hostIpAddress, port);
        }

        /// <summary>
        /// The socket that listens for new client connections
        /// </summary>
        protected Socket ListenSocket { get; set; }

        /// <summary>
        /// The IP address to which the server is bound. 127.0.0.1 for loopback or use a specific IP if providing availability to remote hosts.
        /// </summary>
        public IPAddress HostIpAddress { get; private set; }

        /// <summary>
        /// The port on which the server is listening.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// When called, launches the server listening loop.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops the server and closes all connections.
        /// </summary>
        public virtual void Stop()
        {
            Console.WriteLine(ProtocolType + " Server Stopped");
        }

        /// <summary>
        /// Returns a concrete instance of a server for a specific protocol (UDP or TCP), ip address, and port.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="hostIpAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IServer Factory(ITransport protocol, IPAddress hostIpAddress, int port)
        {
            switch (protocol.ProtocolType)
            {
                case ProtocolType.Udp:
                    return new UdpServer(protocol, hostIpAddress, port);
                case ProtocolType.Tcp:
                    return new TcpServer(protocol, hostIpAddress, port);
                default:
                    throw new ArgumentException("Unsupported Protocol Type: " + protocol.ProtocolType, "protocol");
            }
        }
    }
}