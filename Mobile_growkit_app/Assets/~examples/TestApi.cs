using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestApi : MonoBehaviour
{
    public string result;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Task.Run(async () =>result = await GrowKitAPI.GetTest());
    }
}
