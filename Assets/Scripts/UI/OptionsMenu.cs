using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider SensSlider;
    public TextMeshProUGUI SensLabel;
    public TMP_InputField SensTextInput;

    public Slider MasterVolumeSlider;
    public TextMeshProUGUI MasterVolumeLabel;

    public AudioMixer Mixer;
    
    private float playerSensitivity;
    private float playerMasterVolume;
    // Start is called before the first frame update
    void Start()
    {
        LoadSettings();

        SensSlider.value = playerSensitivity;
        SensTextInput.text = $"{playerSensitivity:F2}";

        MasterVolumeSlider.value = playerMasterVolume;
        MasterVolumeLabel.text = $"Volume: {playerMasterVolume:F2}";
    }

    public void ApplyPlayerPrefs()
    {
        PlayerPrefs.SetFloat("Sensitivity", playerSensitivity);
        PlayerPrefs.SetFloat("MasterVolume", playerMasterVolume);

        PlayerPrefs.Save();
    }

    public void UpdatedSensSlider(float value)
    {
        SensTextInput.text = $"{value:F2}";
        playerSensitivity = value;
    }
    
    public void UpdatedSensInput(string value)
    {
        float fvalue = float.Parse(value);
        SensSlider.value = fvalue;
        playerSensitivity = fvalue;
    }
    
    public void UpdatedMainVolumeSlider(float value)
    {
        MasterVolumeLabel.text = $"Volume: {value:F2}";
        playerMasterVolume = value;
    }

    public void CancelSettings()
    {
        gameObject.SetActive(false);
    }

    public void AcceptSettings()
    {
        ApplyPlayerPrefs();

        LoadSettings();

        gameObject.SetActive(false);
    }

    public void LoadSettings()
    {
        playerSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.0f);
        playerMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        
        Mixer.SetFloat("MasterVolume", linearTodB(playerMasterVolume));
    }

    private float linearTodB(float linearValue)
    {
        var dB = 20f * Mathf.Log10(linearValue * linearValue); // linearValue ranges 0.0 to 1.0
        if (float.IsInfinity(dB))
            dB = -80f;
        return dB;
    }

    private float dBtoLinear(float dB)
    {
        return Mathf.Pow(10.0f, dB / 20.0f);
    }
}
