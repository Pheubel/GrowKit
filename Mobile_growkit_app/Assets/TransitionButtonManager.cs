using UnityEngine;
using Utilities;
using Utilities.SceneTransition;

public class TransitionButtonManager : MonoBehaviour
{
    public string PlantName;

    [SerializeField] SceneReference _linkedScene;
    [SerializeField] long _sensorId;

    public void OnButtonTapped()
    {
        GameMessenger.Instance.SendMessageOfType(new PlantTransitionMessage(_sensorId,_linkedScene.BuildIndex));
    }
}
