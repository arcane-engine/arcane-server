using Arcane.Server.Core;

namespace Arcane.Server.Network.Protocol;

internal struct Packet
{
    public ushort Size { get; init; }
    public InputPacket? Input { get; init; }
}

internal ref struct PacketReader(ref BitReader reader)
{
    private BitReader _reader = reader;

    public Packet Read()
    {
        return new Packet
        {
            Size = _reader.ReadUShort(),
            Input = new InputPacketReader(ref _reader).Read()
        };
    }
}