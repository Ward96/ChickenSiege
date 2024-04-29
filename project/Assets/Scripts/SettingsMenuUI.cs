using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;

public class SettingsMenuUI : MonoBehaviour
{

    public Canvas SettingsMenuCanvas;
    public AudioMixer masterMixer;


    //get each slider, to set them on start
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider uiVolumeSlider;

    public Toggle allowGoreToggle;
    public Toggle cheatModeToggle;

    //settting each key for playerPrefs
    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const string UIVolumeKey = "UIVolume";
    private const string AllowGoreKey = "AllowGore";
    private const string CheatModeKey = "AllowCheats";
    private const string FirstTimeKey = "FirstTime";

    //the names of each of the files, used to reset saved data
    private string[] scoreFileNames = { "Grass1", "Grass2", "Desert1", "Desert2", "Terracotta1", "Terracotta2" }; //the file names of the scores

    public Canvas ConfirmationMenuCanvas; //the canvas for the confirmation menu used to confirm the player wants to wipe data

    // Start is called before the first frame update
    void Start()
    {
        SettingsMenuCanvas.gameObject.SetActive(false);

        if (!PlayerPrefs.HasKey(FirstTimeKey) || PlayerPrefs.GetInt(FirstTimeKey) == 1) //sets the first time launch key
        {
            PlayerPrefs.SetInt(FirstTimeKey, 1);
            PlayerPrefs.Save();
        }

        LoadSavedVolume();
        LoadToggles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MasterVolumeControl(float volume)
    {
        masterMixer.SetFloat("masterVolumeParameter", volume);
        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
    }
    public void MusicVolumeControl(float volume)
    {
        masterMixer.SetFloat("musicVolumeParameter", volume);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }
    public void SFXVolumeControl(float volume)
    {
        masterMixer.SetFloat("SFXVolumeParameter", volume);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }
    public void UIVolumeControl(float volume)
    {
        masterMixer.SetFloat("UIVolumeParameter", volume);
        PlayerPrefs.SetFloat(UIVolumeKey, volume);
    }

    public void ToggleGore(bool allowGore)
    {
        PlayerPrefs.SetInt(AllowGoreKey, allowGore ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleCheatMode(bool allowCheats)
    {
        PlayerPrefs.SetInt(CheatModeKey, allowCheats ? 1 : 0);
    }

    public void ShowSettings()
    {
        SettingsMenuCanvas.gameObject.SetActive(true);
    }
    public void HideSettings()
    {
        SettingsMenuCanvas.gameObject.SetActive(false);
    }

    private void LoadSavedVolume()
    {
        if (PlayerPrefs.HasKey(MasterVolumeKey))
            masterVolumeSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey);
        if (PlayerPrefs.HasKey(MusicVolumeKey))
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey);
        if (PlayerPrefs.HasKey(SFXVolumeKey))
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFXVolumeKey);
        if (PlayerPrefs.HasKey(UIVolumeKey))
            uiVolumeSlider.value = PlayerPrefs.GetFloat(UIVolumeKey);
    }

    private void LoadToggles()
    {
        if (allowGoreToggle != null && PlayerPrefs.HasKey(AllowGoreKey))
            allowGoreToggle.isOn = PlayerPrefs.GetInt(AllowGoreKey) == 1;

        if (cheatModeToggle != null && PlayerPrefs.HasKey(CheatModeKey))
            cheatModeToggle.isOn = PlayerPrefs.GetInt(CheatModeKey) == 1;
    }

    public void ResetSavedStars() //resets the stars so the player can earn them again!
    {
        for (int x = 0; x < scoreFileNames.Length; x++)
        {
            string path = Application.streamingAssetsPath + "/PlayerScores/" + scoreFileNames[x] + ".txt"; //create the path at x

            if (!File.Exists(path)) //if file does not exist, print
            {
                print("file does not exist");
            }
            else
            {
                File.WriteAllText(path, "0");//rewrite all files with 0, effectively resetting the scores
            }
        }
    }

    public void ShowConfirmMenu()//menu to confirm that the player wishes to reset the data
    {
        SettingsMenuCanvas.gameObject.SetActive(false); //set the settings canvas to false
        ConfirmationMenuCanvas.gameObject.SetActive(true); //set the confirm canvas to true
    }

    public void HideConfirmMenu()//hide the confrim menu
    {
        SettingsMenuCanvas.gameObject.SetActive(true); //set the settings canvas to true
        ConfirmationMenuCanvas.gameObject.SetActive(false); //set the confirm canvas to false
    }


}

