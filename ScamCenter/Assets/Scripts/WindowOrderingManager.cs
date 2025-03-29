using System;
using UnityEngine;

public class WindowOrderingManager : MonoBehaviour
{
    public static WindowOrderingManager instance;
    
    [Header("Order")] 
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;
    
    [Header("Windows")] 
    [SerializeField] private GameObject chatWindow;
    [SerializeField] private GameObject bankWindow;
    [SerializeField] private GameObject notepadWindow;
    [SerializeField] private GameObject settingsWindow;
    
    public enum Windows
    {
        Chat,
        Bank,
        Notepad,
        Settings
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    public void SetToTop(Windows window)
    {
        ResetAll();
        switch (window)
        {
            case Windows.Chat:
                chatWindow.transform.SetParent(top.transform);
                break;
            
            case Windows.Bank:
                bankWindow.transform.SetParent(top.transform);
                break;
            
            case Windows.Notepad:
                notepadWindow.transform.SetParent(top.transform);
                break;
            
            case Windows.Settings:
                settingsWindow.transform.SetParent(top.transform);
                break;
        }
    }

    private void ResetAll()
    {
        chatWindow.transform.SetParent(bottom.transform);
        bankWindow.transform.SetParent(bottom.transform);
        notepadWindow.transform.SetParent(bottom.transform);
        settingsWindow.transform.SetParent(bottom.transform);
    }
}
