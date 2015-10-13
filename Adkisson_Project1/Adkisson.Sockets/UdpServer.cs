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
using System.Text;

namespace Adkisson.Sockets
{
    /// <summary>
    /// Concrete class to start a UDP server instance. Requires a host IP address and port to start.
    /// Use Server.Factory to request an instance of this class.
    /// </summary>
    public class UdpServer : Server
    {
        internal UdpServer(ITransport protocol, IPAddress hostIpAddress, int port) : base(protocol, hostIpAddress, port)
        { }

        /// <summary>
        /// Starts the UDP server loop listening for client connections on the Server Socket, then
        /// echoing the client's message in uppercase on the same socket.
        /// </summary>
        public override void Start()
        {
            ListenSocket = Transport.GetSocket(AddressFamily.InterNetwork);

            //changes behavior of Windows UDP socket to timeout instead of returning
            //connection reset error if client cannot be reached for response
            const int SIO_UDP_CONNRESET = -1744830452;
            byte[] inValue = {0};
            byte[] outValue = {0};
            ListenSocket.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
            ListenSocket.Bind(IpEndPoint);

            var remote = (EndPoint) IpEndPoint;

            Console.WriteLine("UDP Server Socket awaiting message from client");
            Console.WriteLine("Send 'stop' message to stop this server.");
            
            //loop to listen for new connections. if msg received is stop, then server will be stopped
            //after replying to client.
            string msg = null;
            while (msg == null || !msg.Equals("stop", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("\r\nUDP Server Socket waiting at {0}", ListenSocket.LocalEndPoint);

                var buffer = new byte[Transport.BufferSize];
                var len = ListenSocket.ReceiveFrom(buffer, ref remote);
                msg = Encoding.ASCII.GetString(buffer, 0, len);

                Console.WriteLine("\r\nUDP message received from {0}: {1}", remote, msg);

                // Echo the message back to the client IN UPPERCASE.
                msg = msg.ToUpper();
                buffer = Encoding.ASCII.GetBytes(msg);
                ListenSocket.SendTo(buffer, len, SocketFlags.None, remote);
                Console.WriteLine("UDP Returned message to {0}: {1}", remote, msg);
            }
        }
    }
}