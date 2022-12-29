using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace SelfDrivingCar
{
    static class Renderer
    {
        //Vertex
        static List<Vertex> _Vertices_Quads_Cars = new List<Vertex>();
        static List<Vertex> _Vertices_Quads_Roads = new List<Vertex>();

        //Rendering
        static RenderStates rndr_state1;
        static RenderStates rndr_state2;
        static RenderTarget rndr_trgt;
        static Camera _camera;

        //Color
        static Color _clearColor = new Color(100,100,100);

        //Getters/Setters
        public static RenderStates States1 { get => rndr_state1; set => rndr_state1 = value; }
        public static RenderStates States2 { get => rndr_state2; set => rndr_state2 = value; }
        public static RenderTarget Target { get => rndr_trgt; set => rndr_trgt = value; }
        public static Color ClearColor { get => _clearColor; set => _clearColor = value; }
        public static Camera Camera { get => _camera; set => _camera = value; }

        public static void LOAD_ROAD_VERTICES(Road[] roads)
        {
            _Vertices_Quads_Roads.Clear();
            Vector2f position;

            foreach (Road road in roads)
            {
                Vertex[] vertices = road.Vertices;

                if (vertices.Length != 4)
                {
                    Console.WriteLine("[RENDERER][LOAD_CAR_VERTICES] Wrong size");
                    return;
                }

                for (int i = 0; i < vertices.Length; i++)
                {
                    position = WorldSpaceToViewSpace(ObjectSpaceToWorldSpace(vertices[i].Position, road.Position, new Vector2f(road.Width / 2, road.Height / 2), road.Rotation));
                    _Vertices_Quads_Roads.Add(new Vertex(position, vertices[i].Color, vertices[i].TexCoords));
                }
            }
        }

        public static void LOAD_CAR_VERTICES(Car[] cars)
        {
            _Vertices_Quads_Cars.Clear();
            Vector2f position;

            foreach(Car car in cars)
            {
                Vertex[] vertices = car.Vertices;

                if(vertices.Length != 4)
                {
                    Console.WriteLine("[RENDERER][LOAD_CAR_VERTICES] Wrong size");
                    return;
                }

                for(int i = 0; i < vertices.Length; i++)
                {
                    position = WorldSpaceToViewSpace(ObjectSpaceToWorldSpace(vertices[i].Position, car.Position, new Vector2f(car.Width / 2, car.Height / 2), car.Rotation));
                    _Vertices_Quads_Cars.Add(new Vertex(position, vertices[i].Color, vertices[i].TexCoords));
                }
            }
        }

        public static void Draw()
        {
            rndr_trgt.Draw(_Vertices_Quads_Roads.ToArray(), PrimitiveType.Quads, rndr_state2);
            rndr_trgt.Draw(_Vertices_Quads_Cars.ToArray(), PrimitiveType.Quads, rndr_state1);
        }

        static Vector2f ObjectSpaceToWorldSpace(Vector2f objPos, Vector2f worldPos, Vector2f scalar, float rotation)
        {
            return GameMath.VectorRotation(GameMath.ScaleVector(objPos, scalar), rotation) + worldPos;
        }

        static Vector2f WorldSpaceToViewSpace(Vector2f worldPos)
        {
            Vector2f scalar = GameMath.ScaleVector((Vector2f)rndr_trgt.Size, _camera.Size, true);
            worldPos = GameMath.ScaleVector(worldPos, scalar);
            return worldPos + (GameMath.ScaleVector((Vector2f)rndr_trgt.Size, new Vector2f(2, 2), true) - _camera.Center);
        }

        //TODO
        //static Vector2f ViewSpaceToScreenSpace(Vector2f vec)
        //{
        //    return new Vector2f();
        //}
    }
}
