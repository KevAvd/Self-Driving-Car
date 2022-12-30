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
        const float width = 60;
        const float height = 80;
        CarType type;
        bool dead = false;
        bool old_dead = false;

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
        const float MAX_SPEED_TRAFFIC = 100;
        const float MAX_SPEED_REVERSE = -50;
        const float TURN_RATE = 90;
        const float FRICTION = 50;
        const float ACCELERATION = 100;

        //Graphic properties
        Sprite sprite = new Sprite(new Texture("..\\..\\..\\..\\SpriteSheet_Cars.png"));
        IntRect AI_Coords = new IntRect(0, 0, 12, 16);
        IntRect AI_Dead_Coords = new IntRect(12, 0, 12, 16);
        IntRect traffic1_Coords = new IntRect(24, 0, 12, 16);
        IntRect traffic2_Coords = new IntRect(0, 16, 12, 16);
        IntRect traffic3_Coords = new IntRect(12, 16, 12, 16);
        IntRect traffic4_Coords = new IntRect(24, 16, 12, 16);

        public float Width { get => width; }
        public float Height { get => height; }
        public CarType Type { get => type; }
        public bool Dead { get => dead; set => dead = value; }
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
                    sprite.TextureRect = AI_Coords;
                    break;
                case CarType.Traffic1:
                    sprite.TextureRect = traffic1_Coords;
                    break;
                case CarType.Traffic2:
                    sprite.TextureRect = traffic2_Coords;
                    break;
                case CarType.Traffic3:
                    sprite.TextureRect = traffic3_Coords;
                    break;
                case CarType.Traffic4:
                    sprite.TextureRect = traffic4_Coords;
                    break;
            }

            sprite.Origin = new Vector2f(12 / 2, 16 / 2);
            sprite.Scale = new Vector2f(width/12, height/16);
        }

        public void Update()
        {
            //Handle die event
            if(!old_dead && dead) 
            {
                sprite.TextureRect = AI_Dead_Coords;
                sprite.Origin = new Vector2f(12 / 2, 16 / 2);
                sprite.Scale = new Vector2f(width / 12, height / 16);
                sprite.Position = position;
                sprite.Rotation = rotation;
                return; 
            }

            //Don't update car if dead
            if (dead) return;

            //Update Axis-Align Bounding Box
            aabb.p1 = position + new Vector2f(-width/2, -height/2);
            aabb.p2 = position + new Vector2f(width/2, -height/2);
            aabb.p3 = position + new Vector2f(width/2, height/2);
            aabb.p4 = position + new Vector2f(-width/2, height/2);

            //Switch car behavior
            switch (type)
            {
                case CarType.AI: TYPE_AI(); break;
                case CarType.Traffic1:
                case CarType.Traffic2:
                case CarType.Traffic3:
                case CarType.Traffic4: TYPE_TRAFFIC(); break;
            }

            //Update sprite
            sprite.Position = position;
            sprite.Rotation = rotation;

            //Update old dead state
            old_dead = dead;
        }

        public void Draw(RenderTarget trgt)
        {
            trgt.Draw(sprite);
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
                rotation -= TURN_RATE * GameTime.DeltaTimeU;
            }
            if (right)
            {
                rotation += TURN_RATE * GameTime.DeltaTimeU;
            }

            //Apply velocity to position
            Vector2f velocity = GameMath.GetUnitVectorFromAngle(GameMath.ToRadian(rotation) - GameMath.ToRadian(90)) * speed;
            position += velocity * GameTime.DeltaTimeU;

            //Apply friction
            if (speed > 0) speed -= FRICTION * GameTime.DeltaTimeU;
            if (speed < 0) speed += FRICTION * GameTime.DeltaTimeU;
        }

        void TYPE_TRAFFIC()
        {
            Vector2f velocity = GameMath.GetUnitVectorFromAngle(GameMath.ToRadian(rotation) - GameMath.ToRadian(90)) * MAX_SPEED_TRAFFIC;
            position += velocity * GameTime.DeltaTimeU;
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
