using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class GameData
    {
        public static int Width = 1000, Height = 1000;
        List<Cell> cells = new List<Cell>();
        Random randy;
        int colorCount = 9;
        public GameData()
        {
            randy = new Random(System.Environment.TickCount);
        }
        public void setMapSize(int width, int height)
        {
            Width = width;
            Height = height;
            generateCells();
        }
        public void generateCells()
        {
            int MaxCells = (((Width + Height) / 2) / 10);
            cells.Clear();
            while (cells.Count < MaxCells)
            {
                cells.Add(new Cell(randy.Next(15, Width - 15), randy.Next(15, Height - 15), randy.Next(colorCount)));
            }
            sendCells();
            ServerHandle.SetText("Cells successfully calculated: " + cells.Count + "\n", 0);
        }
        public void sendCells()
        {
            Lidgren.Network.NetOutgoingMessage msgOut = ServerHandle.server.CreateMessage();
            msgOut.Write((byte)PacketTypes.CLEARCELL);
            ServerHandle.server.SendToAll(msgOut, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
            for (int i = 0; i < cells.Count; i++)
            {
                Lidgren.Network.NetOutgoingMessage msgOut2 = ServerHandle.server.CreateMessage();
                msgOut2.Write((byte)PacketTypes.CELLS);
                msgOut2.Write(cells[i].x);
                msgOut2.Write(cells[i].y);
                msgOut2.Write(cells[i].color);
                msgOut2.Write(i);
                ServerHandle.server.SendToAll(msgOut2, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
            }
        }
        public void spawnCell(int x, int y) //adds to list and sends to clients
        {
            Cell cell = new Cell(x, y, randy.Next(colorCount));
            cells.Add(cell);
            Lidgren.Network.NetOutgoingMessage msgOut2 = ServerHandle.server.CreateMessage();
            msgOut2.Write((byte)PacketTypes.CELLS);
            msgOut2.Write(cell.x);
            msgOut2.Write(cell.y);
            msgOut2.Write(cell.color);
            ServerHandle.server.SendToAll(msgOut2, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);

        }
        public void addCell(Cell cell)
        {
            cells.Add(cell);
        }
        public void handleRecievedCells(Lidgren.Network.NetIncomingMessage msg)
        {
            Thread thread = new Thread(() => ThreadCellParse(msg));
            thread.Start();
        }
        public void ThreadCellParse(Lidgren.Network.NetIncomingMessage msg)
        {
            Console.WriteLine("Bit " + msg.LengthBits + "Byte " + msg.LengthBytes);
            if (msg.LengthBits > 10)
            {
                Console.WriteLine("THREADING");
                int count = msg.ReadInt32();
                Console.WriteLine("count: " + count);
                for (int i = 0; i < count; i++)
                {
                    int x = msg.ReadInt32();
                    int y = msg.ReadInt32();
                    int color = 0;//msg.ReadInt32();
                    cells.Add(new Cell(x, y, color));
                    Console.WriteLine("X: {0} Y: {1} Color: {2} I: {3}", x, y, color, i);

                }
                ServerHandle.SetText("[SERVER]Succesfully created all " + count + " cells\n", 0);
            }
        }
        
    }
    class Cell
    {

        public int x;
        public int y;
        public int color;
        public Cell(int x, int y, int color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
        public string returnPos()
        {
            return "X: " + x + " Y: " + y + " ";
        }
    }
}
