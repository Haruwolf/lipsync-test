using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class RecordManager : MonoBehaviour
{
    [SerializeField] private float recordTime = 10;
    [SerializeField] private TMP_Dropdown micSettings;
    private AudioClip recordedClip;
    private int micIndex = 0;
    private float recordTimeElapsed;
    private bool isRecording = false;
    private const int second = 1;
    private const int audioRate = 44100;

    public AudioClip RecordedClip
    {
        get => recordedClip;
        private set => recordedClip = value;
    }

    public UnityEvent recorded;

    public void StartRecording()
    {
        recordTimeElapsed = recordTime;
        isRecording = true;
        string microphone = Microphone.devices[micIndex];
        RecordedClip = Microphone.Start(microphone, false, Mathf.FloorToInt(recordTime), audioRate);
        Invoke(nameof(StopRecording), recordTime);
        InvokeRepeating(nameof(ReduceSeconds), second, second);
    }

    private void ReduceSeconds()
    {
        recordTimeElapsed -= second;
    }

    public void StopRecording()
    {
        if (!string.IsNullOrEmpty(Microphone.devices[micIndex]))
        {
            isRecording = false;
            CancelInvoke(nameof(StopRecording));
            CancelInvoke(nameof(ReduceSeconds));
            Microphone.End(Microphone.devices[micIndex]);
            recorded.Invoke();
        };
    }

    private void OnGUI()
    {
        if (!isRecording)
        {
            return;
        }

        float width = 200f;
        float height = 40f;
        float x = (Screen.width - width) / 2;
        float y = (Screen.height - height) / 4;
        Rect guiArea = new Rect(x, y, width, height);
        GUI.Box(guiArea, $"Seconds remaining: {recordTimeElapsed} seconds" );
    }

    private void Start()
    {
        micSettings.ClearOptions();
        List<string> micData = new List<string>();
        foreach (var m in Microphone.devices)
        {
            micData.Add(m);
        }
        micSettings.AddOptions(micData);
        micSettings.onValueChanged.AddListener(delegate { ChangeMic(micSettings.value); });
    }

    private void ChangeMic(int index)
    {
        micIndex = index;
    }
}
