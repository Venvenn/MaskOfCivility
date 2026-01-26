using UnityEngine;

namespace Escalon
{
    public static class QuaternionExtensions
    {
        public static System.Numerics.Quaternion ToNumeric(this Quaternion quaternion)
        {
            return new System.Numerics.Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }
        
        public static Quaternion ToUnity(this System.Numerics.Quaternion quaternion)
        {
            return new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }
    }
}
    