namespace Arcane.Server.Network.Protocol.Packets;

internal struct ClientInputPacket
{
    public bool Forward { get; init; }
    public bool Backward { get; init; }
    public bool Left { get; init; }
    public bool Right { get; init; }
}