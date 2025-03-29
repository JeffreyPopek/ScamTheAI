using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public WindowOrderingManager.Windows windowType;
    
    [Header("UI Elements")] 
    [SerializeField] private GameObject window;
    [SerializeField] private Button xButton;
    [SerializeField] private Button taskbarIcon;
    [SerializeField] private GameObject taskbarHighlight;
    [SerializeField] private RectTransform draggableArea;


    public bool startMinimized;
    public bool settings;
    
    private bool _isWindowOpen;
    private readonly Vector2 _offScreenPos = new Vector2(-2000, 94);
    private Vector2 _lastPos;
    private RectTransform _windowRectTransform;
    private Vector2 _offset;

    private void Awake()
    {
        _isWindowOpen = true;

        xButton.onClick.AddListener(()=>ToggleWindow());
        
        if(!settings)
            taskbarIcon.onClick.AddListener(()=>ToggleWindow());

        _windowRectTransform = window.GetComponent<RectTransform>();
        _lastPos = _windowRectTransform.anchoredPosition;
        
        if (startMinimized)
            ToggleWindow();
    }

    private void ToggleWindow()
    {
        if (_isWindowOpen)
        {
            if (settings)
            {
                WindowsButtonManager.instance.settingsOpen = false;
                _windowRectTransform.anchoredPosition = _offScreenPos;
            }
            else
            {
                _lastPos = _windowRectTransform.anchoredPosition;
                _isWindowOpen = false;
                taskbarHighlight.SetActive(false);
                _windowRectTransform.anchoredPosition = _offScreenPos;
            }
        }
        else
        {
            _windowRectTransform.anchoredPosition = _lastPos;
            _isWindowOpen = true;
            taskbarHighlight.SetActive(true);
            WindowOrderingManager.instance.SetToTop(windowType);
        }
    }
    
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(draggableArea, eventData.position, eventData.pressEventCamera))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_windowRectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out _offset);
            _offset = (Vector2)_windowRectTransform.localPosition - _offset;
        }
        
        WindowOrderingManager.instance.SetToTop(windowType);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_windowRectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            _windowRectTransform.localPosition = localPoint + _offset;
    }
}
