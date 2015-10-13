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
    /// Interface to specify the parameters of a transport protocol that can be used by an IClient or IServer instance.
    /// </summary>
    public interface ITransport
    {
        ProtocolType ProtocolType { get; }
        int BufferSize { get; }
        Socket GetSocket(AddressFamily addressFamily);
    }
}