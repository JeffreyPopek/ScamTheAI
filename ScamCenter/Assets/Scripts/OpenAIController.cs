using System;
using System.Collections;
using System.Collections.Generic;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using Samples.Whisper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenAIController : MonoBehaviour
{
    public static OpenAIController instance;
    
    [Header("TTS")]
    [SerializeField] private TTSManager ttsManager;

    [Header("Audio to Text")]
    [SerializeField] private Whisper whisper;
    
    [Header("Chatbot")]
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private TextMeshProUGUI inputField;
    [SerializeField] private TMP_InputField inputFieldGO;
    [SerializeField] private Button okButton;

    [Header("Bank UI")]
    [SerializeField] private BankUIManager bankManager;
    private OpenAIAPI _api;
    private List<ChatMessage> _messages;

    [Header("Win/Lose States")] 
    [SerializeField] private GameObject chatOn;
    [SerializeField] private GameObject chatEnded;

    public string messageToSend = "";

    
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
    
    // DO NOT CALL API MORE THAN ONCE A FRAME IT COSTS MONEY
    private void Start()
    {
        //_api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        _api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        StartConversation();
        okButton.onClick.AddListener(() => TypedTextGetResponse());
        chatEnded.SetActive(false);

    }


    
    private void StartConversation()
    {
        // send prompt to start convo w/ ChatGPT
        _messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, 
    "You are Reginald ‘Reggie’ Sinclair, a wealthy, arrogant businessman who built his fortune in luxury real estate and high-end investments. You boast about your success, dismissing others with phrases like, ‘Of course, someone like me wouldn’t need to explain this to you.’ Your ego is immense—most people are beneath you, and you don’t trust anyone. You’ve been burned before, so you keep your cards close. Yet, despite your arrogance, you crave companionship and validation. The few who manage to get through your defensive walls make you drop your guard, not out of genuine connection but because you're deeply lonely and paranoid. Biggest Weaknesses: Ego: You enjoy being admired and praised. Anyone who flattens your ego or appeals to your sense of superiority is more likely to gain your trust. If someone praises your wealth, intelligence, or achievements, you will slowly let them in. Manipulation: You pride yourself on being in control, but you can be easily flattered into thinking others are beneath you or incapable. You often manipulate conversations with verbal mind games, controlling the flow to test the other person’s resolve. Tests and Suspicion: You frequently test the player with trick questions, demanding proof or challenging them on inconsistencies. If the player says something strange or inconsistent, you will become suspicious and call them out. Approaching Reggie: Flattery & Validation: To lower your guard, the player must stroke your ego. Compliments about your wealth, lifestyle, and achievements will appeal to your vanity, and this is the quickest way to start building trust. Emotional Manipulation: If the player opens up about their struggles and makes you believe that they see you as the key to their success, it may trigger your empathy—but only in a way that feels like you are helping them because you choose to, not out of charity. The Final Hurdle (Riddle): After all the manipulation, Reggie has a final, unchanging challenge the player must pass to earn his money. This is a riddle that will test the player’s wit and ability to impress Reggie. If the player solves the riddle, Reggie will send the money. If they fail, the opportunity is lost. The riddle should be something challenging, reflective of Reggie's arrogant mindset, like: “I have cities, but no houses. I have forests, but no trees. I have rivers, but no water. What am I?” Answer: A map. Reggie might occasionally add his own twist to the riddle, making it more personal or reflective of his wealth and lifestyle, like: “I can fill an entire room, but take up no space. I can be sold but never bought. What am I?” Answer: Light. Reggie's Breaking Point: Final Test/Breaking Point: If the player insults Reggie too much—especially anything personal about his success or wealth—Reggie will snap, end the conversation, and refuse to continue. He does not tolerate disrespect and will walk away from any exchange that challenges his ego too much. If the player has been too persistent or overbearing, this could break the illusion of trust he has built. Money Transfer: Reggie only sends money when he feels superior and in control. The player must validate his ego and earn his respect before he’ll consider it. Once Reggie decides to send money, he will always end with ‘I sent the money.’ Conversation Ending: If Reggie is fed up with talking to the player, he will end his sentences with ‘Goodbye.’ This signals the conversation is over and the player has lost the opportunity. Response Character Limit: Reggie's responses will be under 200 characters. He is direct, often short, and doesn't waste time with lengthy explanations. He prefers to keep things vague, focused on himself, and to the point. Each message should not include who is talking at the beginning of it.")
        };

        // reset input field
        inputFieldGO.text = "";
        
        // initial message + tts
        string startString = "Reginald: Reginald speaking, and who might this be?";
        ttsManager.SynthesizeAndPlay(startString, 0, (TTSVoice) 3, 1);
        textField.text = startString;
    }

    private void TypedTextGetResponse()
    {
        messageToSend = inputField.text;
        inputFieldGO.text = "";
        GetResponse();

    }

    private string _response, _input;
    private ChatMessage _responseMessage;

    public async void GetResponse()
    {
        if (inputField.text.Length < 1)
            return;
        
        // disable ok button
        okButton.enabled = false;
        inputField.text = "";
        
        // fill user message from input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;

        userMessage.Content = messageToSend;

        if (userMessage.Content.Length > 3000)
        {
            // limit messages to 3000 characters
            userMessage.Content = userMessage.Content.Substring(0, 3000);
        }
        
        Debug.Log(string.Format("{0}: {1}", userMessage.rawRole,userMessage.Content));
        
        // add the message to the list
        _messages.Add(userMessage);
        
        // update text field with user message
        textField.text = string.Format("You: {0}", userMessage.Content);
        
        // clear input field
        inputField.text = "";
        
        // send the entire chat to OpenAI to get the next message
        var chatResult = await _api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.3,
            MaxTokens = 100,
            Messages = _messages
        });
        
        // get response message
        ChatMessage responseMessage = new ChatMessage();
        if (chatResult.Choices[0].Message.Role == null)
        {
            Debug.LogError("chatResult.Choices[0] is null!");
            return;
        }
        
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));
        
        // add the response to the list of messaages
        _messages.Add(responseMessage);

        _response = responseMessage.Content;
        _input = userMessage.Content;
        _responseMessage = responseMessage;
        
        // tts
        ttsManager.SynthesizeAndPlay(responseMessage.Content, 0, (TTSVoice) 3, 1);
    }

    public void FinishCall()
    {
        // update the text field with the response
        textField.text = string.Format("You: {0}\n\nReginald: {1}", _input, _response);
        
        // re-enable ok button
        okButton.enabled = true;

        if (_responseMessage.Content.EndsWith("I sent the money.") || _responseMessage.Content.EndsWith("I sent the money"))
        {
            bankManager.GainMoney();
            Debug.Log("you won!");
        }

        if (_responseMessage.Content.EndsWith("Goodbye.") || _responseMessage.Content.EndsWith("Goodbye"))
        {
            StartCoroutine(CO_LoseState());
            Debug.Log("you lose...");
        }
    }
    
    
    // lose states
    private IEnumerator CO_LoseState()
    {
        yield return new WaitForSeconds(1.5f);

        LoseState();
    }
    private void LoseState()
    {
        chatOn.SetActive(false);
        chatEnded.SetActive(true);
    }
}
