using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject SettingsMenu;

    public AudioMixer Mixer;
    
    private void Start()
    {
        if (!SettingsMenu)
            SettingsMenu = GameObject.FindWithTag("OptionsMenu");
        SettingsMenu.GetComponent<OptionsMenu>().LoadSettings();
    }


    public void CreateSettingsMenu()
    {
        SettingsMenu.SetActive(true);
    }

    public void DestroySettingsMenu()
    {
        SettingsMenu.SetActive(false);

    }
}
