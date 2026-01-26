using UnityEngine;
 
namespace Escalon
{
    public static class Vector3Extensions
    {
        public static Vector3 MulElem(this Vector3 vec, Vector3 other)
        {
            return new Vector3(
                vec.x * other.x,
                vec.y * other.y,
                vec.z * other.z
                );
        }

        public static Vector3 Lerp(this Vector3 vec, Vector3 other, Vector3 lerp)
        {
            return new Vector3(
                vec.x * lerp.x + other.x * (1.0f - lerp.x),
                vec.y * lerp.y + other.y * (1.0f - lerp.y),
                vec.z * lerp.z + other.z * (1.0f - lerp.z)
                );
        }

        public static Vector3 Clamp(this Vector3 instance, float min, float max)
        {
            Vector3 clamped = new Vector3(
                Mathf.Clamp(instance.x, min, max),
                Mathf.Clamp(instance.y, min, max),
                Mathf.Clamp(instance.z, min, max)
                );
            return clamped;
        }

        public static Vector3Int Clamp(this Vector3Int instance, int min, int max)
        {
            Vector3Int clamped = new Vector3Int(
                Mathf.Clamp(instance.x, min, max),
                Mathf.Clamp(instance.y, min, max),
                Mathf.Clamp(instance.z, min, max)
                );
            return clamped;
        }

        public static Vector3 MulScalar(this Vector3 instance, float s)
        {
            return new Vector3(
                instance.x * s,
                instance.y * s,
                instance.z * s
                );
        }
        public static Vector3 Div(this Vector3 instance, Vector3 other)
        {
            return new Vector3(
                instance.x / other.x,
                instance.y / other.y,
                instance.z / other.z
                );
        }

        public static Vector3 Sub(this Vector3 instance, float other)
        {
            return new Vector3(
                instance.x - other,
                instance.y - other,
                instance.z - other
                );
        }

        public static Vector3 Min(Vector3 a, Vector3 b)
        {
            return new Vector3(
                Mathf.Min(a.x, b.x),
                Mathf.Min(a.y, b.y),
                Mathf.Min(a.z, b.z)
                );
        }
        public static Vector3 Max(Vector3 a, Vector3 b)
        {
            return new Vector3(
                Mathf.Max(a.x, b.x),
                Mathf.Max(a.y, b.y),
                Mathf.Max(a.z, b.z)
                );
        }

        public static Vector3 RandomSphere(float radius = 1.0f)
        {
            Vector3 sphereVec = new Vector3(
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f));
            return sphereVec.normalized * radius;
        }
        public static Vector3 RandomUnitCube()
        {
            Vector3 cubeVec = new Vector3(
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f));
            return cubeVec;
        }
        public static Vector3 RandomCube(Vector3 halfRange)
        {
            Vector3 cubeVec = new Vector3(
                Random.Range(-halfRange.x, halfRange.x),
                Random.Range(-halfRange.y, halfRange.y),
                Random.Range(-halfRange.z, halfRange.z));
            return cubeVec;
        }

        public static float MinElement(this Vector3 a)
        {
            return Mathf.Min(a.x, a.y, a.z);
        }

        public static float MaxElement(this Vector3 a)
        {
            return Mathf.Max(a.x, a.y, a.z);
        }

        public static Vector3 SmoothStep(Vector3 from, Vector3 to, float t)
        {
            return new Vector3(
                Mathf.SmoothStep(from.x, to.x, t),
                Mathf.SmoothStep(from.y, to.y, t),
                Mathf.SmoothStep(from.z, to.z, t)
                );
        }

        public static Vector2 xz(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.z);
        }

        public static Vector2 xy(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static System.Numerics.Vector3 ToNumeric(this Vector3 vector3)
        {
            return new System.Numerics.Vector3(vector3.x, vector3.y, vector3.z);
        }
        
        public static Vector3 ToUnity(this System.Numerics.Vector3 vector3)
        {
            return new Vector3(vector3.X, vector3.Y, vector3.Z);
        }
    }
}
