using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Preset = GrowKitApp.StructureModels.PresetDTO;
using SensorStick = GrowKitApp.StructureModels.StickUpdateDTO;

/// <summary> Manages the information about the plant.</summary>
public class PlantManager : MonoBehaviour
{
    /// <summary> The id of the stick to fetch.</summary>
    [SerializeField] private long _stickId;
    /// <summary> The id of the preset to use for measures</summary>
    [SerializeField] private int _presetId;
    // the text fields that will be updated.
    [SerializeField] private Text _waterText;
    [SerializeField] private Text _lightText;
    [SerializeField] private Text _temperatureText;

    /// <summary> contains the preset values fetched from the API.</summary>
    private Preset _preset;
    private SensorStick _stick;

    private void Start()
    {
        StartCoroutine(RequestPresetData());
        StartCoroutine(RequestStickData());
    }

    /// <summary> Performs a call to the growkit api to fetch the preset.</summary>
    private IEnumerator RequestPresetData()
    {
        bool succesfullyRequested = false;

        do
        {
            UnityWebRequest request = UnityWebRequest.Get(GrowKitAPI.StickEndpoint(_stickId));
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                _preset = JsonUtility.FromJson<Preset>(request.downloadHandler.text);

                succesfullyRequested = true;
            }
        } while (!succesfullyRequested);
    }

    private IEnumerator RequestStickData()
    {
        do
        {
            UnityWebRequest request = UnityWebRequest.Get(GrowKitAPI.StickEndpoint(_stickId));
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                yield return new WaitForSecondsRealtime(30);
                continue;
            }

            _stick = JsonUtility.FromJson<SensorStick>(request.downloadHandler.text);

            _waterText.text = $"{(_stick.Moisture < _preset.Moisture[0] ? "too little water" : _stick.Moisture < _preset.Moisture[1] ? "Ok" : "too much water")} ({_stick.Moisture})";
            _lightText.text = $"{(_stick.Light < _preset.Light[0] ? "too little light" : _stick.Light < _preset.Light[1] ? "Ok" : "too much light")} ({_stick.Light})";
            _temperatureText.text = $"{(_stick.Temperature < _preset.Temperature[0] ? "temperature too low" : _stick.Temperature < _preset.Temperature[1] ? "Ok" : "temperature too high")} ({_stick.Temperature})";

            yield return new WaitForSecondsRealtime(10);
        } while (true);
    }
}
