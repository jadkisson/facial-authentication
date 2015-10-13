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

namespace Adkisson.TcpServer
{
    internal class TcpServerProgram
    {
        /// <summary>
        /// Main entry point for TCP Server console application.
        /// Asks user for port and IP address of host and starts TCP server.
        /// The default IP address is 127.0.0.1 (loopback). If running on a 
        /// remote host, be certain to include the host's IP address and open
        /// the firewall port to the host for the port specified below (default = 11000).
        /// 
        /// The server will transform all received messages into uppercase before
        /// responding to the client.
        /// </summary>
        private static void Main()
        {
            Console.WriteLine("==========");
            Console.WriteLine("TCP SERVER");
            Console.WriteLine("==========\r\n");

            //get host port - default = 12000
            Console.Write("Host TCP Port [Default=11000]: ");
            var sPort = Console.ReadLine();
            int port;
            int.TryParse(sPort, out port);
            port = port == 0 ? 11000 : port;
            Console.WriteLine("TCP Host Port: " + port);

            //show local IP addresses
            Transport.ListLocalIpAddresses();

            //get host ip or hostname - default = 127.0.0.1 loopback
            Console.Write("\r\nHost TCP IP Address [Default=127.0.0.1]: ");
            var host = Console.ReadLine();
            host = string.IsNullOrWhiteSpace(host) ? "127.0.0.1" : host;
            IPAddress hostIpAddress;
            if (!IPAddress.TryParse(host, out hostIpAddress))
            {
                hostIpAddress = new IPAddress(IPAddress.Parse("127.0.0.1").GetAddressBytes());
            }
            Console.WriteLine("Host TCP Address: " + string.Join(".", hostIpAddress.GetAddressBytes()) + "\r\n");

            //get TCP protoccol
            var tcp = Transport.Factory(ProtocolType.Tcp, 1024);

            //get server for protocol, host, and port, and start listen loop
            var server = Server.Factory(tcp, hostIpAddress, port);
            server.Start();
            
            //if user types [stop] as the message, TCP server will stop
            Console.WriteLine("\r\nTCP Server Stopped. Press any key.");
            Console.ReadKey();
        }
    }
}