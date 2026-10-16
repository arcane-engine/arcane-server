using Arcane.Server.Core;
using Arcane.Server.Network.Protocol.Packets;

namespace Arcane.Server.Network.Protocol;

internal ref struct ClientInputPacketReader
{
    public static ClientInputPacket? Read(ref BitReader reader)
    {
        if (reader.ReadBit())
        {
            return new ClientInputPacket
            {
                Forward = reader.ReadBit(),
                Backward = reader.ReadBit(),
                Left = reader.ReadBit(),
                Right = reader.ReadBit()
            };
        }

        return null;
    }
}