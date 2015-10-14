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
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Adkisson.Sockets
{
    /// <summary>
    ///     Concrete class to start a TCP server instance. Requires a host IP address and port to start.
    ///     Use Server.Factory to request an instance of this class.
    /// </summary>
    public class TcpServer : Server
    {
        internal TcpServer(ITransport protocol, IPAddress hostIpAddress, int port)
            : base(protocol, hostIpAddress, port)
        {
            Connections = 0;
        }

        protected int Connections { get; set; }

        /// <summary>
        ///     Starts the TCP server loop listening for client connections on the Server Socket, then
        ///     communicating with the client on that client's TCP Connection Socket.
        /// </summary>
        public override void Start()
        {
            ListenSocket = Transport.GetSocket(IPAddress.Any.AddressFamily);
            ListenSocket.Bind(IpEndPoint);
            ListenSocket.Listen(10);
            Console.WriteLine("TCP Server Socket awaiting connection from client");
            Console.WriteLine("Press ESC to stop this server");

            var shutdownToken = new CancellationTokenSource();
            var socketTasks = new ConcurrentBag<Task>();

            var serverSocketTask = Task.Run(() =>
            {
                //loop to listen for TCP connections while token isn't
                while (!shutdownToken.IsCancellationRequested)
                {
                    try
                    {
                        Console.WriteLine("\r\nTCP Server Socket waiting at {0}", ListenSocket.LocalEndPoint);

                        var newClientSocket = ListenSocket.Accept();

                        Connections++;
                        Console.WriteLine("\r\nConnections: {0}", Connections);
                        var client = new TcpClientConnection(newClientSocket, Transport.BufferSize);
                        var clientTask = Task.Factory.StartNew(() =>
                        {
                            client.Execute();
                            Task toRemove;
                            socketTasks.TryTake(out toRemove); //remove from concurrent bag
                            Connections--;
                            Console.WriteLine("\r\nConnections: {0}", Connections);
                        }, shutdownToken.Token);
                        socketTasks.Add(clientTask);
                    }
                    catch (OperationCanceledException)
                    {
                        //time to shutdown
                    }
                }
            }, shutdownToken.Token); //cancel this task when token is flagged
            socketTasks.Add(serverSocketTask);

            //wait for connections
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
            }

            //no more waiting... shutdown
            Console.WriteLine("Stopping... closing open TCP connections");
            shutdownToken.CancelAfter(1000); //give tasks 1000ms to finish - then throw them if necessary
            Task.WaitAll(socketTasks.ToArray(), 1000); //wait up to 5 seconds for all tasks to end, then give up

            Stop();
        }
    }
}