using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        string IPAddress;
        int port;
        public long ID;
        NetConnection conection;
        PlayerData player;
        public bool isconnected = true;
        public Client(String ip, NetConnection connect , PlayerData player)
        {
            this.IPAddress = ip;
            this.ID = connect.RemoteUniqueIdentifier;
            this.conection = connect;
            port =connect.RemoteEndPoint.Port;
            this.player = player;
        }
        public NetConnection getConnection()
        {
            return conection;
        }
        public string getIP()
        {
            return IPAddress;
        }
        public long getID()
        {
            return ID;
        }
        public static int getByID(long ID, List<Client> clients)
        {
            for (int i = 0; i < clients.Count; i++)
                if (clients[i].ID == ID)
                    return i;
            return -1;
        }
        public void SendMessage(NetOutgoingMessage msgout)
        {
            IPEndPoint receiver = new IPEndPoint(NetUtility.Resolve(IPAddress), port);
            ServerHandle.server.SendUnconnectedMessage(msgout, receiver);
          
        }
    }
}
