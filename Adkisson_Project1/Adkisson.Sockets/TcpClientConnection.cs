using System;
using System.Net.Sockets;
using System.Text;

namespace Adkisson.Sockets
{
    public class TcpClientConnection
    {
        public TcpClientConnection(Socket clientSocket, int bufferSize)
        {
            ClientSocket = clientSocket;
            BufferSize = bufferSize;
        }

        /// <summary>
        /// The TCP socket opened to communicate with the client. Not the same as the ListenSocket, which
        /// welcomes new connections.
        /// </summary>
        protected Socket ClientSocket { get; set; }

        protected int BufferSize { get; set; }

        public void Execute()
        {
            try
            {
                Console.WriteLine("\r\nTCP Connection Socket established with {0}", ClientSocket.RemoteEndPoint);

                //Thread.Sleep(2000);

                var buffer = new byte[BufferSize];
                var len = ClientSocket.Receive(buffer);
                var msg = Encoding.ASCII.GetString(buffer, 0, len);

                Console.WriteLine("TCP Message received from {0}: {1}", ClientSocket.RemoteEndPoint, msg);

                // Echo the message back to the client IN UPPERCASE.
                msg = msg.ToUpper();
                Console.WriteLine("TCP Returned message to {0}: {1}", ClientSocket.RemoteEndPoint, msg);
                ClientSocket.Send(Encoding.ASCII.GetBytes(msg));

            }
            catch (OperationCanceledException)
            {
                return; //operation was cancelled... server is shutting down
            }
            finally
            {
                // Close connection socket, server socket remains open
                Console.WriteLine("TCP Connection Socket to {0} closed", ClientSocket.RemoteEndPoint);
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();
            }

        }
    }
}