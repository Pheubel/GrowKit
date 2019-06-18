using System;
using UnityEngine.Events;

namespace Utilities.SceneTransition
{
    public struct PlantTransitionMessage
    {
        public long SensorId;
        public int SceneIndex;

        public PlantTransitionMessage(long sensorId, int sceneIndex)
        {
            SensorId = sensorId;
            SceneIndex = sceneIndex;
        }
    }

    [Serializable]
    public class PlantTransitionEvent : UnityEvent<PlantTransitionMessage> { }
}
