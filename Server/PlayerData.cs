using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class PlayerData
    {
        public Vector2 Pos;
        Vector2 Velocity;
        public int color;
        public float size = 10;
        public long ID;
        public PlayerData(Vector2 pos, float size, int color, long id)
        {
            this.Pos = pos;
            this.color = color;
            this.size = size;
            this.ID = id;
        }
        public void updatePos()
        {
            Pos += Velocity;
        }
        public void setVelocity(Vector2 NewVelocity)
        {
            Velocity = NewVelocity;
        }
        public static PlayerData RandomPlayer(long ID)
        {
            return new PlayerData(new Vector2(10, 10), 15, new Random().Next(9), ID);
        }
    }
}
