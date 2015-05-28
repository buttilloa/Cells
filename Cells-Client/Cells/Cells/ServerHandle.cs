using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cells
{
    enum PacketTypes
    {
        PLAYER,
        MAPSIZE,
        CELLCOUNT,
        CELLS,
        REQUESTPLAYERS,
        SENDPLAYERS,
        KILL,
        ID,
        LEAVE,
        CLEARCELL

    }
    public class ServerHandle
    {
        public bool isRunning = false;
       
        static NetClient client;
        int ID;
        public ServerHandle()
        {
            NetPeerConfiguration Config = new NetPeerConfiguration("Cell");
            Config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            client = new NetClient(Config);
            Config.EnableUPnP = true;
            client.Start();
            client.UPnP.ForwardPort(50001, "Cell");
            //client.Connect("64.121.122.40", 50001);
            client.DiscoverLocalPeers(50001);
            isRunning = true;
            //ThreadPool.QueueUserWorkItem(readMessage, null);
            Thread thread = new Thread(new ThreadStart(readMessage));
            thread.Start();
        }
        public void readMessage()
        {
            while (true)
            {
                if (!isRunning) break;
                /*ThreadPool.QueueUserWorkItem(ThreadSwitch, null);
                int temp, temp2;
                ThreadPool.GetAvailableThreads(out temp, out temp2);
                temp *= temp2;
                 */
                ThreadSwitch(null);
            }
        }
        public void ThreadSwitch(object msg)
        {
           
            NetIncomingMessage msgIn = client.ReadMessage();
            if (msgIn != null)
            {
                switch (msgIn.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        handleMessage(msgIn);
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        handleMessage(msgIn);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        client.Connect(msgIn.SenderEndPoint);
                        break;

                }
                client.Recycle(msgIn);
            }

        }
        public void handleMessage(object msg2)
        {
            NetIncomingMessage msg = (NetIncomingMessage)msg2;
            switch ((PacketTypes)msg.ReadByte())
            {
                case PacketTypes.SENDPLAYERS:
                    int count = msg.ReadInt32();
                    if (Game1.map != null && Game1.map.cells.Count > 0)
                        for (int i = 0; i < count; i++)
                        {
                            int PlayerId = msg.ReadInt32();
                            int x = msg.ReadInt32();
                            int y = msg.ReadInt32();
                            float size = msg.ReadSingle();
                            int color = msg.ReadInt32();
                            if (PlayerId != ID) ThreadPool.QueueUserWorkItem(createPlayer, new Player(new Microsoft.Xna.Framework.Vector2(x, y), size, color, PlayerId));
                        } break;
                case PacketTypes.KILL:
                    Game1.instance.Exit();
                    break;
                case PacketTypes.ID:
                    ID = msg.ReadInt32();
                    break;
                case PacketTypes.MAPSIZE:
                    Game1.map.setSize(msg.ReadInt32(), msg.ReadInt32());
                    break;
                case PacketTypes.CLEARCELL:
                    Game1.map.cells.Clear();
                    break;
                case PacketTypes.CELLS:
                    int Xpos = msg.ReadInt32();
                    int Ypos = msg.ReadInt32();
                    int Newcolor = msg.ReadInt32();
                    Cell newCell = new Cell(new Microsoft.Xna.Framework.Vector2(Xpos, Ypos), 10, Newcolor);
                    Game1.map.cells.Add(newCell);
                    break;

                default: Console.WriteLine("Unknown PacketType"); break;
            }
        }
        public void createPlayer(object obj)
        {
            Player newPlayer = (Player)obj;
            bool contains = false;
            for (int i = 0; i < Game1.map.players.Count; i++)
                if (Game1.map.players[i].getID() == newPlayer.getID())
                {
                    contains = true;
                    Game1.map.players[i] = newPlayer;
                    break;
                }
            if (!contains) Game1.map.players.Add(newPlayer);
        }
        public void sendMessage(String text)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write("HELOOO");
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }
        public void sendPlayer(Player player)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write((byte)PacketTypes.PLAYER);
            msg.Write((int)Math.Round(player.Pos.X));
            msg.Write((int)Math.Round(player.Pos.Y));
            msg.Write(player.size);
            msg.Write(player.getColor());
            client.SendMessage(msg, NetDeliveryMethod.UnreliableSequenced, 8);
        }
        public void sendMap(int height, int width)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write((byte)PacketTypes.MAPSIZE);
            msg.Write(height);
            msg.Write(width);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }
        public void sendCell(List<Cell> cells)
        {

            int count = 0;
            while (count < cells.Count)
            {
                NetOutgoingMessage msg = client.CreateMessage();
                msg.Write((byte)PacketTypes.CELLS);
                if (count + 10 < cells.Count) msg.Write(10);
                else msg.Write(cells.Count - count);
                int add = 0;
                for (int i = count; i < count + 10 && i < cells.Count; i++, add++)
                {
                    msg.Write((int)cells[i].Pos.X);
                    msg.Write((int)cells[i].Pos.Y);
                    //msg.Write((int)cells[i].getColor());
                }
                client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
                count += add;
            }
        }
        public void sendPlayerRequest()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write((byte)PacketTypes.REQUESTPLAYERS);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 5);
        }
        public void disconect()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write((byte)PacketTypes.LEAVE);
            msg.Write(ID);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 9);
            if (isRunning)
                client.Disconnect("GAME EXIT");
            isRunning = false;

        }
    }
}
