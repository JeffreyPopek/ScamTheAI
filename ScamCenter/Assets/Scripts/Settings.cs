using System;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Wallpaper")]
    [SerializeField] private Image wallpaper;
    
    [Header("Button")]
    [SerializeField] private Button wallpaper1Button;
    [SerializeField] private Button wallpaper2Button;
    [SerializeField] private Button wallpaper3Button;
    [SerializeField] private Button wallpaper4Button;
    
    [Header("Highlights")]
    [SerializeField] private GameObject wallpaper1Highlight;
    [SerializeField] private GameObject wallpaper2Highlight;
    [SerializeField] private GameObject wallpaper3Highlight;
    [SerializeField] private GameObject wallpaper4Highlight;
    
    [Header("Wallpaper Images")]
    [SerializeField] private Sprite wallpaper1Img;
    [SerializeField] private Sprite wallpaper2Img;
    [SerializeField] private Sprite wallpaper3Img;
    [SerializeField] private Sprite wallpaper4Img;

    private int _currentWallpaper;

    private void Start()
    {
        _currentWallpaper = 1;
        DetermineHighlight();
        
        wallpaper1Button.onClick.AddListener(()=>SetWallpaper1());
        wallpaper2Button.onClick.AddListener(()=>SetWallpaper2());
        wallpaper3Button.onClick.AddListener(()=>SetWallpaper3());
        wallpaper4Button.onClick.AddListener(()=>SetWallpaper4());
    }

    private void SetWallpaper1()
    {
        wallpaper.sprite = wallpaper1Img;
        _currentWallpaper = 1;
        DetermineHighlight();
    }
    
    private void SetWallpaper2()
    {
        wallpaper.sprite = wallpaper2Img;
        _currentWallpaper = 2;
        DetermineHighlight();
    }
    
    private void SetWallpaper3()
    {
        wallpaper.sprite = wallpaper3Img;
        _currentWallpaper = 3;
        DetermineHighlight();
    }
    
    private void SetWallpaper4()
    {
        wallpaper.sprite = wallpaper4Img;
        _currentWallpaper = 4;
        DetermineHighlight();
    }

    private void DetermineHighlight()
    {
        wallpaper1Highlight.SetActive(false);
        wallpaper2Highlight.SetActive(false);
        wallpaper3Highlight.SetActive(false);
        wallpaper4Highlight.SetActive(false);
        
        switch (_currentWallpaper)
        {
            case 1:
                wallpaper1Highlight.SetActive(true);
                break;
            
            case 2:
                wallpaper2Highlight.SetActive(true);
                break;
            
            case 3:
                wallpaper3Highlight.SetActive(true);
                break;
            
            case 4:
                wallpaper4Highlight.SetActive(true);
                break;
        }
    }

}
