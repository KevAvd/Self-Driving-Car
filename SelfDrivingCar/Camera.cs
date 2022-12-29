using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar
{
    internal class Camera
    {
        Vector2f center;
        Vector2f size;
        float speed = 200;
        bool up;
        bool down;
        bool left;
        bool right;

        public Vector2f Center { get => center; set => center = value; }
        public Vector2f Size { get => size; set => size = value; }
        public float Speed { get => speed; set => speed = value; }
        public bool Move_Up { get => up; set => up = value; }
        public bool Move_Down { get => down; set => down = value; }
        public bool Move_Left { get => left; set => left = value; }
        public bool Move_Right { get => right; set => right = value; }

        public Camera(Vector2f center, Vector2f size)
        {
            this.center = center;
            this.size = size;
        }

        public void UpdateCam()
        {
            if (up)
            {
                center.Y -= speed * GameTime.DeltaTimeU;
            }
            if (down)
            {
                center.Y += speed * GameTime.DeltaTimeU;
            }
            if (left)
            {
                center.X -= speed * GameTime.DeltaTimeU;
            }
            if (right)
            {
                center.X += speed * GameTime.DeltaTimeU;
            }
        }

        public void Zoom(float value, bool zoomOut = false)
        {
            size = zoomOut ? size * 2 : size / 2;
        }
    }
}
