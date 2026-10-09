namespace Arcane.Server.Core;

internal ref struct BitReader(ReadOnlySpan<byte> buffer)
{
    private readonly ReadOnlySpan<byte> _buffer = buffer;
    private int _offset;

    public bool ReadBit()
    {
        var byteIndex = _offset / 8;
        var bitIndex = 7 - _offset % 8;
        _offset++;

        return (_buffer[byteIndex] & (1 << bitIndex)) != 0;
    }

    public ushort ReadByte()
    {
        ushort value = 0;
        for (var i = 0; i < 8; i++)
        {
            value = (ushort)((value << 1) | (ReadBit() ? 1 : 0));
        }

        return value;
    }

    public ushort ReadUnsignedShort()
    {
        ushort value = 0;
        for (var i = 0; i < 16; i++)
        {
            value = (ushort)((value << 1) | (ReadBit() ? 1 : 0));
        }

        return value;
    }

    public void Skip(int bits)
    {
        _offset += bits;
    }
}