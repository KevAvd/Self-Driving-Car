using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar
{
    static class CollisionDetection
    {
        public static bool AABB_RAY(AABB box1, Ray ray)
        {
            return AABB_RAY(box1, ray, out Vector2f pNear, out Vector2f pFar, out Vector2f normal);
        }

        /// <summary>
        /// Detect collision between AABB and a ray
        /// </summary>
        /// <param name="box1"> AABB to check </param>
        /// <param name="ray"> Ray to check </param>
        /// <param name="pNear"> Near collision point </param>
        /// <param name="pFar"> Far collision point </param>
        /// <param name="normal"> Surface normal of the near collision point </param>
        /// <returns> True if collision </returns>
        public static bool AABB_RAY(AABB box1, Ray ray, out Vector2f pNear, out Vector2f pFar, out Vector2f normal)
        {
            //Init
            pNear = new Vector2f(0, 0);               //Near collision point
            pFar = new Vector2f(0, 0);                //Far collision point
            normal = new Vector2f(0, 0);              //Surface normal of the near collision point
            float swap;                               //Used for swaping two values
            Vector2f[] aabbCoords = { box1.p1, box1.p2, box1.p3, box1.p4 }; //Contains AABB's coords
            Vector2f[] rayCoords = { ray.p1, ray.p2 };//Contains ray's coords
            Vector2f d = rayCoords[1] - rayCoords[0]; //Ray distance
            Vector2f tNear = new Vector2f((aabbCoords[0].X - rayCoords[0].X) / d.X, (aabbCoords[0].Y - rayCoords[0].Y) / d.Y);   //time to near collision
            Vector2f tFar = new Vector2f((aabbCoords[2].X - rayCoords[0].X) / d.X, (aabbCoords[2].Y - rayCoords[0].Y) / d.Y);    //time to far collision

            //Sort values
            if (tNear.X > tFar.X)
            {
                swap = tNear.X;
                tNear.X = tFar.X;
                tFar.X = swap;
            }
            if (tNear.Y > tFar.Y)
            {
                swap = tNear.Y;
                tNear.Y = tFar.Y;
                tFar.Y = swap;
            }

            //Check if there is a collision
            if (tNear.X > tFar.Y || tNear.Y > tFar.X) { return false; }
            if (Math.Min(tFar.X, tFar.Y) < 0) { return false; }
            float tHitNear = Math.Max(tNear.X, tNear.Y);
            float tHitFar = Math.Min(tFar.X, tFar.Y);
            if (tHitNear > 1) { return false; }

            //Get collision point
            pNear = rayCoords[0] + tHitNear * d;
            pFar = rayCoords[0] + tHitFar * d;

            //Find surface normal of the AABB at collision point
            if (tNear.X > tNear.Y) { normal = d.X < 0 ? new Vector2f(1, 0) : new Vector2f(-1, 0); }
            else if (tNear.X < tNear.Y) { normal = d.Y < 0 ? new Vector2f(0, 1) : new Vector2f(0, -1); }

            return true;
        }

        /// <summary>
        /// Detect collision between two AABBs
        /// </summary>
        /// <param name="box1"> AABB to check </param>
        /// <param name="box2"> AABB to check </param>
        /// <returns> True if collision </returns>
        public static bool AABB_AABB(AABB box1, AABB box2)
        {
            //Check for collision
            if (box1.p1.X > box2.p3.X)
            {
                return false;
            }
            if (box1.p3.X < box2.p1.X)
            {
                return false;
            }
            if (box1.p1.Y > box2.p3.Y)
            {
                return false;
            }
            if (box1.p3.Y < box2.p1.Y)
            {
                return false;
            }

            return true;
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
