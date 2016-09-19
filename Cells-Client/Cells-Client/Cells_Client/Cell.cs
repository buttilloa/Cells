using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cells
{

    public class Cell
    {
        public readonly Color[] Colors = { Color.Blue, Color.Red, Color.Pink, Color.Green, Color.Yellow, Color.Purple, Color.Orange, Color.Brown, Color.Cyan };
        public Vector2 Pos;
        protected int color;
        public float size = 10;
        protected float sizeMultiplier = .50f;
        public int getColor()
        {
            //return 0;
            return color;
        }
        public Cell()
        {
            Pos = new Vector2(Game1.randy.Next(0, Map.Width), Game1.randy.Next(0, Map.Height));
            color = Game1.randy.Next(Colors.Length);
         }
        public Cell(Vector2 Position)
        {
            Pos = Position;
            color = Game1.randy.Next(Colors.Length);
        }
        public Cell(Vector2 Position, float size, int color)
        {
            Pos = Position;
            this.size = size;
            this.color = color;
        }
        public int getSize
        {
            get { return (int)(size * sizeMultiplier); }
        }
        public Vector2 getPostion()
        {
            return Pos;
        }
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Game1.circle, new Rectangle((int)Pos.X - getSize, (int)Pos.Y - getSize, 2 * getSize, 2 * getSize), new Rectangle(0, 0, 932, 932), Colors[color]);
        }
    }
}
