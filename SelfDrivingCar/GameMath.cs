using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace SelfDrivingCar
{
    static class GameMath
    {
        static Random _rnd = new Random();

        public static Random Rnd { get => _rnd; set => _rnd = value; }

        /// <summary>
        /// Return vector length
        /// </summary>
        /// <param name="vec"> vector </param>
        /// <returns> Length of vector </returns>
        public static float GetVectorLength(Vector2f vec)
        {
            return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
        }

        /// <summary>
        /// Get a random point in a circle
        /// </summary>
        /// <param name="pos"> Circle position </param>
        /// <param name="r"> Circle radius </param>
        /// <param name="surface"> If true, the methode will return a point at the surface of the circle </param>
        /// <returns> Vector2f position inside the circle </returns>
        public static Vector2f RandomPointInCircle(Vector2f pos, float r, bool surface = false)
        {
            float angle;
            float hyp = r;
            float dx;
            float dy;

            angle = (float)_rnd.NextDouble() * ((float)Math.PI * 2);
            if (!surface) { hyp = (float)_rnd.NextDouble() * r; }
            dx = (float)Math.Cos(angle) * hyp;
            dy = (float)Math.Sin(angle) * hyp;

            return pos + new Vector2f(dx, dy);
        }

        /// <summary>
        /// Get unit vector of an angle
        /// </summary>
        /// <param name="angle"> Angle of the unit vector </param>
        /// <returns> Unit vector </returns>
        public static Vector2f GetUnitVectorFromAngle(float angle)
        {
            return new Vector2f((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        /// <summary>
        /// Add only one axis of a vector to another
        /// </summary>
        /// <param name="vec"> Vector </param>
        /// <param name="toAdd"> Vector to add </param>
        /// <param name="XAxis"> Add only X axis if true </param>
        /// <returns> Vector sum </returns>
        public static Vector2f AddOnlyOneAxis(Vector2f vec, Vector2f toAdd, bool XAxis)
        {
            if (XAxis)
            {
                return new Vector2f(vec.X + toAdd.X, vec.Y);
            }

            return new Vector2f(vec.X, vec.Y + toAdd.Y);
        }

        /// <summary>
        /// Keep an angle in degree between 0 and 360
        /// </summary>
        /// <returns> Angle in degree </returns>
        public static float Stay360(float angle)
        {
            if (angle > 360)
            {
                angle -= 360;
            }
            else if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }

        /// <summary>
        /// Keep an angle in radian between 0 and 2 PI
        /// </summary>
        /// <returns> Angle in radian </returns>
        public static float Stay2PI(float angle)
        {
            if (angle > Math.PI * 2)
            {
                angle -= (float)Math.PI * 2;
            }
            else if (angle < 0)
            {
                angle += (float)Math.PI * 2;
            }

            return angle;
        }

        /// <summary>
        /// Convert degree to radian
        /// </summary>
        /// <param name="degree"> Degree to convert </param>
        /// <returns> Radian </returns>
        public static float ToRadian(float degree)
        {
            return degree * (float)Math.PI / 180.0f;
        }

        /// <summary>
        /// Convert radian to degree 
        /// </summary>
        /// <param name="radian"> Radian to convert </param>
        /// <returns> Degree </returns>
        public static float ToDegree(float radian)
        {
            return radian * 180.0f / (float)Math.PI;
        }

        /// <summary>
        /// Scale a vector
        /// </summary>
        /// <param name="toScale"> Vector to scale </param>
        /// <param name="scalar"> Scalar </param>
        /// <returns> Scaled vector </returns>
        public static Vector2f ScaleVector(Vector2f toScale, Vector2f scalar, bool divide = false)
        {
            if (divide) { return new Vector2f(toScale.X / scalar.X, toScale.Y / scalar.Y); }
            else { return new Vector2f(toScale.X * scalar.X, toScale.Y * scalar.Y); }
        }

        public static Vector2f VectorRotation(Vector2f vec, float angle)
        {
            return new Vector2f(vec.X * MathF.Cos(angle) - vec.Y * MathF.Sin(angle), vec.X * MathF.Sin(angle) + vec.Y * MathF.Cos(angle));
        }

        public static Vector2f NormalizeVector(Vector2f vec)
        {
            if (vec.X == 0 && vec.Y == 0)
            {
                return vec;
            }

            float d = MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
            return new Vector2f(vec.X / d, vec.Y / d);
        }

        public static float Dot(Vector2f vec1, Vector2f vec2)
        {
            return vec1.X * vec2.X + vec1.Y * vec2.Y;
        }

        public static Vector2f ProjectVector(Vector2f toProject, Vector2f vec)
        {
            return Dot(toProject, vec) / (vec.X * vec.X + vec.Y * vec.Y) * vec;
        }
    }
}