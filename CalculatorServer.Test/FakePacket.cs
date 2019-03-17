using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorServer.Test
{
    using System;
    using System.IO;

    namespace ServerCalculator.Test
    {
        public class FakePacket
        {
            private MemoryStream stream;
            private BinaryWriter writer;

            public FakePacket()
            {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }

            public FakePacket(byte command, params object[] elements) : this()
            {
                writer.Write(command);
                foreach (object element in elements)
                {
                    if (element is float)
                        writer.Write((float)element);
                    else
                        throw new Exception("unknown type");
                }
            }

            public byte[] GetData()
            {
                return stream.ToArray();
            }
        }
    }
}
