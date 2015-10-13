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
    /// Concrete class to send messages from a UDP client to a UDP server. Requires a host IP address and port to start.
    /// Use Client.Factory to request an instance of this class.
    /// </summary>
    public class UdpClient : Client
    {
        internal UdpClient(ITransport protocol, string hostname, int port)
            : base(protocol, hostname, port)
        { }

        /// <summary>
        /// Sets up a UDP socket connection to a UDP server
        /// </summary>
        /// <returns></returns>
        public override bool Connect()
        {
            try
            {
                ClientSocket = Transport.GetSocket(IpEndPoint.AddressFamily);

                //setup socket to timeout instead of return useless "connection reset"
                const int SIO_UDP_CONNRESET = -1744830452;
                byte[] inValue = { 0 };
                byte[] outValue = { 0 };
                ClientSocket.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
                ClientSocket.SendTimeout = 1000;
                ClientSocket.ReceiveTimeout = 1000;

                ClientSocket.Connect(IpEndPoint);
                Console.WriteLine("\r\nUDP Client socket created from {0} to {1}", ClientSocket.LocalEndPoint, IpEndPoint);
            }
            catch (SocketException se)
            {
                Console.WriteLine("UDP Connection failure to {0}: {1}", IpEndPoint, se.SocketErrorCode);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sends a message to the target host/port and listens for a response.
        /// If host is not available, then an exception will be shown.
        /// </summary>
        /// <param name="message"></param>
        public override void Send(string message)
        {
            var msgBytes = Encoding.ASCII.GetBytes(message);
            Console.WriteLine("UDP Sent to {0}: {1}", IpEndPoint, message);
            ClientSocket.Send(msgBytes);

            try
            {
                var buffer = new byte[Transport.BufferSize];
                var len = ClientSocket.Receive(buffer);
                var recMsg = Encoding.ASCII.GetString(buffer, 0, len);
                Console.WriteLine("UDP Response from {0}: {1}", IpEndPoint, recMsg);
            }
            catch (SocketException se)
            {
                Console.WriteLine("UDP Exception: {0}", se.SocketErrorCode);
            }
        }
    }
}