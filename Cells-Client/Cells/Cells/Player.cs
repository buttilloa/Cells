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
        Vector2 Velocity , oldPos;
        int width, heigt;
        String name = "YO";
        Boolean manualDraw = false;
        int id;
        float oldSize;
        public Player(Vector2 position)
        {
            Pos = position;
            color = Game1.randy.Next(Colors.Length);
            size = 15;
            Game1.camera.Zoom = 7;
        }
        public Player(Vector2 position , float size, int color , int id)
        {
            Pos = position;
            this.color =color;
            this.size = size;
            this.id = id;
        }
        public int getID()
        {
            return id;
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
            oldPos = Pos;
            width = Game1.Bounds.Width / 2;
            heigt = Game1.Bounds.Height / 2;
            MouseState ms = Mouse.GetState();
            Velocity = new Vector2((ms.X - width) / 20, (ms.Y - heigt) / 20);

            if ((Pos.X - getSize) <= 0 && Velocity.X < 0)
            { Velocity = new Vector2(0, Velocity.Y); Pos = new Vector2(0 + getSize, Pos.Y); }
            if ((Pos.X + getSize) >= Map.Width && Velocity.X > 0)
            { Velocity = new Vector2(0, Velocity.Y); Pos = new Vector2(Map.Width - getSize, Pos.Y); }
            if ((Pos.Y - getSize) <= 0 && Velocity.Y < 0)
            { Velocity = new Vector2(Velocity.X, 0); Pos = new Vector2(Pos.X, 0 + getSize); }
            if ((Pos.Y + getSize) >= Map.Height && Velocity.Y > 0)
            { Velocity = new Vector2(Velocity.X, 0); Pos = new Vector2(Pos.X, Map.Height - getSize); }
            Velocity *= .2f;
            Velocity = new Vector2((int)Math.Round(Velocity.X * 1d, 0), (int)Math.Round(Velocity.Y * 1d, 0));
            Pos += Velocity;
            if (manualDraw)
            { if (oldSize < size) oldSize++; if (Math.Abs(oldSize - size) < 2)manualDraw = false; }


        }
        public bool hasMoved()
        {
            return !oldPos.Equals(Pos);
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            if (manualDraw)
                batch.Draw(Game1.circle, new Rectangle((int)Pos.X - (int)(oldSize * sizeMultiplier), (int)Pos.Y - (int)(oldSize * sizeMultiplier), 2 * (int)(oldSize * sizeMultiplier), 2 * (int)(oldSize * sizeMultiplier)), new Rectangle(0, 0, 932, 932), Colors[color]);
            else base.Draw(batch);
        }
    }
}
