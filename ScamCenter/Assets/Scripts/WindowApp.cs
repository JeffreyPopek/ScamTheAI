using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WindowApp : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("UI Elements")] 
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject taskbarIcon;
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject minimizeButton;
    [SerializeField] private GameObject topBar;

    public bool windowOpen;

    private RectTransform _rectTransform;

    public void Start()
    {
        windowOpen = true;

        _rectTransform = GetComponent<RectTransform>();
    }

    // public void OnMouseDrag()
    // {
    //     topBar.transform.position = Input.mousePosition;
    // }

    public void ClickTaskBarIcon()
    {
        if (windowOpen)
        {
            windowOpen = false;
            // minimize
        }
        else
        {
            windowOpen = true;
            // maximize
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
