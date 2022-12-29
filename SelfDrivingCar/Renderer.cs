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
        static List<Vertex> _Vertices_Quads_Background = new List<Vertex>();

        //Rendering
        static RenderStates rndr_state;
        static RenderTarget rndr_trgt;
        static Camera _camera;

        //Color
        static Color _clearColor = new Color(100,100,100);

        //Getters/Setters
        public static RenderStates States { get => rndr_state; set => rndr_state = value; }
        public static RenderTarget Target { get => rndr_trgt; set => rndr_trgt = value; }
        public static Color ClearColor { get => _clearColor; set => _clearColor = value; }
        public static Camera Camera { get => _camera; set => _camera = value; }

        public static void LOAD_CAR_VERTICES(Car[] cars)
        {
            _Vertices_Quads_Cars.Clear();
            Vector2f position;

            foreach(Car car in cars)
            {
                Vertex[] vertices = car.vertices;

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
            rndr_trgt.Clear(_clearColor);
            rndr_trgt.Draw(_Vertices_Quads_Background.ToArray(), PrimitiveType.Quads, rndr_state);
            rndr_trgt.Draw(_Vertices_Quads_Cars.ToArray(), PrimitiveType.Quads, rndr_state);
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
