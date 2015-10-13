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
    /// Abstract class to provide a common framework for working with various protocols in a client or server.
    /// Describes the protocol type and buffer size, plus provides new sockets of a given type.
    /// Also provides a static Factory to easily request a new protocol instance.
    /// </summary>
    public abstract class Transport : ITransport
    {
        /// <summary>
        /// Use Transport.Factory to call the constructor for a Transport class.
        /// </summary>
        /// <param name="protocolType"></param>
        /// <param name="bufferSize"></param>
        protected Transport(ProtocolType protocolType, int bufferSize)
        {
            ProtocolType = protocolType;
            BufferSize = bufferSize;
        }

        /// <summary>
        /// Returns TCP or UDP.
        /// </summary>
        public ProtocolType ProtocolType { get; private set; }

        /// <summary>
        /// The buffer size of messages sent and received to/from servers and clients.
        /// </summary>
        public int BufferSize { get; private set; }

        /// <summary>
        /// Returns a new socket for the correct protocol and address family of the IP binding (IP4 or IP6)
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        public Socket GetSocket(AddressFamily addressFamily)
        {
            var socketType = ProtocolType == ProtocolType.Tcp ? SocketType.Stream : SocketType.Dgram;
            return new Socket(addressFamily, socketType, ProtocolType);
        }

        //Lists all of the IP addresses on the machine
        public static void ListLocalIpAddresses()
        {
            try
            {
                // Get host name
                String strHostName = Dns.GetHostName();

                // Find host by name
                IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);


                // Enumerate IP addresses
                Console.WriteLine("\r\nLocal IP addresses:");
                foreach (IPAddress ipaddress in iphostentry.AddressList.Where(o => o.AddressFamily == AddressFamily.InterNetwork).Distinct().OrderBy(o => o.ToString()))
                {
                    Console.WriteLine("- " + ipaddress);
                }
            }
            catch (Exception)
            {
                //not critical... ignore
            }
            
        }

        /// <summary>
        /// Static factory to easily request a new instance of a transport protocol.
        /// </summary>
        /// <param name="protocolType"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static ITransport Factory(ProtocolType protocolType, int bufferSize)
        {
            switch (protocolType)
            {
                case ProtocolType.Udp:
                    return new UdpTransport(bufferSize);
                case ProtocolType.Tcp:
                    return new TcpTransport(bufferSize);
                default:
                    throw new ArgumentException("Unsupported Protocol Type: " + protocolType, "protocolType");
            }
        }
    }
}