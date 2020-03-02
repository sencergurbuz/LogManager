using System;
using System.IO;
using System.Threading;

namespace LogManager
{
    public class cLogItem
    {
        public long Id { get; private set; }

        static private int nextId;

        public DateTime dateTime { get; private set; }

        public LogLevel enLogLevel { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public cLogItem()
        {
            Id = Interlocked.Increment(ref nextId);
            dateTime = DateTime.Now;
        }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(this.enLogLevel.ToString());
                    writer.Write(this.Summary);
                    writer.Write(this.Description);

                }
                return m.ToArray();
            }
        }

        public static cLogItem Desserialize(byte[] data)
        {
            cLogItem result = new cLogItem();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    result.enLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), reader.ReadString());
                    result.Summary = reader.ReadString();
                    result.Description = reader.ReadString();
                }
            }

            return result;
        }
    }
}