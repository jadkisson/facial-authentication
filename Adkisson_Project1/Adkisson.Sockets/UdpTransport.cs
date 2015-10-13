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
    /// Concrete class to perform UDP transport. The default buffer size is 1024 bytes.
    /// Use static Transport.Factory to request an instance of this class.
    /// </summary>
    public class UdpTransport : Transport
    {
        internal UdpTransport(int bufferSize = 1024) : base(ProtocolType.Udp, bufferSize)
        {
        }
    }
}