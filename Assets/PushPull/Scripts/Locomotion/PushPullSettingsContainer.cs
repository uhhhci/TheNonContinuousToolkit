using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPushPullSettings", menuName = "PushPull/PushPull Settings File")]
public class PushPullSettingsContainer : ScriptableObject
{
    public bool MovementVignette;
    public bool MovementDirection;
    public float MovementMultiplier;
    public float MovementStep;
    public float JoystickMovementMultiplier;
    public float JoystickMovementStep;

    public bool TurningVignette;
    public bool TurningDirection;
    public float TurningMultiplier;
    public float TurningStep;
    public float JoystickTurningMultiplier;
    public float JoystickTurningStep;

    public void SetValueByString(string name, float value)
    {
        switch (name)
        {
            case "MovementMultiplier":
                MovementMultiplier = value;
                break;
            case "MovementStep":
                MovementStep = value;
                break;
            case "JoystickMovementMultiplier":
                JoystickMovementMultiplier = value;
                break;
            case "JoystickMovementStep":
                JoystickMovementStep = value;
                break;
            case "TurningMultiplier":
                TurningMultiplier = value;
                break;
            case "TurningStep":
                TurningStep = value;
                break;
            case "JoystickTurningMultiplier":
                JoystickTurningMultiplier = value;
                break;
            case "JoystickTurningStep":
                JoystickTurningStep = value;
                break;
            case "MovementVignette":
                MovementVignette = value == 1 ? true : false;
                break;
            case "MovementDirection":
                MovementDirection = value == 1 ? true : false;
                break;
            case "TurningVignette":
                TurningVignette = value == 1 ? true : false;
                break;
            case "TurningDirection":
                TurningDirection = value == 1 ? true : false;
                break;
            default:
                break;
        }
    }

    public float GetValueByString(string name)
    {
        switch (name)
        {
            case "MovementMultiplier":
                return MovementMultiplier;
            case "MovementStep":
                return MovementStep;
            case "JoystickMovementMultiplier":
                return JoystickMovementMultiplier;
            case "JoystickMovementStep":
                return JoystickMovementStep;
            case "TurningMultiplier":
                return TurningMultiplier;
            case "TurningStep":
                return TurningStep;
            case "JoystickTurningMultiplier":
                return JoystickTurningMultiplier;
            case "JoystickTurningStep":
                return JoystickTurningStep;
            case "MovementVignette":
                return MovementVignette ? 1 : 0;
            case "MovementDirection":
                return MovementDirection ? 1 : 0;
            case "TurningVignette":
                return TurningVignette ? 1 : 0;
            case "TurningDirection":
                return TurningDirection ? 1 : 0;
            default:
                return 0f;
        }
    }
}
