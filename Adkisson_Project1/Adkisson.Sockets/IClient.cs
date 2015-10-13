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
    /// Interface to specify the parameters of a Client that can implement a transport protocol of type ITransport.
    /// </summary>
    public interface IClient 
    {
        ProtocolType ProtocolType { get; }
        bool Connect();
        void Disconnect();
        void Send(string message);
    }
}