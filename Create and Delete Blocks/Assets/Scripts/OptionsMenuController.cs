using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    //Audio source object 
    public AudioSource audioSource;

    // Res dropdown
    public Dropdown resolutionDD;

    // Unity resolutions
    Resolution[] resolutions;

    void Start()
    {
        // Get available resolutions
        resolutions = Screen.resolutions;

        // Clear dropdown labels
        resolutionDD.ClearOptions();

        List<string> res = new List<string>();

        int currentRes = 0; 

        for(int i = 0; i < resolutions.Length; i++)
        {
            // String formatting
            string option = resolutions[i].width + "x" + resolutions[i].height;
            res.Add(option);
            
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }

        // Add options to dropdown and default to current res
        resolutionDD.AddOptions(res);
        resolutionDD.value = currentRes;
        resolutionDD.RefreshShownValue();
    }

    // Set volume based on slider
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    // Set graphics quality (dropdown)
    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    // Set fullscreen (toggle)
    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    // Set resolution (dropdown)
    public void SetResolution(int resOption)
    {
        Resolution resolution = resolutions[resOption];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}

