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
        int ID, port;  
        NetConnection conection;
        public bool isconnected = true;
        public Client(String ip, int ID, NetConnection connect)
        {
            this.IPAddress = ip;
            this.ID = ID;
            this.conection = connect;
            port =connect.RemoteEndPoint.Port;
        }
        public NetConnection getConnection()
        {
            return conection;
        }
        public string getIP()
        {
            return IPAddress;
        }
        public int getID()
        {
            return ID;
        }
        public static int getByIP(string Address, List<Client> clients)
        {
            for (int i = 0; i < clients.Count; i++)
                if (clients[i].getIP() == Address)
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
