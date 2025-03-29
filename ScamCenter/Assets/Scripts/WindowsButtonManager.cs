using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WindowsButtonManager : MonoBehaviour
{
    public static WindowsButtonManager instance;
    
    [SerializeField] private Button windowsButton;
    [SerializeField] private GameObject windowsWindow;
    [SerializeField] private GameObject settingsWindow;
    
    [SerializeField] private Button shutdownButton;
    [SerializeField] private Button settingsButton;


    private bool _windowOpen;
    public bool settingsOpen;

    private Vector2 _defaultSettingsPos = new Vector2(0, 94);

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
    
    private void Start()
    {
        windowsWindow.SetActive(false);
        settingsWindow.SetActive(false);
        _windowOpen = false;
        settingsOpen = false;
        
        windowsButton.onClick.AddListener(()=>ToggleWindow());
        shutdownButton.onClick.AddListener(()=>Shutdown());
        settingsButton.onClick.AddListener(()=>ShowSettings());
    }

    private void ToggleWindow()
    {
        if (_windowOpen)
        {
            windowsWindow.SetActive(false);
            _windowOpen = false;
        }
        else
        {
            windowsWindow.SetActive(true);
            _windowOpen = true;
        }
    }

    private void ShowSettings()
    {
        if (!settingsOpen)
        {
            settingsOpen = true;
            settingsWindow.SetActive(true);
            settingsWindow.GetComponent<RectTransform>().anchoredPosition = _defaultSettingsPos; 
        }
    }

    private void Shutdown()
    {
        Debug.Log("Shutting down...");
        Application.Quit();
    }
}
