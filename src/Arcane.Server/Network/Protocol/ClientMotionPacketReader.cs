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
                Position = new Float3
                {
                    X = reader.ReadFloat(),
                    Y = reader.ReadFloat(),
                    Z = reader.ReadFloat()
                },
                Velocity = new Float3
                {
                    X = reader.ReadFloat(),
                    Y = reader.ReadFloat(),
                    Z = reader.ReadFloat()
                }
            };
        }

        return null;
    }
}