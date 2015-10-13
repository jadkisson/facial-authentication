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
    /// Concrete class to start a TCP server instance. Requires a host IP address and port to start.
    /// Use Server.Factory to request an instance of this class.
    /// </summary>
    public class TcpServer : Server
    {
        internal TcpServer(ITransport protocol, IPAddress hostIpAddress, int port) : base(protocol, hostIpAddress, port)
        {
        }

        /// <summary>
        /// The TCP socket opened to communicate with the client. Not the same as the ListenSocket, which
        /// welcomes new connections.
        /// </summary>
        protected Socket ClientSocket { get; set; }

        /// <summary>
        /// Starts the TCP server loop listening for client connections on the Server Socket, then
        /// communicating with the client on that client's TCP Connection Socket.
        /// </summary>
        public override void Start()
        {
            ListenSocket = Transport.GetSocket(IPAddress.Any.AddressFamily);
            ListenSocket.Bind(IpEndPoint);
            ListenSocket.Listen(10);
            Console.WriteLine("TCP Server Socket awaiting connection from client");

            Console.WriteLine("Send 'stop' message to stop this server.");

            //loop to listen for new connections. if msg received is stop, then server will be stopped
            //after replying to client.
            string msg = null;
            while (msg == null || !msg.Equals("stop", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("\r\nTCP Server Socket waiting at {0}", ListenSocket.LocalEndPoint);
                ClientSocket = ListenSocket.Accept();
                Console.WriteLine("\r\nTCP Connection Socket established with {0}", ClientSocket.RemoteEndPoint);

                var buffer = new byte[Transport.BufferSize];
                var len = ClientSocket.Receive(buffer);
                msg = Encoding.ASCII.GetString(buffer, 0, len);

                Console.WriteLine("TCP Message received from {0}: {1}", ClientSocket.RemoteEndPoint, msg);

                // Echo the message back to the client IN UPPERCASE.
                msg = msg.ToUpper();
                Console.WriteLine("TCP Returned message to {0}: {1}", ClientSocket.RemoteEndPoint, msg);
                ClientSocket.Send(Encoding.ASCII.GetBytes(msg));

                // Close connection socket, server socket remains open
                Console.WriteLine("TCP Connection Socket to {0} closed", ClientSocket.RemoteEndPoint);
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
            }

            Stop();
        }
    }
}