using Arcane.Server.Core;
using Arcane.Server.Network.Protocol.Packets;

namespace Arcane.Server.Network.Protocol;

internal ref struct ClientInputPacketReader(ref BitReader reader)
{
    private BitReader _reader = reader;

    public ClientInputPacket? Read()
    {
        if (_reader.ReadBit())
        {
            return new ClientInputPacket
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