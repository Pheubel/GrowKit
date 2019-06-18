using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Utilities.SceneTransition;

public class TransitionSender : MonoBehaviour
{
    [SerializeField] SceneReference scene;

    public void SendTransitionRequest()
    {
        GameMessenger.Instance.SendMessageOfType(new TransitionMessage (scene.BuildIndex));
    }
}
