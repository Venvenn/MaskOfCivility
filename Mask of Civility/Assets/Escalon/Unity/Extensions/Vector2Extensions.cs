using UnityEngine;

namespace Escalon
{
    public static class Vector2Extensions
    {
        public static Vector2 RandomSphere(float radius = 1.0f)
        {
            Vector2 sphereVec = new Vector3(
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f));
            return sphereVec.normalized * radius;
        }

        public static Vector2 RandomCube(Vector2 halfRange)
        {
            Vector2 cubeVec = new Vector2(
                Random.Range(-halfRange.x, halfRange.x),
                Random.Range(-halfRange.y, halfRange.y));
            return cubeVec;
        }

        public static Vector3 AsVec3XY(this Vector2 vec)
        {
            return new Vector3(vec.x, vec.y, 0.0f);
        }

        public static Vector3 AsVec3XZ(this Vector2 vec)
        {
            return new Vector3(vec.x, 0.0f, vec.y);
        }

        public static Vector3 AsVec3YZ(this Vector2 vec)
        {
            return new Vector3(0.0f, vec.x, vec.y);
        }
        
        public static System.Numerics.Vector2 ToNumeric(this Vector2 vector2)
        {
            return new System.Numerics.Vector2(vector2.x, vector2.y);
        }
        
        public static Vector2 ToUnity(this System.Numerics.Vector2 vector2)
        {
            return new Vector2(vector2.X, vector2.Y);
        }
    }
}
    

