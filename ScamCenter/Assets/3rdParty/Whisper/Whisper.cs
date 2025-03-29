using OpenAI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Samples.Whisper
{
    public class Whisper : MonoBehaviour
    {
        [SerializeField] private OpenAIController controller;
        [SerializeField] private Button recordButton;
        [SerializeField] private Button stopRecordButton;
        [SerializeField] private TMP_Dropdown dropdown;
        
        private readonly string fileName = "output.wav";
        private readonly int duration = 20;
        
        private AudioClip clip;
        private bool isRecording;
        private float time;
        private OpenAIApi openai = new OpenAIApi();
        
        
        // message to send to ai
        private string _messageToSend;

        public string GetMessage()
        {
            return _messageToSend;
        }

        private void Start()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            dropdown.options.Add(new Dropdown.OptionData("Microphone not supported on WebGL"));
            #else
            
            dropdown.options.Clear();
            
            foreach (var device in Microphone.devices)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(device));
            }
            recordButton.onClick.AddListener(StartRecording);
            stopRecordButton.onClick.AddListener(EndRecording);
            dropdown.onValueChanged.AddListener(ChangeMicrophone);
            
            var index = PlayerPrefs.GetInt("user-mic-device-index");
            dropdown.SetValueWithoutNotify(index);
            #endif
        }
        

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }
        
        private void StartRecording()
        {
            Debug.Log("Recording...");
            
            isRecording = true;
            recordButton.enabled = false;

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            
            #if !UNITY_WEBGL
            clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
            #endif
        }

        private async void EndRecording()
        {
            Debug.Log("Finished Recording, transcribing message...");
            //message.text = "Transcripting...";
            
            #if !UNITY_WEBGL
            Microphone.End(null);
            #endif
            
            byte[] data = SaveWav.Save(fileName, clip);
            
            var req = new CreateAudioTranscriptionsRequest
            {
                FileData = new FileData() {Data = data, Name = "audio.wav"},
                // File = Application.persistentDataPath + "/" + fileName,
                Model = "whisper-1",
                Language = "en"
            };
            var res = await openai.CreateAudioTranscription(req);

            // progressBar.fillAmount = 0;
            // message.text = res.Text;
            controller.messageToSend = res.Text;
            controller.GetResponse();
            
            recordButton.enabled = true;
        }

        private void Update()
        {
            // if (Input.GetMouseButtonDown(0) && !isRecording)
            //     StartRecording();
            
            if (isRecording)
            {
                time += Time.deltaTime;
                // progressBar.fillAmount = time / duration;
                
                if (time >= duration)
                {
                    time = 0;
                    isRecording = false;
                    EndRecording();
                }
                //
                // if (Input.GetMouseButtonUp(0))
                // {
                //     isRecording = false;
                //     EndRecording();
                // }
            }
        }
        
        
    }
}
