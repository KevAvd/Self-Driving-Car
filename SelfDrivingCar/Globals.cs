using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar
{
    static class Globals
    {
        public const float RAY_LENGTH = 200;
        public const float RAY_FIELD = 100;
        public const int RAY_NBR = 6;
        public const float ROAD_WIDTH = 600;
        public const float ROAD_HEIGHT = 800;
        public const float ROAD_TIME = 4;
        public const float CAR_WIDTH = 60;
        public const float CAR_HEIGHT = 80;
        public const float MAX_SPEED_CAR = 300;
        public const float MAX_SPEED_CAR_REVERSE = -100;
        public const float MAX_SPEED_TRAFFIC = 150;
        public const float MAX_SPEED_TRAFFIC_REVERSE = 50;
        public const float TURN_RATE = 90;
        public const float FRICTION = 50;
        public const float ACCELERATION = 300;
        public const float MUTATION_AMMOUNT = 0.3f;
        public const int MUTATION_CHANCE = 1;
        public const int MAX_TRAFFIC = 12;
        public const int MAX_DEAD_TRAFFIC = 8;
        public const int AI_PER_GENERATION = 2_500;
        public static readonly IntRect AI_TEXCOORDS = new IntRect(0, 0, 12, 16);
        public static readonly IntRect AIDEAD_TEXCOORDS = new IntRect(12, 0, 12, 16);
        public static readonly IntRect TRAFFIC1_TEXCOORDS = new IntRect(24, 0, 12, 16);
        public static readonly IntRect TRAFFIC2_TEXCOORDS = new IntRect(0, 16, 12, 16);
        public static readonly IntRect TRAFFIC3_TEXCOORDS = new IntRect(12, 16, 12, 16);
        public static readonly IntRect TRAFFIC4_TEXCOORDS = new IntRect(24, 16, 12, 16);
        public static readonly IntRect ROAD_TEXCOORDS = new IntRect(0, 0, 22 * 3, 32);
        public static Sprite SPRITE_ROAD = new Sprite(new Texture("..\\..\\..\\..\\SpriteSheet_Road.png") { Repeated = true });
        public static Sprite SPRITE_CAR = new Sprite(new Texture("..\\..\\..\\..\\SpriteSheet_Cars.png"));
    }
}
