using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Lipsync
{
    public class Speech : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] Image mouthImage;
        [SerializeField] private ButtonManager btnManager;
        [SerializeField] private ScriptableAudioObjects audioSettings;
        [SerializeField] private ScriptableImageObjects imageSettings;
        private Dictionary<float, Sprite> amplitudeMouthMappingFilter = new Dictionary<float, Sprite>();
        private float averageAmplitude;
        private const int defaultValue = 0;
        private const int defaultSamples = 256;

        /// <summary>
        /// Com todas as interações mapeadas no filtro, caso o valor médio de amplitude seja menor do filtro mapeado, carrega uma boca diferente
        /// Caso não seja menor que nenhum, a boca fica aberta.
        /// </summary>
        private void ChangeMouth(float averageAmplitude)
        {
            Sprite mouthSprite = imageSettings.mouthOpenSprite;
            foreach (var mouthFilter in amplitudeMouthMappingFilter)
            {
                if (averageAmplitude < mouthFilter.Key)
                {
                    mouthSprite = mouthFilter.Value;
                    break;
                }
            }
            mouthImage.sprite = mouthSprite;
        }

        /// <summary>
        /// Normaliza o volume do áudio para evitar que audios muito altos não consigam ser processados corretamente
        /// Passa sua amplitude média para trocar a imagem da boca.
        /// </summary>
        private void AnalyzeAudio()
        {
            float[] audioSamples = new float[defaultSamples];
            audioSource.GetOutputData(audioSamples, defaultValue);

            for (int i = defaultValue; i < audioSamples.Length; i++)
            {
                audioSamples[i] /= audioSettings.maxAttribute;
            }

            averageAmplitude = defaultValue;
            for (int i = default; i < audioSamples.Length; i++)
            {
                averageAmplitude += Mathf.Abs(audioSamples[i]);
            }
            averageAmplitude /= audioSamples.Length;
            ChangeMouth(averageAmplitude);
        }

        private void Start()
        {
            amplitudeMouthMappingFilter.Add(audioSettings.semiCloseMouthFilter, imageSettings.mouthClosedSprite);
            amplitudeMouthMappingFilter.Add(audioSettings.closeMouthFilter, imageSettings.mouthSemiOpenSprite);
        }

        private void Update()
        {
            if (audioSource.isPlaying)
            {
                AnalyzeAudio();
            }
        }
    }
}

