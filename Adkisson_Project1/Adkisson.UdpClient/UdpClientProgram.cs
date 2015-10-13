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
using Adkisson.Sockets;

namespace Adkisson.UdpClient
{
    internal class UdpClientProgram
    {
        /// <summary>
        /// Main entry point for UDP Client console application.
        /// Asks user for port and IP address of host and starts UDP client.
        /// The default host IP address is 127.0.0.1 (loopback). If running on a 
        /// remote host, be certain to include the host's IP address and open
        /// the firewall port to the host for the port specified below (default = 12000).
        /// Once the parameters are present, you can send a message to the UDP Server.
        /// The server, if found, will return the message in uppercase.
        /// If the server is not found, an exception will be shown.
        /// 
        /// Enter a blank line to exit.
        /// </summary>
        private static void Main()
        {
            Console.WriteLine("==========");
            Console.WriteLine("UDP CLIENT");
            Console.WriteLine("==========\r\n");

            //get host port - default = 12000
            Console.Write("\r\nUDP Host Port [Default=12000]: ");
            var sPort = Console.ReadLine();
            int port;
            int.TryParse(sPort, out port);
            port = port == 0 ? 12000 : port;
            Console.WriteLine("UDP Host Port: " + port);

            //get host ip or hostname - default = 127.0.0.1 loopback
            Console.Write("\r\nUDP Host [Default=127.0.0.1]: ");
            var host = Console.ReadLine();
            host = string.IsNullOrWhiteSpace(host) ? "127.0.0.1" : host;
            Console.WriteLine("UDP Host: " + host);

            //get udp protocol
            var udp = Transport.Factory(ProtocolType.Udp, 1024);

            //show local IP addresses
            Transport.ListLocalIpAddresses();

            //continue sending messages until [blank line] is entered
            var cont = true;
            while (cont)
            {
                Console.Write("\r\nUDP Message [blank=stop]: ");
                var msg = Console.ReadLine();
                cont = !string.IsNullOrWhiteSpace(msg);
                if (!cont) continue;

                //create client for udp protocol, target host and port
                var client = Client.Factory(udp, host, port);

                //connect, then send msg. send also listens for a response from server.
                client.Connect();
                client.Send(msg);
            }

            Console.WriteLine("\r\nUDP Client Stopped. Press any key.");
            Console.ReadKey();
        }
    }
}