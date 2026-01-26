using System;

namespace Escalon
{
    [Serializable]
    public struct EventViewTimeData : IData
    {
        public float VFXSpeed;
        public float PopUpSpeed;
        public float AnimationSpeed;
        public float CameraStateChangeSpeed;
        public float TargettingPauseDuration;
        public SerializableDictionary<string, float> EventViewDelays;
    }
}
