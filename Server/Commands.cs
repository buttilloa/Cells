using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Commands
    {
        public static ServerHandle Server;
        System.Windows.Forms.RichTextBox textBox;
        List<Command> cmds = new List<Command>();
        public Commands(ServerHandle server, System.Windows.Forms.RichTextBox box)
        {
            Server = server;
            this.textBox = box;
            addCommands();
        }
        public void addCommands()
        {
            cmds.Add(new Command("set", new Command.ExecuteCommand(Command.Set)));
            cmds.Add(new Command("kill", new Command.ExecuteCommand(Command.Kill)));
            cmds.Add(new Command("clear", new Command.ExecuteCommand(Command.Clear)));
            cmds.Add(new Command("spawn", new Command.ExecuteCommand(Command.Spawn)));
            cmds.Add(new Command("send", new Command.ExecuteCommand(Command.Send)));
   
        }
        public void ParseCmd(string input)
        {
            String[] args = input.ToLower().Split(' ');
            bool found = false;
            for (int i = 0; i < cmds.Count; i++)
                if (cmds[i].getName() == args[0])
                {
                    {
                      //  try
                      //  {
                            cmds[i].Execute(args);
                       // }
                       // catch (Exception e)
                      /*  {
                            if (e is System.IndexOutOfRangeException)
                                Command.Log("Error invalid arguments");
                        }
                        */found = true;
                    }
                }
            if (!found) ServerHandle.SetText("unknown cmd: '" + input + "'\n", 0);
        }
    }
    class Command
    {
        public delegate void ExecuteCommand(String[] args);
        String Name;
        ExecuteCommand Action;
        public Command(String name, ExecuteCommand execute)
        {
            Name = name;
            Action = execute;

        }
        public void Execute(String[] args)
        {
            Action(args);
        }
        public String getName()
        {
            return Name;
        }
        public static void Log(string text)
        {
            ServerHandle.SetText(text, 0);
        }

        public static void Set(String[] args)
        {
            switch (args[1])
            {
                case "map":
                    Commands.Server.data.setMapSize(Convert.ToInt32(args[2]), Convert.ToInt32(args[3]));
                    Log("Set Map size to " + args[2] + ", " + args[3] + "\n");
                    NetOutgoingMessage msg = ServerHandle.server.CreateMessage();
                    msg.Write((byte)PacketTypes.MAPSIZE);
                    msg.Write(GameData.Width);
                    msg.Write(GameData.Height);
                    ServerHandle.server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);

                    break;
            }
        }
        public static void Kill(String[] args)
        {
            NetOutgoingMessage msg = ServerHandle.server.CreateMessage();
            msg.Write((byte)PacketTypes.KILL);
            ServerHandle.server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
            Log("Killed all Clients\n");
        }
        public static void Stop(String[] args)
        {

        }
        public static void Clear(String[] args)
        {
            ServerHandle.log = "";
            Log("");
        }
        public static void Spawn(String[] args)
        {
            switch (args[1])
            {
                case "cell":
                    Commands.Server.data.spawnCell(Convert.ToInt32(args[2]),Convert.ToInt32(args[3]));
                    Log("Spawned a cell at: "+args[2]+","+args[3]+"\n");
                    break;

            }

        }
        public static void Send(String[] args)
        {
            switch (args[1])
            {
                case "cells":
                    Commands.Server.data.sendCells();
                    Log("Sent Cells to clients \n");
                    break;
            }
        }

    }
}
