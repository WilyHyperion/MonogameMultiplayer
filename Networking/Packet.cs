

using System;
using System.ComponentModel.Design;
using System.Net;
using System.Runtime.InteropServices;

namespace Server;


public abstract class Packet
{
    public IPEndPoint Sender;
    public byte[] data;
    public Packet(byte[] data, IPEndPoint from)
    {
        this.data = data;
        this.Sender = from;
    }
    public Packet(byte[] data)
    {
        this.data = data;
    }
    public Packet(){
    }

    /// <summary>
    /// The code to get the contents of this package
    /// </summary>
    /// <returns></returns>
    public abstract Byte[] Send();
}

/// <summary>
/// Represents a packet that gets sent from the server to the clients
/// </summary>
public abstract class ServerOriginatingPacket : Packet{
        /// <summary>
    /// code that runs on the server when this packet is sent, assume data is set
    /// </summary>
    public abstract void  ClientReceive();
}

/// <summary>
/// A packet sent from the clients to the server
/// </summary>
public abstract class ClientOrigniatingPacket : Packet {
    protected ClientOrigniatingPacket()
    {
    }

    protected ClientOrigniatingPacket(byte[] data) : base(data)
    {
    }

    protected ClientOrigniatingPacket(byte[] data, IPEndPoint from) : base(data, from)
    {
    }

    /// <summary>
    /// The CLIENT SIDED code to run when this packet is recived. Assume data is set
    /// </summary>
    public virtual void ServerReceive(ServerPlayer sender){

    }
}