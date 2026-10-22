using Arcane.Server.Core;
using Arcane.Server.Network.Protocol.Packets;

namespace Arcane.Server.Network.Protocol;

internal ref struct ClientMotionPacketReader
{
    public static ClientMotionPacket? Read(ref BitReader reader)
    {
        if (reader.ReadBit())
        {
            return new ClientMotionPacket
            {
                Position = reader.ReadFloat3(),
                Velocity = reader.ReadFloat3()
            };
        }

        return null;
    }
}