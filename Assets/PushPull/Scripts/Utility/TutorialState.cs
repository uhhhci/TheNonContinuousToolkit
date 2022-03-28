using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PushPull/Tutorial State")]
public class TutorialState : ScriptableObject
{
    public string _messageBoardText;
    public bool _enableTeleportMovement;
    public bool _enableJoystickMovement;
    public bool _enableJoystickTurning;
    public bool _enablePushPullMovement;
    public bool _enablePushPullTurning;
    public bool _enableButtonInput = true;
    public Texture2D _image;
    public GameEvent _event;
}
