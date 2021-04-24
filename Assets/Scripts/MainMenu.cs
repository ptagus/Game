using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class MusicSettings
{
    public static bool music;
    public static float volume;
    public static bool beStart;
}

public class MainMenu : MonoBehaviour
{

    public GameObject settingPanel;
    public Slider volume;
    public Toggle toggle;
    float musicVolume;
    public AudioSource ac;

    private void Start()
    {
        if (!MusicSettings.beStart)
        {
            MusicSettings.beStart = true;
            musicVolume = volume.value;
            MusicSettings.volume = musicVolume;
        }
        Debug.Log(MusicSettings.volume);
        CheckVolume();
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Settings()
    {
        settingPanel.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void MusicOffOn()
    {
        MusicSettings.music = toggle.isOn;
        CheckVolume();
    }

    public void Volume()
    {
        musicVolume = volume.value;
        MusicSettings.volume = musicVolume;
        CheckVolume();
    }

    public void CloseSettingsPanel()
    {
        settingPanel.SetActive(false);
    }

    void CheckVolume()
    {
        if (MusicSettings.music)
        {
            ac.mute = true;
            return;
        }
        else
        {
            ac.mute = false;
        }
        ac.volume = MusicSettings.volume;
    }
}
