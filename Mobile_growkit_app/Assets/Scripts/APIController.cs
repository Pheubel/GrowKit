using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class APIController : MonoBehaviour
{
    [SerializeField] private LoggingState _loggingState;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveUtility.TryGetAuthToken(out string token))
        {
            _loggingState = LoggingState.GotAuthKey;
            StartCoroutine(AuthenticateWithToken(token));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AuthenticateWithToken(string authToken)
    {
        var c = 1;
        c++;

        return null;
    }

    public enum LoggingState
    {
        NoAuthKey,
        GotAuthKey,
        ConnectingToApi,
        NoConnectionToApi,
        InvalidCredentials,
        Connected
    }
}
