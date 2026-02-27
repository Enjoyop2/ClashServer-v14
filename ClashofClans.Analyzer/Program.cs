using ClashofClans.Utilities.Netty;
using DotNetty.Buffers;
using System;

namespace ClashofClans.Analyzer
{
    public class Program
    {
        public static void Main()
        {
            string hex = "0000008b955f0a04000000030000025b0000000100000000000002150000000500000021000000141dcd650600000021000000121dcd650700000021000000161dcd650800000021000000131dcd650900000021000000151dcd650a00000039000002150000000500000021000000141dcd650600000021000000121dcd650700000021000000161dcd650800000021000000131dcd650900000021000000151dcd650a0000004b";
            hex = hex.Replace(" ", string.Empty);

            IByteBuffer buffer = Unpooled.Buffer();
            //buffer.WriteHex(hex);

            buffer.SetReaderIndex(0);

            //DecodeHeader(buffer);

            for (int i = 0; i < 42; i++)
                Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            //DecodeLogicClientHome(buffer);
            //DecodeLogicClientAvatar(buffer);
            //Console.WriteLine(buffer.ReadCompressedString(false));

            buffer.DiscardReadBytes();
            Console.WriteLine(BitConverter.ToString(buffer.ReadBytes(buffer.ReadableBytes).Array).Replace("-", string.Empty));

            Console.Read();
        }

        public static void DecodeHeader(ByteBuffer buffer)
        {
            Console.WriteLine("--HEADER--");
            Console.WriteLine($"ID:      {buffer.ReadShort()}");
            Console.WriteLine($"Length:  {buffer.ReadMedium()}");
            Console.WriteLine($"Version: {buffer.ReadShort()}");
            Console.WriteLine("--HEADER END--");
        }

        public static void DecodeLogicClientAvatar(ByteBuffer buffer)
        {
            Console.WriteLine(buffer.ReadLong());
            Console.WriteLine(buffer.ReadLong());

            Console.WriteLine(buffer.ReadByte()); // HasAlliance

            Console.WriteLine("--ALLIANCE--");
            Console.WriteLine(buffer.ReadLong());
            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadInt()); // Badge
            Console.WriteLine(buffer.ReadInt()); // Members
            Console.WriteLine(buffer.ReadInt()); // Members

            Console.WriteLine(buffer.ReadByte());
            Console.WriteLine(buffer.ReadLong());
            Console.WriteLine("--ALLIANCE END--");

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            Console.WriteLine(buffer.ReadString());
            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadInt()); // Exp Level
            Console.WriteLine(buffer.ReadInt()); // Exp Points

            Console.WriteLine(buffer.ReadInt()); // Diamonds
            Console.WriteLine(buffer.ReadInt()); // Diamonds

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadInt()); // Trophies
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadInt()); // Clan Castle Gold
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadByte());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadByte());

            int recourceCapCount = buffer.ReadInt();
            for (int i = 0; i < recourceCapCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            int resourceCount = buffer.ReadInt();
            for (int i = 0; i < resourceCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            int troopCount = buffer.ReadInt();
            for (int i = 0; i < troopCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            int spellCount = buffer.ReadInt();
            for (int i = 0; i < spellCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            int troopLevelCount = buffer.ReadInt();
            for (int i = 0; i < troopLevelCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            int spellLevelCount = buffer.ReadInt();
            for (int i = 0; i < spellLevelCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            int heroLevelCount = buffer.ReadInt();
            for (int i = 0; i < heroLevelCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            int heroHealthCount = buffer.ReadInt();
            for (int i = 0; i < heroHealthCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            int heroStateCount = buffer.ReadInt();
            for (int i = 0; i < heroStateCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            Console.WriteLine($"ClanUnits: {buffer.ReadInt()}");

            int unknownCount = buffer.ReadInt();
            for (int i = 0; i < unknownCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            Console.WriteLine(buffer.ReadInt());

            int missionCount = buffer.ReadInt();
            for (int i = 0; i < missionCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
            }

            int achivementCount = buffer.ReadInt();
            for (int i = 0; i < achivementCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            int completedAchivementCount = buffer.ReadInt();
            for (int i = 0; i < completedAchivementCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            Console.WriteLine(buffer.ReadInt());

            int npcCount = buffer.ReadInt();
            for (int i = 0; i < npcCount; i++)
            {
                Console.WriteLine(buffer.ReadInt());
                Console.WriteLine(buffer.ReadInt());
            }

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");

            for (int i = 0; i < 97; i++)
            {
                Console.WriteLine($"packet.WriteInt({buffer.ReadInt()});");
            }

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadByte());
        }

        public static void DecodeLogicClientHome(ByteBuffer buffer)
        {
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadLong());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt()); // 14400

            Console.WriteLine(buffer.ReadCompressedString());
            Console.WriteLine(buffer.ReadCompressedString());
            Console.WriteLine(buffer.ReadCompressedString());
        }

        public static void DecodeLoginOk(ByteBuffer buffer)
        {
            Console.WriteLine(buffer.ReadLong());
            Console.WriteLine(buffer.ReadLong());
            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadString());
            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadString());
            Console.WriteLine(buffer.ReadString());
            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadString());
            Console.WriteLine(buffer.ReadString());
            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadInt());

            Console.WriteLine(buffer.ReadString());
            Console.WriteLine(buffer.ReadString());
            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadInt());
            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadByte());
            Console.WriteLine(buffer.ReadByte());

            Console.WriteLine(buffer.ReadString());

            Console.WriteLine(buffer.ReadInt());
        }
    }
}
