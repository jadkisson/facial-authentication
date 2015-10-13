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
using System.Net.Sockets;
using System.Text;

namespace Adkisson.Sockets
{
    /// <summary>
    /// Concrete class to send messages from a TCP client to a TCP server. Requires a host IP address and port to start.
    /// Use Client.Factory to request an instance of this class.
    /// </summary>
    public class TcpClient : Client
    {
        internal TcpClient(ITransport protocol, string hostname, int port) : base(protocol, hostname, port)
        {
        }

        /// <summary>
        /// Sets up a TCP socket connection to a TCP server
        /// </summary>
        /// <returns></returns>
        public override bool Connect()
        {
            ClientSocket = Transport.GetSocket(IpEndPoint.AddressFamily);

            try
            {
                ClientSocket.Connect(IpEndPoint);
                Console.WriteLine("TCP Connection Socket between {0} and {1} established", ClientSocket.LocalEndPoint, IpEndPoint);
                return true;
            }
            catch (SocketException se)
            {
                Console.WriteLine("TCP Connection failure to {0}: {1}", IpEndPoint, se.SocketErrorCode);
                return false;
            }
        }

        /// <summary>
        /// Sends a message to the target host/port and listens for a response.
        /// If host is not available, then an exception will be shown.
        /// </summary>
        /// <param name="message"></param>
        public override void Send(string message)
        {
            try
            {
                var msgBytes = Encoding.ASCII.GetBytes(message);
                Console.WriteLine("TCP Sent to {0}: {1}", IpEndPoint, message);
                ClientSocket.Send(msgBytes);

                var buffer = new byte[Transport.BufferSize];
                var len = ClientSocket.Receive(buffer);
                var recMsg = Encoding.ASCII.GetString(buffer, 0, len);
                Console.WriteLine("TCP Response from {0}: {1}", IpEndPoint, recMsg);
            }
            catch (SocketException se)
            {
                Console.WriteLine("TCP Exception: {0}", se.SocketErrorCode);
            }
        }
    }
}