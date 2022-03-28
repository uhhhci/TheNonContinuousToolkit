using System.Collections.Generic;
using UnityEngine;

public class PushPullSettingsManager : MonoBehaviour
{
    public static PushPullSettingsManager _instance;
    public PushPullSettingsContainer _presetDefault;
    public PushPullSettingsContainer _presetComfort;
    public PushPullSettingsContainer _presetEfficiency;
    public PushPullSettingsContainer _presetImmersive;
    public PushPullSettingsContainer _currentSettings;
    private List<UI_MultiChoice> _buttons = new List<UI_MultiChoice>();
    private enum PushPullSettingsPreset { Default, Comfort, Efficiency, Immersive };

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ApplyPresetDefault()
    {
        ApplyPresetSettings(PushPullSettingsPreset.Default);
    }

    public void ApplyPresetComfort()
    {
        ApplyPresetSettings(PushPullSettingsPreset.Comfort);
    }

    public void ApplyPresetEfficiency()
    {
        ApplyPresetSettings(PushPullSettingsPreset.Efficiency);
    }

    public void ApplyPresetImmersive()
    {
        ApplyPresetSettings(PushPullSettingsPreset.Immersive);
    }

    private void ApplyPresetSettings(PushPullSettingsPreset preset)
    {
        PushPullSettingsContainer settingsContainer = _presetDefault;
        switch (preset)
        {
            case PushPullSettingsPreset.Default:
                settingsContainer = _presetDefault;
                break;
            case PushPullSettingsPreset.Comfort:
                settingsContainer = _presetComfort;
                break;
            case PushPullSettingsPreset.Efficiency:
                settingsContainer = _presetEfficiency;
                break;
            case PushPullSettingsPreset.Immersive:
                settingsContainer = _presetImmersive;
                break;
        }
        _currentSettings.MovementVignette = settingsContainer.MovementVignette;
        _currentSettings.MovementDirection = settingsContainer.MovementDirection;
        _currentSettings.MovementMultiplier = settingsContainer.MovementMultiplier;
        _currentSettings.MovementStep = settingsContainer.MovementStep;
        _currentSettings.JoystickMovementMultiplier = settingsContainer.JoystickMovementMultiplier;
        _currentSettings.JoystickMovementStep = settingsContainer.JoystickMovementStep;

        _currentSettings.TurningVignette = settingsContainer.TurningVignette;
        _currentSettings.TurningDirection = settingsContainer.TurningDirection;
        _currentSettings.TurningMultiplier = settingsContainer.TurningMultiplier;
        _currentSettings.TurningStep = settingsContainer.TurningStep;
        _currentSettings.JoystickTurningMultiplier = settingsContainer.JoystickTurningMultiplier;
        _currentSettings.JoystickTurningStep = settingsContainer.JoystickTurningStep;

        PushPullBrain._instance.UpdateLocomotionScripts();

        foreach (UI_MultiChoice button in _buttons)
        {
            button.PullSettingFromManager();
        }
    }

    public void RegisterButton(UI_MultiChoice button)
    {
        if (!_buttons.Contains(button))
            _buttons.Add(button);
    }

    public float GetValue(string name)
    {
        return _currentSettings.GetValueByString(name);
    }

    public void SetValue(string name, float value)
    {
        _currentSettings.SetValueByString(name, value);
        PushPullBrain._instance.UpdateLocomotionScripts();
    }
}
