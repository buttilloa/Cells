using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cells
{
    public class Camera
    {
        protected float zoom;
        public Matrix transform;
        public Vector2 pos;
        int ViewportWidth,ViewportHeight;
        protected float rotation;

        public Camera(int Height, int Width)
        {
            zoom = 1.0f;
            rotation = 0.0f;
            pos = Vector2.Zero;
            ViewportHeight = Height;
            ViewportWidth = Width;
        }
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public void Move(Vector2 amount)
        {
            pos += amount;
        }
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        float newZoom=0;
        bool Zooming = false;
        public void slowzoom(float addition)
        {
            if (newZoom == 0)
                newZoom = Zoom + addition;
            else newZoom += addition;
            Zooming = true;
        }
        public Matrix gettransformation(GraphicsDevice graphicsDevice)
        {
            if (Zooming) { if (Zoom > newZoom) Zoom -= .01f; if (Zoom < newZoom) { Zooming = false; Zoom = newZoom; newZoom = 0; }; }
            transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(ViewportWidth * 0.5f, ViewportHeight * 0.5f, 0));
            transform.Translation.Normalize();
            return transform;
        }
    }
  }