using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cells
{

    public class Map
    {
        public List<Cell> cells = new List<Cell>();
        public List<Player> players = new List<Player>();
        MouseState MS, oldState;
        //public Player player;
        Texture2D mapTexture;
        public static int Width = 100;
        public static int Height = 100;
        private int MaxCells;
        Vector2 Velocity;
        public Map()
        {
            //player = new Player(new Vector2(10, 10));
            Game1.client.sendMap(Width, Height);
            MaxCells = (((Width + Height) / 2) / 10);
            cells.Clear();
        }
        public void setSize(int width, int height)
        {
            Width = width;
            Height = height;
            //MaxCells = (((Width + Height) / 2) / 10);
            //cells.Clear();
        }
        public void update(GameTime time)
        {
           
            /*while (cells.Count < MaxCells)
            {
                cells.Add(new Cell(new Vector2(Game1.randy.Next(15, Width - 15), Game1.randy.Next(15, Height - 15))));
                if (cells.Count == MaxCells)
                {
                    // Game1.client.sendCell(cells);
                }
            }
             */
            //handleCollisions();
           // handleMouse();
            if (oldState.Equals(MS)) Game1.client.sendMouse((int)Velocity.X,(int)Velocity.Y);
        }
        public void handleMouse()
        {
            oldState = MS;
            int width = Game1.Bounds.Width / 2;
            int heigt = Game1.Bounds.Height / 2;
            MS = Mouse.GetState();
            Velocity = new Vector2((MS.X - width) / 20, (MS.Y - heigt) / 20);
            int size = players[0].getSize;
            Vector2 pos = players[0].Pos;
            if ((pos.X - size) <= 0 && Velocity.X < 0)
            { Velocity = new Vector2(0, Velocity.Y); pos = new Vector2(0 + size, pos.Y); }
            if ((pos.X + size) >= Map.Width && Velocity.X > 0)
            { Velocity = new Vector2(0, Velocity.Y); pos = new Vector2(Map.Width - size, pos.Y); }
            if ((pos.Y - size) <= 0 && Velocity.Y < 0)
            { Velocity = new Vector2(Velocity.X, 0); pos = new Vector2(pos.X, 0 + size); }
            if ((pos.Y + size) >= Map.Height && Velocity.Y > 0)
            { Velocity = new Vector2(Velocity.X, 0); pos = new Vector2(pos.X, Map.Height - size); }
            Velocity *= .2f;
            Velocity = new Vector2((int)Math.Round(Velocity.X * 1d, 0), (int)Math.Round(Velocity.Y * 1d, 0));
         
        }
      /*  public void handleCollisions()
        {
            for (int i = 0; i < cells.Count; i++)
                if (player[0].size > cells[i].size)
                    if (Vector2.Distance(player.getPostion(), cells[i].getPostion()) < player.getSize + cells[i].getSize)
                    {
                        player.SlowIncrement(cells[i].size * .01f);
                        cells.RemoveAt(i);
                    }
            for (int i = 0; i < players.Count; i++)
                if (player.size > players[i].size)
                    if (Vector2.Distance(player.getPostion(), players[i].getPostion()) < player.getSize + players[i].getSize)
                    {
                        //player.SlowIncrement(players[i].size * .01f);
                        //players.RemoveAt(i);
                    }
        }*/
        public void setTexture(Texture2D texture)
        {
            mapTexture = texture;
        }
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(mapTexture, new Rectangle(0, 0, Width, Height), Color.White);
            for (int i = 0; i < cells.Count; i++)
                cells[i].Draw(batch);
            for (int i = 0; i < players.Count; i++)
                players[i].Draw(batch);
        }
        public void DrawStatic(SpriteBatch batch)
        {

        }
    }
}
