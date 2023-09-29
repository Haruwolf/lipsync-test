using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_STANDALONE_WIN
using System.Windows.Forms;
#endif

namespace Lipsync
{
    public class LoadAudio : MonoBehaviour
    {
        private bool isLoading = false;
        public AudioClip UploadAudio(AudioSource audioSource)
        {
            string audioPath = ShowLoadDialog("Upload Audio", "", "wav,mp3"); // Filtro para tipos de arquivos de áudio
            if (!string.IsNullOrEmpty(audioPath))
            {
                AudioClip audioClip = LoadAudioClip(audioPath);
                if (audioClip != null)
                {
                    return audioClip;
                }
            }
            return null;
        }

        private string ShowLoadDialog(string title, string directory, string extension)
        {
            string path;
//#if UNITY_EDITOR
//            path = EditorUtility.OpenFilePanel(title, directory, extension);
#if UNITY_STANDALONE_WIN
            path = OpenFileUsingWindowsDialog();
            return path;
#endif

        }

        private AudioClip LoadAudioClip(string path)
        {
            if (File.Exists(path))
            {
                AudioType audioType = AudioType.UNKNOWN;

                string extension = Path.GetExtension(path).ToLower();
                if (extension == ".wav")
                    audioType = AudioType.WAV;
                else if (extension == ".mp3")
                    audioType = AudioType.MPEG;

                UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, audioType);
                request.SendWebRequest();

                while (!request.isDone)
                {
                    isLoading = true;
                }

                isLoading = false;
                if (request.result == UnityWebRequest.Result.Success)
                {
                    AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                    audioClip.name = Path.GetFileNameWithoutExtension(path);
                    return audioClip;
                }
                else
                {
                    Debug.LogError("Erro ao carregar o áudio: " + request.error);
                    return null;
                }
            }
            else
            {
                Debug.LogError("O arquivo de áudio não foi encontrado: " + path);
                return null;
            }
        }

        private string OpenFileUsingWindowsDialog()
        {
#if UNITY_STANDALONE_WIN
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "",
                Filter = "Arquivos de áudio (*" + ".wav;*.mp3)|*.wav;*.mp3|Todos os arquivos (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

#endif
            return null;
        }

        private void OnGUI()
        {
            if (isLoading)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.fontSize = 48; 
                style.normal.textColor = Color.black;

                Rect rect = new Rect(UnityEngine.Screen.width / 2 - 100, UnityEngine.Screen.height / 2 - 25, 200, 50);

                GUI.Label(rect, "Loading Audio...", style);
            }
        }
    }
}