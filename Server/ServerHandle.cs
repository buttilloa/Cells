using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{  enum PacketTypes
    {
        PLAYER,
        MAPSIZE,
        CELLCOUNT,
        CELLS,
        PLAYERDATA,
        SENDPLAYERS,
        KILL,
        ID,
        LEAVE,
        CLEARCELL,
        MOUSE

    }
    class ServerHandle
    {
        #region vars
        public static NetServer server;
        static ServerForm parent;
        static RichTextBox box;
        public static String log = "";
        List<PlayerData> players = new List<PlayerData>();
        List<Client> regClients = new List<Client>();
        public GameData data = new GameData();
        public bool isRunning = false;
        #endregion
        public ServerHandle()
        {

        }
        public String getIP()
        {
            return server.UPnP.GetExternalIP().ToString() + ":" + server.Configuration.Port;
        }
        public void initiate(ServerForm form, RichTextBox text)
        {
            parent = form;
            box = text;
            SetText("Staring Server...\n", 0);
            NetPeerConfiguration config = new NetPeerConfiguration("Cell");
            config.Port = 50001;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableUPnP = true;
            server = new NetServer(config);
            server.Start();
            server.UPnP.ForwardPort(50001, "Cell");
            isRunning = true;
            new Thread(new ThreadStart(listen)).Start();
            new Thread(new ThreadStart(SendPlayerData)).Start();
            SetText("Started...\n", 0);
            data.generateCells();
            new Thread(new ThreadStart(UpdateLoop)).Start();
        }
        
        public void listen()
        {
            SetText("Waiting for request...\n", 0);
            while (true)
            {
                if (!isRunning) break;
                NetIncomingMessage msgIn;
                while ((msgIn = server.ReadMessage()) != null)
                {
                    switch (msgIn.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            HandleData(msgIn);
                            break;
                        case NetIncomingMessageType.DiscoveryRequest:
                            NetOutgoingMessage msg = server.CreateMessage();
                            server.SendDiscoveryResponse(msg, msgIn.SenderEndPoint);
                            SetText("[" + msgIn.SenderEndPoint + "]Attemping Connection\n", 0);
                            break;
                        case NetIncomingMessageType.ConnectionApproval:
                            msgIn.SenderConnection.Approve();
                            Console.WriteLine(msgIn.SenderConnection.RemoteUniqueIdentifier);
                            SetText("[" + msgIn.SenderEndPoint + "]Connected\n", 0);
                            regClients.Add(new Client(msgIn.SenderEndPoint.Address.ToString(), msgIn.SenderConnection, PlayerData.RandomPlayer(msgIn.SenderConnection.RemoteUniqueIdentifier)));
                            parent.addConnection((int)regClients.Last().getID(), regClients.Last().getIP());
                            NetOutgoingMessage msgout = server.CreateMessage();
                            msgout.Write((byte)PacketTypes.ID);
                            msgout.Write(regClients.Last().getID());
                            regClients.Last().SendMessage(msgout);
                            break;
                    }
                    server.Recycle(msgIn);
                }
            }
        }
        public void HandleData(object msg2)
        {
            if (msg2 != null)
            {
                NetIncomingMessage msg = (NetIncomingMessage)msg2;
                String user = msg.SenderEndPoint.Address.ToString();
                int clietNum = Client.getByID(msg.SenderConnection.RemoteUniqueIdentifier, regClients);
                if (clietNum > -1)
                    user = "C:" + clietNum;
                switch ((PacketTypes)msg.ReadByte())
                {
                    #region PLAYER
                    case PacketTypes.PLAYER:
                        PlayerData newPlayer = new PlayerData(new Vector2(msg.ReadInt32(), msg.ReadInt32()), msg.ReadSingle(), msg.ReadInt32(), msg.SenderConnection.RemoteUniqueIdentifier);
                        //ThreadPool.QueueUserWorkItem(handlePlayerData, newPlayer);
                        //new Thread(() => handlePlayerData(newPlayer)).Start();
                        break;
                    #endregion
                    #region MOUSE
                    case PacketTypes.MOUSE:
                        //Vector2 velocity = new Vector2(msg.ReadInt32(), msg.ReadInt32());
                       ThreadPool.QueueUserWorkItem(handleMouseData, msg);
                        //new Thread(() => h(newPlayer)).Start();
                        break;
                    #endregion
                    #region MAPSIZE
                    case PacketTypes.MAPSIZE:
                        if (GameData.Width == 0 || GameData.Height == 0)
                        {
                            data.setMapSize(msg.ReadInt32(), msg.ReadInt32());
                            SetText("[" + user + "]Set Map size to " + GameData.Width + "," + GameData.Height + "\n", 0);
                        }
                        else
                        {
                            NetOutgoingMessage msgout = server.CreateMessage();
                            msgout.Write((byte)PacketTypes.MAPSIZE);
                            msgout.Write(GameData.Width);
                            msgout.Write(GameData.Height);
                            server.SendMessage(msgout, regClients[clietNum].getConnection(), NetDeliveryMethod.ReliableOrdered);
                            SetText("[" + user + "]Requested Map data \n", 0);
                        } break;
                    #endregion
                    #region CELLCOUNT
                    case PacketTypes.CELLCOUNT:

                        break;
                    #endregion
                    #region CELLS
                    case PacketTypes.CELLS:
                        data.handleRecievedCells(msg);
                        SetText("[" + user + "]Parsing cells\n", 0);
                        break;
                    #endregion
                    #region LEAVE
                    case PacketTypes.LEAVE:
                        int ID = msg.ReadInt32();
                        parent.removeConnection(ID);
                        SetText(ID + " Has left the game\n", 0);
                        break;
                    #endregion
                }
            }
        }
        public void handleMouseData(object obj)
        {
            if (obj is NetIncomingMessage)
            {
                NetIncomingMessage MsgIN = (NetIncomingMessage)obj;
               int pos= Client.getByID(MsgIN.SenderConnection.RemoteUniqueIdentifier, regClients);
               players[pos].setVelocity(new Vector2(MsgIN.ReadInt32(), MsgIN.ReadInt32()));
            }
        }
        /*  public void handlePlayerData(object obj)
        {
            if (obj is PlayerData)
            {
                PlayerData newPlayer = (PlayerData)obj;
                long user = newPlayer.ID;
                if (Convert.ToInt32(user.Substring(2)) > players.Count - 1)
                {
                    players.Add(newPlayer);
                    SetText("[" + user + "]NP " + newPlayer.Pos + "\n", 0);
                }
                else
                {
                    PlayerData regPlayer = players[Convert.ToInt32(user.Substring(2))];
                    if (regPlayer.Pos.X != newPlayer.Pos.X || regPlayer.Pos.Y != newPlayer.Pos.Y)
                    {
                        players[Convert.ToInt32(user.Substring(2))] = newPlayer;
                    }
                }
            }
        }*/
        public void SendPlayerData()
        {
            NetOutgoingMessage msgOut = server.CreateMessage();
            while (true)
            {
                if (!isRunning) break;
                msgOut = server.CreateMessage();
                msgOut.Write((byte)PacketTypes.SENDPLAYERS);
                msgOut.Write(players.Count);
                for (int i = 0; i < players.Count && regClients[i].isconnected; i++)
                {
                    msgOut.Write(regClients[i].getID());
                    msgOut.Write((int)players[i].Pos.X);
                    msgOut.Write((int)players[i].Pos.Y);
                    msgOut.Write(players[i].size);
                    msgOut.Write((int)players[i].color);
                }
                server.SendToAll(msgOut, NetDeliveryMethod.UnreliableSequenced);
            }
        }

        public void UpdateLoop()
        {
            while (true)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].updatePos();
                }
            }
        }
        
        delegate void SetTextCallback(string text, int call);
        public static void SetText(string text, int call)
        {
            if (call == 0)
            {
                DateTime now = DateTime.Now;
                log += "(" + now.Hour+":"+now.Minute+":"+now.Second.ToString().PadLeft(2,'0') + ") " + text;
            }
            if (box != null)
                if (box.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    parent.Invoke(d, new object[] { log, 1 });
                }
                else
                {
                    box.Text = log;

                }
        }
        public void shutdown()
        {
            if (isRunning)
            {
                NetOutgoingMessage msg = server.CreateMessage();
                msg.Write((byte)PacketTypes.KILL);
                server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
                isRunning = false;
                server.Shutdown("SHUTTING DOWN");
                log = "";
            }

        }
    }
}
