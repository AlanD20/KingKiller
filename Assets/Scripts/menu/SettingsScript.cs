using System.ComponentModel;
using System.Globalization;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    public static SettingsScript Instance;
    [SerializeField] Slider SliderX;
    // [SerializeField] TMP_Text Xaxis_Numb;
    [SerializeField] Slider SliderY;
    // [SerializeField] TMP_Text Yaxis_Numb;
    [SerializeField] TMP_InputField XsenseField;
    [SerializeField] TMP_InputField YsenseField;
    [SerializeField] AudioMixer GameVolume;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // Xaxis.value = CameraScript.SenseX;
        // Yaxis.value = CameraScript.SenseY;
        SliderX.value = CameraScript.sensitivity.x;
        SliderY.value = CameraScript.sensitivity.y;
    }
    void Update()
    {
        // Xaxis_Numb.text = Xaxis.value.ToString("F3");
        // Yaxis_Numb.text = Yaxis.value.ToString("F3");

    }
    public void SliderValueChange()
    {
        XsenseField.text = SliderX.value.ToString("F3");
        YsenseField.text = SliderY.value.ToString("F3");
    }
    public void InputValueChange()
    {
        float NewXsense = ConvertFloat(XsenseField.text);
        float NewYsense = ConvertFloat(YsenseField.text);
        if (NewXsense > 7f) XsenseField.text = "7";
        if (NewYsense > 7f) YsenseField.text = "7";
        SliderX.value = NewXsense > 7f ? 7f : NewXsense;
        SliderY.value = NewYsense > 7f ? 7f : NewYsense;
    }
    float ConvertFloat(string text)
    {
        float result = 0f;
        float.TryParse(text, out result);
        return result;
    }

    public void Save()
    {
        UpdateSensitivity();
    }
    public void setَQuality(int QualityIndex)
    {
        QualitySettings.SetQualityLevel(QualityIndex);
    }

    void UpdateSensitivity()
    {
        // CameraScript.SenseX = Xaxis.value;
        // CameraScript.SenseY = Yaxis.value;
        CameraScript.sensitivity.x = SliderX.value;
        CameraScript.sensitivity.y = SliderY.value;
        PlayerPrefs.SetFloat("XSense", CameraScript.sensitivity.x);
        PlayerPrefs.SetFloat("YSense", CameraScript.sensitivity.y);
    }
    public void UpdateVolume(float volume)
    {
        GameVolume.SetFloat("volume", volume);
    }

}
