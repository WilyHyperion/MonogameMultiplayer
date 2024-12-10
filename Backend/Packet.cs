

using System;

namespace Server;


public abstract class Packet
{
    public byte[] data;
    public Packet(byte[] data)
    {
        this.data = data;
    }
    public Packet(){
        
    }
    /// <summary>
    /// The SERVER SIDED code to run when this packet is sent from any client. Assume data is set
    /// </summary>
    /// <returns></returns> <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public abstract byte[] ServerHandle();
    
    /// <summary>
    /// The CLIENT SIDED code to run when this packet is recived. Assume data is set
    /// </summary> <summary>
    /// 
    /// </summary>
    public abstract void ClientReceive();
    /// <summary>
    /// The CLIENT SIDED code to get the contents of this package
    /// </summary>
    /// <returns></returns>
    public abstract Byte[] Send();
}