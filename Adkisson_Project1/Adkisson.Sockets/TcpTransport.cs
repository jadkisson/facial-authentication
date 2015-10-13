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

using System.Net.Sockets;

namespace Adkisson.Sockets
{
    /// <summary>
    /// Concrete class to perform TCP transport. The default buffer size is 1024 bytes.
    /// Use static Transport.Factory to request an instance of this class.
    /// </summary>
    public class TcpTransport : Transport
    {
        internal TcpTransport(int bufferSize = 1024)
            : base(ProtocolType.Tcp, bufferSize)
        {
        }
    }
}