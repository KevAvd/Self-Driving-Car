using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace SelfDrivingCar
{
    class Car
    {
        //Properties
        float width = 30;
        float height = 40;
        CarType type;
        bool dead = false;

        //Controls
        bool forwards = false;
        bool backwards = false;
        bool left = false;
        bool right = false;

        //Physic properties
        float rotation = 0;
        float speed = 0;
        Vector2f position;
        AABB aabb;
        Ray ray;
        const float MAX_SPEED = 200;
        const float MAX_SPEED_REVERSE = -50;
        const float TURN_RATE = 90;
        const float FRICTION = 50;
        const float ACCELERATION = 100;

        //Graphic properties
        public Vertex[] vertices = new Vertex[4];

        public float Width { get => width; }
        public float Height { get => height; }
        public CarType Type { get => type; }
        public bool Dead { get => dead; }
        public bool Forwards { get => forwards; set => forwards = value; }
        public bool Backwards { get => backwards; set => backwards = value; }
        public bool Left { get => left; set => left = value; }
        public bool Right { get => right; set => right = value; }
        public float Rotation { get => rotation; }
        public float Speed { get => speed; }
        public Vector2f Position { get => position; }
        public AABB Aabb { get => aabb; }
        public Ray Ray { get => ray; }

        //Enum
        public enum CarType
        {
            AI, Traffic1, Traffic2, Traffic3, Traffic4
        }

        public Car(Vector2f position, CarType type)
        {
            this.position = position;
            this.type = type;
            switch (type)
            {
                case CarType.AI:
                    vertices[0] = new Vertex(new Vector2f(-1,-1), new Color(255, 255, 255, 255), new Vector2f(00, 00));
                    vertices[1] = new Vertex(new Vector2f( 1,-1), new Color(255, 255, 255, 255), new Vector2f(12, 00));
                    vertices[2] = new Vertex(new Vector2f( 1, 1), new Color(255, 255, 255, 255), new Vector2f(12, 16));
                    vertices[3] = new Vertex(new Vector2f(-1, 1), new Color(255, 255, 255, 255), new Vector2f(00, 16));
                    break;
                case CarType.Traffic1:
                    vertices[0] = new Vertex(new Vector2f(-1,-1), new Color(255, 255, 255, 255), new Vector2f(24, 00));
                    vertices[1] = new Vertex(new Vector2f( 1,-1), new Color(255, 255, 255, 255), new Vector2f(36, 00));
                    vertices[2] = new Vertex(new Vector2f( 1, 1), new Color(255, 255, 255, 255), new Vector2f(36, 16));
                    vertices[3] = new Vertex(new Vector2f(-1, 1), new Color(255, 255, 255, 255), new Vector2f(24, 16));
                    break;
                case CarType.Traffic2:
                    vertices[0] = new Vertex(new Vector2f(-1,-1), new Color(255, 255, 255, 255), new Vector2f(00, 16));
                    vertices[1] = new Vertex(new Vector2f( 1,-1), new Color(255, 255, 255, 255), new Vector2f(12, 16));
                    vertices[2] = new Vertex(new Vector2f( 1, 1), new Color(255, 255, 255, 255), new Vector2f(12, 32));
                    vertices[3] = new Vertex(new Vector2f(-1, 1), new Color(255, 255, 255, 255), new Vector2f(00, 32));
                    break;
                case CarType.Traffic3:
                    vertices[0] = new Vertex(new Vector2f(-1,-1), new Color(255, 255, 255, 255), new Vector2f(12, 16));
                    vertices[1] = new Vertex(new Vector2f( 1,-1), new Color(255, 255, 255, 255), new Vector2f(24, 16));
                    vertices[2] = new Vertex(new Vector2f( 1, 1), new Color(255, 255, 255, 255), new Vector2f(24, 32));
                    vertices[3] = new Vertex(new Vector2f(-1, 1), new Color(255, 255, 255, 255), new Vector2f(12, 32));
                    break;
                case CarType.Traffic4:
                    vertices[0] = new Vertex(new Vector2f(-1,-1), new Color(255, 255, 255, 255), new Vector2f(24, 16));
                    vertices[1] = new Vertex(new Vector2f( 1,-1), new Color(255, 255, 255, 255), new Vector2f(36, 16));
                    vertices[2] = new Vertex(new Vector2f( 1, 1), new Color(255, 255, 255, 255), new Vector2f(36, 32));
                    vertices[3] = new Vertex(new Vector2f(-1, 1), new Color(255, 255, 255, 255), new Vector2f(24, 32));
                    break;
            }
        }

        public void Update()
        {
            if(dead) return;

            switch (type)
            {
                case CarType.AI: TYPE_AI(); break;
                case CarType.Traffic1:
                case CarType.Traffic2:
                case CarType.Traffic3:
                case CarType.Traffic4: TYPE_TRAFFIC(); break;
            }
        }

        void TYPE_AI()
        {
            if (forwards)
            {
                speed += ACCELERATION * GameTime.DeltaTimeU;
                if (speed > MAX_SPEED) { speed = MAX_SPEED; }
            }
            if (backwards)
            {
                speed -= ACCELERATION * GameTime.DeltaTimeU;
                if (speed < MAX_SPEED_REVERSE) { speed = MAX_SPEED_REVERSE; }
            }
            if (left)
            {
                rotation -= GameMath.ToRadian(TURN_RATE) * GameTime.DeltaTimeU;
            }
            if (right)
            {
                rotation += GameMath.ToRadian(TURN_RATE) * GameTime.DeltaTimeU;
            }

            //Apply velocity to position
            Vector2f velocity = GameMath.GetUnitVectorFromAngle(rotation - GameMath.ToRadian(90)) * speed;
            position += velocity * GameTime.DeltaTimeU;

            if (speed > 0)
            {
                speed -= FRICTION * GameTime.DeltaTimeU;
            }
            if (speed < 0)
            {
                speed += FRICTION * GameTime.DeltaTimeU;
            }
        }

        void TYPE_TRAFFIC()
        {

        }
    }

    struct AABB
    {
        public Vector2f p1;
        public Vector2f p2;
        public Vector2f p3;
        public Vector2f p4;
    }

    struct Ray
    {
        public Vector2f p1;
        public Vector2f p2;
    }
}
