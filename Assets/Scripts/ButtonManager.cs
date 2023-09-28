using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Lipsync
{
  public class ButtonManager : MonoBehaviour
    {
        [SerializeField] private Button buttonPlay;
        [SerializeField] private Button buttonStop;
        [SerializeField] private TMP_Dropdown audioDropdown;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

        void Start()
        {
            CreateButtonFunc();
            StartDropdown();
            List<string> nameList = PopulateNameList();
            PopulateDropdown(nameList);
        }

        private void CreateButtonFunc()
        {
            buttonPlay.onClick.AddListener(() =>
            {
                audioSource.Play();
            });

            buttonStop.onClick.AddListener(() =>
            {
                audioSource.Stop();
            });
        }

        private void ChangeAudio(int audioIndex)
        {
            audioSource.clip = audioClips[audioIndex];
        }

        private List<string> PopulateNameList()
        {
            List<string> list = new List<string>();
            foreach (var a in audioClips)
            {
                list.Add(a.name);
            }
            return list;
        }

        private void PopulateDropdown(List<string> dropList)
        {
            audioDropdown.ClearOptions();
            audioDropdown.AddOptions(dropList);
        }

        private void StartDropdown()
        {
            ChangeAudio(audioDropdown.value);
            audioDropdown.onValueChanged.AddListener(delegate { ChangeAudio(audioDropdown.value); });
        }

    }

}
