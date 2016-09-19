using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cells
{
    public class Player : Cell
    {
        int width, heigt;
        String name = "YO";
        Boolean manualDraw = false;
        float oldSize;
        long ID;
        public Player(Vector2 position)
        {
            Pos = position;
            color = Game1.randy.Next(Colors.Length);
            size = 15;
            Game1.camera.Zoom = 7;
        }
        public Player(Vector2 position, float size, int color, long ID)
        {
            Pos = position;
            this.color = color;
            this.size = size;
            this.ID = ID;
        }
        public long getID()
        {
            return ID;
        }
        public void SlowIncrement(float addition)
        {
            oldSize = size;
            size += addition;
            manualDraw = true;
            Game1.camera.slowzoom(-addition * .05f);
        }
        public void update(GameTime time)
        {
            //if (manualDraw)
            //{ if (oldSize < size) oldSize++; if (Math.Abs(oldSize - size) < 2)manualDraw = false; }
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (manualDraw)
                batch.Draw(Game1.circle, new Rectangle((int)Pos.X - (int)(oldSize * sizeMultiplier), (int)Pos.Y - (int)(oldSize * sizeMultiplier), 2 * (int)(oldSize * sizeMultiplier), 2 * (int)(oldSize * sizeMultiplier)), new Rectangle(0, 0, 932, 932), Colors[color]);
            else base.Draw(batch);
        }
    }
}
