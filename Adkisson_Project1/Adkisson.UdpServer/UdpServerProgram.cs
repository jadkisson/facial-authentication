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
using Adkisson.Sockets;

namespace Adkisson.UdpServer
{
    internal class UdpServerProgram
    {
        /// <summary>
        /// Main entry point for UDP Server console application.
        /// Asks user for port and IP address of host and starts UDP server.
        /// The default IP address is 127.0.0.1 (loopback). If running on a 
        /// remote host, be certain to include the host's IP address and open
        /// the firewall port to the host for the port specified below (default = 12000).
        /// 
        /// The server will transform all received messages into uppercase before
        /// responding to the client.
        /// </summary>
        private static void Main()
        {
            Console.WriteLine("==========");
            Console.WriteLine("UDP SERVER");
            Console.WriteLine("==========\r\n");

            //get host port - default = 12000
            Console.Write("\r\nHost UDP Port [Default=12000]: ");
            var sPort = Console.ReadLine();
            int port;
            int.TryParse(sPort, out port);
            port = port == 0 ? 12000 : port;
            Console.WriteLine("Host UDP Port: " + port);

            //show local IP addresses
            Transport.ListLocalIpAddresses();

            //get host ip or hostname - default = 127.0.0.1 loopback
            Console.Write("\r\nHost UDP IP Address [Default=127.0.0.1]: ");
            var host = Console.ReadLine();
            host = string.IsNullOrWhiteSpace(host) ? "127.0.0.1" : host;
            IPAddress hostIpAddress;
            if (!IPAddress.TryParse(host, out hostIpAddress))
            {
                hostIpAddress = new IPAddress(IPAddress.Parse("127.0.0.1").GetAddressBytes());
            }
            Console.WriteLine("Host UDP Address: " + string.Join(".", hostIpAddress.GetAddressBytes()) + "\r\n");

            //get udp protocol
            var udp = Transport.Factory(ProtocolType.Udp, 1024);

            //get server for protocol, host, and port, and start listen loop
            var server = Server.Factory(udp, hostIpAddress, port);
            server.Start();

            //if user types [stop] as the message, UDP server will stop
            Console.WriteLine("\r\nUDP Server Stopped. Press any key.");
            Console.ReadKey();
        }
    }
}