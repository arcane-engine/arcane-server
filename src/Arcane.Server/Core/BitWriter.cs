namespace Arcane.Server.Core;

internal ref struct BitWriter(Span<byte> buffer)
{
    private readonly Span<byte> _buffer = buffer;
    private int _offset;

    public int Size => (_offset + 7) / 8;

    public void WriteBit(bool value)
    {
        var byteIndex = _offset / 8;
        var bitIndex = 7 - _offset % 8;

        if (value)
        {
            _buffer[byteIndex] |= (byte)(1 << bitIndex);
        }

        _offset++;
    }

    public void WriteByte(byte value)
    {
        for (var i = 7; i >= 0; i--)
        {
            WriteBit((value & (1 << i)) != 0);
        }
    }

    public void WriteUnsignedShort(ushort value)
    {
        for (var i = 15; i >= 0; i--)
        {
            WriteBit((value & (1 << i)) != 0);
        }
    }

    public void Skip(int bits)
    {
        _offset += bits;
    }
}