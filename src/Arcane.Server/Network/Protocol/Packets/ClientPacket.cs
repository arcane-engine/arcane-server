namespace Arcane.Server.Network.Protocol.Packets;

internal struct ClientPacket
{
    public ClientInputPacket? Input { get; init; }
    public ClientMotionPacket? Motion { get; init; }
}