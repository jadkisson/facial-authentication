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
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Adkisson.Sockets;

namespace Adkisson.TcpClient
{
    internal class TcpClientProgram
    {
        /// <summary>
        /// Main entry point for TCP Client console application.
        /// Asks user for port and IP address of host and starts TCP client.
        /// The default host IP address is 127.0.0.1 (loopback). If running on a 
        /// remote host, be certain to include the host's IP address and open
        /// the firewall port to the host for the port specified below (default = 11000).
        /// Once the parameters are present, you can send a message to the TCP Server.
        /// The server, if found, will return the message in uppercase.
        /// If the server is not found, an exception will be shown.
        /// 
        /// Enter a blank line to exit.
        /// </summary>
        private static void Main()
        {
            Console.WriteLine("==========");
            Console.WriteLine("TCP CLIENT");
            Console.WriteLine("==========\r\n");

            //get host port - default = 11000
            Console.Write("TCP Host Port [Default=11000]: ");
            var sPort = Console.ReadLine();
            int port;
            int.TryParse(sPort, out port);
            port = port == 0 ? 11000 : port;
            Console.WriteLine("TCP Host Port: " + port + "\r\n");

            //get host ip or hostname - default = 127.0.0.1 loopback
            Console.Write("TCP Host [Default=127.0.0.1]: ");
            var host = Console.ReadLine();
            host = string.IsNullOrWhiteSpace(host) ? "127.0.0.1" : host;
            Console.WriteLine("TCP Host: " + host);

            //get tcp protocol
            var tcp = Transport.Factory(ProtocolType.Tcp, 1024);

            //show local IP addresses
            Transport.ListLocalIpAddresses();
            

            //continue sending messages until [blank line] is entered
            var cont = true;
            while (cont)
            {
                Console.Write("\r\nTCP Message [blank=stop]: ");
                var msg = Console.ReadLine();
                cont = !string.IsNullOrWhiteSpace(msg);
                if (!cont) continue;

                //create client for udp protocol, target host and port
                var client = Client.Factory(tcp, host, port);

                //if we were able to TCP handshake with the server, send message, then disconnect
                //send also listens for server response
                if (client.Connect())
                {
                    Console.WriteLine("TCP Connected: " + host + ": " + port);
                    client.Send(msg);
                    client.Disconnect();
                }
            }

            Console.WriteLine("\r\nTCP Client Stopped. Press any key.");
            Console.ReadKey();
        }
    }
}