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
        float width = 55*3*2;
        float height = 90*3*2;
        float rotation = 0;
        Vector2f position = new Vector2f(0,0);
        Vertex[] vertices;

        public Vertex[] Vertices { get => vertices; set => vertices = value; }
        public Vector2f Position { get => position; set => position = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public float Width { get => width; set => width = value; }
        public float Height { get => height; set => height = value; }

        public Road()
        {
            vertices = new Vertex[4];
            vertices[0] = new Vertex(new Vector2f(-1,-1), new Color(255, 255, 255, 255), new Vector2f(00, 00));
            vertices[1] = new Vertex(new Vector2f( 1,-1), new Color(255, 255, 255, 255), new Vector2f(22*3, 00));
            vertices[2] = new Vertex(new Vector2f( 1, 1), new Color(255, 255, 255, 255), new Vector2f(22*3, 32));
            vertices[3] = new Vertex(new Vector2f(-1, 1), new Color(255, 255, 255, 255), new Vector2f(00, 32));
        }
    }
}
