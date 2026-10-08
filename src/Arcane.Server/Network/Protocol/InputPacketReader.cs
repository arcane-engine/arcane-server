using Arcane.Server.Core;

namespace Arcane.Server.Network.Protocol;

internal struct InputPacket
{
    public bool Forward { get; init; }
    public bool Backward { get; init; }
    public bool Left { get; init; }
    public bool Right { get; init; }
}

internal ref struct InputPacketReader(ref BitReader reader)
{
    private BitReader _reader = reader;

    public InputPacket? Read()
    {
        if (_reader.ReadBit())
        {
            return new InputPacket
            {
                Forward = _reader.ReadBit(),
                Backward = _reader.ReadBit(),
                Left = _reader.ReadBit(),
                Right = _reader.ReadBit()
            };
        }

        return null;
    }
}