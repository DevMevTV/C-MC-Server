namespace Minecraft_Server
{
    public class NBTEncoder
    {
        public static void EncodeCompound(Stream stream, object compound)
        {
            foreach (var property in compound.GetType().GetProperties())
            {
                var name = property.Name;
                var value = property.GetValue(compound);
                WriteTag(stream, name, value);
            }
            stream.WriteByte(0); // End tag
        }

        private static void WriteTag(Stream stream, string name, object value)
        {
            if (value is byte)
            {
                stream.WriteByte(1); // TAG_Byte
                WriteString(stream, name);
                stream.WriteByte((byte)value);
            }
            else if (value is short)
            {
                stream.WriteByte(2); // TAG_Short
                WriteString(stream, name);
                WriteShort(stream, (short)value);
            }
            else if (value is int)
            {
                stream.WriteByte(3); // TAG_Int
                WriteString(stream, name);
                WriteInt(stream, (int)value);
            }
            else if (value is long)
            {
                stream.WriteByte(4); // TAG_Long
                WriteString(stream, name);
                WriteLong(stream, (long)value);
            }
            else if (value is float)
            {
                stream.WriteByte(5); // TAG_Float
                WriteString(stream, name);
                WriteFloat(stream, (float)value);
            }
            else if (value is double)
            {
                stream.WriteByte(6); // TAG_Double
                WriteString(stream, name);
                WriteDouble(stream, (double)value);
            }
            else if (value is byte[])
            {
                stream.WriteByte(7); // TAG_Byte_Array
                WriteString(stream, name);
                WriteByteArray(stream, (byte[])value);
            }
            else if (value is string)
            {
                stream.WriteByte(8); // TAG_String
                WriteString(stream, name);
                WriteString(stream, (string)value);
            }
            else if (value is IEnumerable<object>)
            {
                stream.WriteByte(9); // TAG_List
                WriteString(stream, name);
                WriteList(stream, (IEnumerable<object>)value);
            }
            else if (value != null)
            {
                stream.WriteByte(10); // TAG_Compound
                WriteString(stream, name);
                EncodeCompound(stream, value);
            }
            else
            {
                throw new InvalidOperationException("Unsupported NBT type");
            }
        }

        private static void WriteString(Stream stream, string value)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(value);
            WriteShort(stream, (short)data.Length);
            stream.Write(data, 0, data.Length);
        }

        private static void WriteShort(Stream stream, short value)
        {
            stream.WriteByte((byte)((value >> 8) & 0xFF));
            stream.WriteByte((byte)(value & 0xFF));
        }

        private static void WriteInt(Stream stream, int value)
        {
            for (int i = 3; i >= 0; i--)
            {
                stream.WriteByte((byte)((value >> (8 * i)) & 0xFF));
            }
        }

        private static void WriteLong(Stream stream, long value)
        {
            for (int i = 7; i >= 0; i--)
            {
                stream.WriteByte((byte)((value >> (8 * i)) & 0xFF));
            }
        }

        private static void WriteFloat(Stream stream, float value)
        {
            WriteInt(stream, BitConverter.SingleToInt32Bits(value));
        }

        private static void WriteDouble(Stream stream, double value)
        {
            WriteLong(stream, BitConverter.DoubleToInt64Bits(value));
        }

        private static void WriteByteArray(Stream stream, byte[] value)
        {
            WriteInt(stream, value.Length);
            stream.Write(value, 0, value.Length);
        }

        private static void WriteList(Stream stream, IEnumerable<object> list)
        {
            var enumerator = list.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException("NBT lists cannot be empty");
            }

            var firstElementType = GetTagType(enumerator.Current);
            stream.WriteByte(firstElementType);

            var count = 0;
            using (var memoryStream = new MemoryStream())
            {
                do
                {
                    WriteTagValue(memoryStream, enumerator.Current);
                    count++;
                } while (enumerator.MoveNext());

                WriteInt(stream, count);
                memoryStream.WriteTo(stream);
            }
        }

        private static void WriteTagValue(Stream stream, object value)
        {
            if (value is byte)
                stream.WriteByte((byte)value);
            else if (value is short)
                WriteShort(stream, (short)value);
            else if (value is int)
                WriteInt(stream, (int)value);
            else if (value is long)
                WriteLong(stream, (long)value);
            else if (value is float)
                WriteFloat(stream, (float)value);
            else if (value is double)
                WriteDouble(stream, (double)value);
            else if (value is byte[])
                WriteByteArray(stream, (byte[])value);
            else if (value is string)
                WriteString(stream, (string)value);
            else if (value != null)
                EncodeCompound(stream, value);
            else
                throw new InvalidOperationException("Unsupported NBT list value type");
        }

        private static byte GetTagType(object value)
        {
            return value switch
            {
                byte => 1,
                short => 2,
                int => 3,
                long => 4,
                float => 5,
                double => 6,
                byte[] => 7,
                string => 8,
                IEnumerable<object> => 9,
                _ => 10,
            };
        }
    }

}