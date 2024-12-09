using System.Text;

namespace Minecraft_Server
{
    internal static class Datatypes
    {
        public static class VarInt
        {
            public static int Decode(byte[] data, ref int offset)
            {
                int value = 0;
                int position = 0;

                while (true)
                {
                    byte currentByte = data[offset++];
                    value |= (currentByte & 0x7F) << position;

                    if ((currentByte & 0x80) == 0) break;

                    position += 7;

                    if (position >= 32) throw new InvalidOperationException("VarInt is too big");
                }

                return value;
            }

            public static int Decode(byte[] data, int offset)
            {
                int tempOffset = offset;
                return Decode(data, ref tempOffset);
            }

            public static void Encode(ref byte[] buffer, int value, bool prepend = false)
            {
                List<byte> output = [];

                while (true)
                {
                    if ((value & ~0x7F) == 0)
                    {
                        output.Add((byte)value);
                        break;
                    }

                    output.Add((byte)((value & 0x7F) | 0x80));
                    value >>= 7;
                }
                if (prepend)
                {
                    buffer = output.ToArray().Concat(buffer).ToArray();
                } else
                {
                    buffer = buffer.Concat(output.ToArray()).ToArray();
                }
            }

            public static byte[] Encode(int value, bool prepend = false)
            {
                byte[] result = new byte[0];
                Encode(ref result, value, prepend);
                return result;
            }
        }

        

        public static class String
        {
            public static void Encode(ref byte[] buffer, string value)
            {
                var utf8Bytes = Encoding.UTF8.GetBytes(value);
                byte[] lengthVarInt = VarInt.Encode(utf8Bytes.Length);
                
                byte[] result = new byte[lengthVarInt.Length + utf8Bytes.Length];
                Buffer.BlockCopy(lengthVarInt, 0, result, 0, lengthVarInt.Length);
                Buffer.BlockCopy(utf8Bytes, 0, result, lengthVarInt.Length, utf8Bytes.Length);
                
                buffer = buffer.Concat(result).ToArray();
            }

            public static string Decode(byte[] buffer, ref int offset)
            {
                var length = VarInt.Decode(buffer, ref offset);
                if (offset + length > buffer.Length)
                    throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer too small to contain the encoded string.");

                var value = Encoding.UTF8.GetString(buffer, offset, length);

                offset += length;
                return value;
            }

            public static (string Value, int Offset) Decode(byte[] buffer, int offset)
            {
                return (Decode(buffer, ref offset), offset);
            }
        }



        public static class Int
        {

        }



        public static class UnsignedShort
        {
            public static void Encode(ushort value, byte[] buffer, ref int offset)
            {
                buffer[offset] = (byte)(value >> 8);
                buffer[offset + 1] = (byte)(value);
                offset += 2;
            }

            public static ushort Decode(byte[] buffer, ref int offset)
            {
                ushort value = (ushort)(buffer[offset] << 8 | buffer[offset + 1]);
                offset += 2;
                return value;
            }
        }



        public static class UUID
        {
            public static void Encode(ref byte[] buffer, string uuid)
            {

                byte[] byteArray = uuid
                    .Split('-')
                    .SelectMany(part => Enumerable.Range(0, part.Length / 2)
                        .Select(i => Convert.ToByte(part.Substring(i * 2, 2), 16)))
                    .ToArray();

                buffer = buffer.Concat(byteArray).ToArray();

            }

            public static string Decode(byte[] buffer, ref int offset)
            {
                byte[] subArray = new byte[16];

                Array.Copy(buffer, offset, subArray, 0, 16);

                offset += 16;

                return BitConverter.ToString(subArray)
                    .Replace("-", "")
                    .Insert(8, "-")
                    .Insert(17, "-")
                    .Insert(26, "-");
            }
        }
    }
}
