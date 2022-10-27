using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] TMP_Dropdown fpsDropdown;
	[SerializeField] Toggle advanced_isMobileInputToggle;
	[SerializeField] Slider LJSensitivitySlider;
	[SerializeField] Slider RJSensitivitySlider;
	[SerializeField] Toggle inverseMouseWheelToggle;

	public static SettingsManager Instance;

	//default values
	int default_fpsIndex = 1;
	float default_leftJoystickSensitivity = 1;
	float default_rightJoystickSensitivity = 1;
	bool default_inverseMouseWheel = false;

	void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

		LoadSettings();
    }

	public void LoadSettings() 
	{
		int fpsIndex = ES3.Load("settings_FPS", default_fpsIndex);
		SetFPS(fpsIndex);
		fpsDropdown.value = fpsIndex;

		bool isMobile = ES3.Load("settings_isMobile", Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
		SetIsMobile(isMobile);
		advanced_isMobileInputToggle.isOn = isMobile;

		float LJSensitivity = ES3.Load("settings_virtualJoystickSensitivityLeft", default_leftJoystickSensitivity);
		//print("LJSensitivity is " + LJSensitivity);
		SetVirtualJoystickSensitivityLeft(LJSensitivity);
		LJSensitivitySlider.value = LJSensitivity;

		float RJSensitivity = ES3.Load("settings_virtualJoystickSensitivityRight", default_rightJoystickSensitivity);
		//print("RJSensitovoty is " + RJSensitivity);
		SetVirtualJoystickSensitivityRight(RJSensitivity);
		RJSensitivitySlider.value = RJSensitivity;

		bool isInverseMouseWheel = ES3.Load("settings_inverseMouseWheel", default_inverseMouseWheel);
		SetInverseMouseWheel(isMobile);
		inverseMouseWheelToggle.isOn = isInverseMouseWheel;
	}

	#region set and saves

	public void SetFPS(int value)
	{
		ES3.Save("settings_FPS", value);

		switch (value)
		{
			case 0:
				Application.targetFrameRate = 30;
				//quality_index
				break;
			case 1:
				Application.targetFrameRate = 60;
				//quality_index
				break;
			case 2:
				Application.targetFrameRate = 120;
				//quality_index
				break;
		}
	}
    public void SetIsMobile(bool value)
	{
        ES3.Save("settings_isMobile", value);
		Settings.SetIsMobile(value);
	}

	public void SetInverseMouseWheel(bool value)
	{
		ES3.Save("settings_inverseMouseWheel", value);
		Settings.SetInverseMouseWheel(value); //TODO change 
	}

    public void SetVirtualJoystickSensitivityLeft(float value)
	{
		ES3.Save("settings_virtualJoystickSensitivityLeft", value);
		Settings.SetLJSensitivity(value);
    }

	public void SetVirtualJoystickSensitivityRight(float value)
	{
		ES3.Save("settings_virtualJoystickSensitivityRight", value);
		Settings.SetRJSensitivity(value);
	}

	#endregion
}

public static class Settings
{
	public static bool isMobile { get; private set; }
	public static float LJSensitivity { get; private set; }
	public static float RJSensitivity { get; private set; }
	public static bool inverseMouseWheel { get; private set; }

	public static void SetIsMobile(bool value)
	{
		isMobile = value;
	}

	public static void SetLJSensitivity(float value)
	{
		LJSensitivity = value;
	}

	public static void SetRJSensitivity(float value)
	{
		RJSensitivity = value;
	}

	public static void SetInverseMouseWheel(bool value)
	{
		inverseMouseWheel = value;
	}
}