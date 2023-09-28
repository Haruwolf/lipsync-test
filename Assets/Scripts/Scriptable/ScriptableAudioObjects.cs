using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioCalibrationSettings")]
public class ScriptableAudioObjects : ScriptableObject
{
    public float maxAttribute = 0.1f;
    public float closeMouthFilter = 0.5f;
    public float semiCloseMouthFilter = 0.2f;
}
