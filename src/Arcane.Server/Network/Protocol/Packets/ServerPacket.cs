namespace Arcane.Server.Network.Protocol.Packets;

internal struct ServerPacket
{
    public List<ServerRegisterPacket> Register { get; set; }
}

internal struct ServerRegisterPacket
{
    public bool Register { get; set; }
    public ushort Id { get; set; }
}
