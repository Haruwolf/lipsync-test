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
        [SerializeField] private LoadAudio loadAudio;
        [SerializeField] private RecordManager recordManager;
        [SerializeField] private Button buttonPlay;
        [SerializeField] private Button buttonStop;
        [SerializeField] private Button buttonUpload;
        [SerializeField] private Toggle toggleRecording;
        [SerializeField] private TMP_Dropdown audioDropdown;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

        void Start()
        {
            CreateButtonFunc();
            StartDropdown();
            PopulateNameList();
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

            buttonUpload.onClick.AddListener(() =>
            {
                AudioClip clipLoaded = loadAudio.UploadAudio(audioSource);
                audioClips.Add(clipLoaded);
                PopulateNameList();
                ChangeAudio(audioClips.IndexOf(clipLoaded));
            });

            recordManager.recorded.AddListener(CreateRecordedClip);
            toggleRecording.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    audioSource.Stop();
                    recordManager.StartRecording();
                    UpdateBtnStates(value);
                    return;
                }

                UpdateBtnStates(value);
                recordManager.StopRecording();
            });
        }

        private void UpdateBtnStates(bool value)
        {
            buttonUpload.enabled = !value;
            buttonStop.enabled = !value;
            buttonPlay.enabled = !value;
        }

        private void CreateRecordedClip()
        {
            toggleRecording.isOn = false;
            AudioClip clipRecorded = recordManager.RecordedClip;
            clipRecorded.name = "audio_" + System.DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            audioClips.Add(clipRecorded);
            PopulateNameList();
            ChangeAudio(audioClips.IndexOf(clipRecorded));
        }


        private void ChangeAudio(int audioIndex)
        {
            audioSource.clip = audioClips[audioIndex];
            audioDropdown.value = audioIndex;
        }

        private void PopulateNameList()
        {
            List<string> list = new List<string>();
            foreach (var a in audioClips)
            {
                list.Add(a.name);
            }
            PopulateDropdown(list);
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
