using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace SelfDrivingCar
{
    internal class Road
    {
        AABB aabb;
        const float width = 600;
        const float height = 800;
        Vector2f position = new Vector2f(0,0);
        Sprite sprite;
        Texture textureRoad = new Texture("..\\..\\..\\..\\SpriteSheet_Road.png") { Repeated = true };
        public Vector2f Position { get => position; set => position = value; }
        public float Width { get => width; }
        public float Height { get => height; }

        public Road(Vector2f pos)
        {
            sprite = new Sprite(textureRoad);
            sprite.TextureRect = new IntRect(0, 0, 22*3, 32);
            sprite.Scale = new Vector2f(width / (22*3), height / 32);
            position = pos;
        }

        public void Update()
        {
            sprite.Position = position;
        }

        public void Draw(RenderTarget trgt)
        {
            trgt.Draw(sprite);
        }
    }
}
