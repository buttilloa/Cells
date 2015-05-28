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
        public int color;
        public float size = 10;
        public string ID;
        public PlayerData(Vector2 pos, float size, int color, string id)
        {
            this.Pos = pos;
            this.color = color;
            this.size = size;
            this.ID = id;
        }
    }
}
