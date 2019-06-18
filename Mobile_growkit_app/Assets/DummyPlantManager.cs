using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.SceneTransition;
using Utilities;

public class DummyPlantManager : MonoBehaviour
{
    public ulong SensorId;

    public void HandlePlantTransitionMessage(PlantTransitionMessage message)
    {
        SensorId = long.MaxValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
