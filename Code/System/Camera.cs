
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnamedGame.Abstract;
using UnamedGame.Helpers;

namespace UnamedGame.GameSystem
{

    public enum CameraMode
    {
        Free,
        FollowEntity,

    }
    public class Camera
    {
        public Vector2 Position;
        public bool ZoomLocked = false;
        private float zoom = 0.5f;
        public float Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = MathHelper.Clamp(value, 0.1f, 2.0f);
            }
        }
        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                    Matrix.CreateScale(Zoom);
            }
        }
        public int width;
        public int height;
        public Camera(Vector2 position, int width, int height, float zoom = 1.0f)
        {
            Position = position;

            this.Zoom = zoom;
            this.width = width;
            this.height = height;

        }
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return worldPosition - Position;
        }
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return screenPosition / zoom + Position;
        }
        public CameraMode Mode = CameraMode.Free;
        public Entity followEntity;
        public Vector2 DragStart = Vector2.Zero;
        private int lastScrollValue = 0;

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && Mode == CameraMode.Free)
            {
                if (DragStart == Vector2.Zero)
                {
                    DragStart = mouseState.Position.ToVector2();
                }
                else
                {
                    Position += DragStart - mouseState.Position.ToVector2();
                    DragStart = mouseState.Position.ToVector2();
                }
            }
            else
            {
                DragStart = Vector2.Zero;
            }
            if (!ZoomLocked)
            {
                int scrollValue = mouseState.ScrollWheelValue;
                if (scrollValue > lastScrollValue && Zoom < 2.0f)
                {
                    Zoom += 0.1f;
                    Position += new Vector2(width / 2 * 0.1f, height / 2 * 0.1f);
                }
                else if (scrollValue < lastScrollValue && Zoom > 0.1f)
                {
                    Zoom -= 0.1f;
                    Position -= new Vector2(width / 2 * 0.1f, height / 2 * 0.1f);
                }
                lastScrollValue = scrollValue;
            }
            if (Mode == CameraMode.FollowEntity)
            {
                centerOnEntity(this.followEntity);
            }
            KeyboardState state = Keyboard.GetState();

            if (state.keyPressed(Keys.Y))
            {
                if (Mode == CameraMode.Free)
                {
                    Mode = CameraMode.FollowEntity;
                    followEntity = UnamedGame.Instance.player;
                }
                else
                {
                    Mode = CameraMode.Free;
                }
            }

        }

        public void centerOnEntity(Entity e)
        {
            Position = e.Position - new Vector2(width  / 2 * (1/zoom) , height  / 2 * (1/zoom));
        }
    }
}