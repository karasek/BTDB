﻿using System;

namespace BTDB.Buffer
{
    public struct ByteBuffer
    {
        byte[] _buffer;
        uint _offset;
        int _length;

        public static ByteBuffer NewAsync(byte[] buffer)
        {
            return new ByteBuffer(buffer, 0, buffer.Length);
        }

        public static ByteBuffer NewAsync(byte[] buffer, int offset, int length)
        {
            return new ByteBuffer(buffer, (uint)offset, length);
        }

        public static ByteBuffer NewSync(byte[] buffer)
        {
            return new ByteBuffer(buffer, 0x80000000u, buffer.Length);
        }

        public static ByteBuffer NewSync(byte[] buffer, int offset, int length)
        {
            return new ByteBuffer(buffer, (((uint)offset) | 0x80000000u), length);
        }

        private ByteBuffer(byte[] buffer, uint offset, int length)
        {
            _buffer = buffer;
            _offset = offset;
            _length = length;
        }

        public byte[] Buffer { get { return _buffer; } }
        public int Offset { get { return (int)(_offset & 0x7fffffffu); } }
        public int Length { get { return _length; } }
        public bool AsyncSafe { get { return (_offset & 0x80000000u) == 0u; } }

        public ByteBuffer ToAsyncSafe()
        {
            if (AsyncSafe) return this;
            var copy = new byte[_length];
            Array.Copy(_buffer, Offset, copy, 0, _length);
            return NewAsync(copy);
        }

        public void MakeAsyncSafe()
        {
            if (AsyncSafe) return;
            var copy = new byte[_length];
            Array.Copy(_buffer, Offset, copy, 0, _length);
            _buffer = copy;
            _offset = 0;
        }
    }
}