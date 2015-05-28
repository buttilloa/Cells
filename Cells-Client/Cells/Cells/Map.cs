using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public Player player;
        Texture2D mapTexture;
        public static int Width = 100;
        public static int Height = 100;
        private int MaxCells;
        public Map()
        {
            player = new Player(new Vector2(10, 10));
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
            player.update(time);
            /*while (cells.Count < MaxCells)
            {
                cells.Add(new Cell(new Vector2(Game1.randy.Next(15, Width - 15), Game1.randy.Next(15, Height - 15))));
                if (cells.Count == MaxCells)
                {
                    // Game1.client.sendCell(cells);
                }
            }
             */
            handleCollisions();
            if (player.hasMoved()) Game1.client.sendPlayer(player);
        }
        public void handleCollisions()
        {
            for (int i = 0; i < cells.Count; i++)
                if (player.size > cells[i].size)
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
        }
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
            player.Draw(batch);
        }
        public void DrawStatic(SpriteBatch batch)
        {

        }
    }
}
