using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullBrain : MonoBehaviour
{
    public static PushPullBrain _instance;
    public PushPullJoystickLocomotion _pushPullJoystickLocomotion;
    public PushPullMovement _pushPullMovement;
    public PushPullTurning _pushPullTurning;
    public Teleport _teleport;
    public PushPullSettingsContainer _currentSettings;
    public enum LocomotionMovement { PushPullMovement, JoystickMovement, Teleport };
    public enum LocomotionTurning { PushPullTurning, JoystickTurning };

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

    private void Start()
    {
        UpdateLocomotionScripts();
    }

    //Used in tutorial to disable individual locomotion types. End any ongoing locomotion and enable/disable the script
    public void EnableLocomotionType(LocomotionMovement movement, bool enable)
    {
        switch (movement)
        {
            case LocomotionMovement.PushPullMovement:
                _pushPullMovement.EnableMovement(enable);
                _pushPullMovement.enabled = enable;
                break;
            case LocomotionMovement.JoystickMovement:
                _pushPullJoystickLocomotion.EnableMovement(enable);
                break;
            case LocomotionMovement.Teleport:
                _teleport.EnableMovement(enable);
                _teleport.enabled = enable;
                break;
            default:
                break;
        }
    }

    //Used in tutorial to disable individual locomotion types. End any ongoing locomotion and enable/disable the script
    public void EnableLocomotionType(LocomotionTurning turning, bool enable)
    {
        switch (turning)
        {
            case LocomotionTurning.PushPullTurning:
                _pushPullTurning.EnableTurning(enable);
                _pushPullTurning.enabled = enable;
                break;
            case LocomotionTurning.JoystickTurning:
                _pushPullJoystickLocomotion.EnableTurning(enable);
                break;
            default:
                break;
        }
    }

    public void ChangeActiveStatusMovement(LocomotionMovement type, bool active)
    {
        if (type == LocomotionMovement.Teleport)
        {
            _pushPullMovement.EnableMovement(!active);
            _pushPullJoystickLocomotion.EnableMovement(!active);
        }
        if (type == LocomotionMovement.JoystickMovement)
        {
            _pushPullMovement.EnableMovement(!active);
        }
    }

    public void ChangeActiveStatusTurning(LocomotionTurning type, bool active)
    {
        if (type == LocomotionTurning.JoystickTurning)
        {
            _pushPullTurning.EnableTurning(!active);
        }
    }

    public void UpdateSettings(PushPullSettingsContainer settingsContainer)
    {
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
    }

    public void UpdateLocomotionScripts()
    {
        UpdateMovementVignette(_currentSettings.GetValueByString("MovementVignette") == 1);
        UpdateMovementDirection(_currentSettings.GetValueByString("MovementDirection") == 1);
        UpdateMovementMultiplier(_currentSettings.GetValueByString("MovementMultiplier"));
        UpdateMovementStep(_currentSettings.GetValueByString("MovementStep"));
        UpdateJoystickMovementMultiplier(_currentSettings.GetValueByString("JoystickMovementMultiplier"));
        UpdateJoystickMovementStep(_currentSettings.GetValueByString("JoystickMovementStep"));

        UpdateTurningVignette(_currentSettings.GetValueByString("TurningVignette") == 1);
        UpdateTurningDirection(_currentSettings.GetValueByString("TurningDirection") == 1);
        UpdateTurningMultiplier(_currentSettings.GetValueByString("TurningMultiplier"));
        UpdateTurningStep(_currentSettings.GetValueByString("TurningStep"));
        UpdateJoystickTurningMultiplier(_currentSettings.GetValueByString("JoystickTurningMultiplier"));
        UpdateJoystickTurningStep(_currentSettings.GetValueByString("JoystickTurningStep"));

    }

    private void UpdateMovementVignette(bool enable)
    {
        _pushPullJoystickLocomotion._vignetteDuringMovement = enable;
        _pushPullMovement._vignetteDuringMovement = enable;
    }

    private void UpdateTurningVignette(bool enable)
    {
        _pushPullJoystickLocomotion._vignetteDuringTurning = enable;
        _pushPullTurning._vignetteDuringTurning = enable;
    }

    public void UpdateMovementStep(float stepSize)
    {
        _pushPullMovement._movementStep = stepSize;
    }

    public void UpdateJoystickMovementStep(float stepSize)
    {
        _pushPullJoystickLocomotion._movementStep = stepSize;
    }

    public void UpdateJoystickMovementMultiplier(float multiplier)
    {
        _pushPullJoystickLocomotion._movementMultiplier = multiplier;
    }

    public void UpdateMovementMultiplier(float multiplier)
    {
        _pushPullMovement._multiplier = multiplier;
    }

    public void UpdateMovementDirection(bool invert)
    {
        _pushPullMovement._invertDirection = invert;
    }

    public void UpdateTurningStep(float stepSize)
    {
        _pushPullTurning._turningStep = stepSize;
    }

    public void UpdateJoystickTurningMultiplier(float multiplier)
    {
        _pushPullJoystickLocomotion._turningMultiplier = multiplier;
    }

    public void UpdateJoystickTurningStep(float stepSize)
    {
        _pushPullJoystickLocomotion._turningStep = stepSize;
    }

    public void UpdateTurningMultiplier(float multiplier)
    {
        _pushPullTurning._multiplier = multiplier;
    }

    public void UpdateTurningDirection(bool invert)
    {
        _pushPullTurning._invertDirection = invert;
    }
}
