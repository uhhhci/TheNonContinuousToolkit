using UnityEngine;

/*
 * PushPull Joystick Locomotion by Jann Philipp Freiwald, 2020
 * Put on Root PlayerController Object
 */
public class PushPullJoystickLocomotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OVRInput.Axis2D _movementJoystick = OVRInput.Axis2D.PrimaryThumbstick; //OVR Input for movement
    [SerializeField] private OVRInput.Axis2D _turningJoystick = OVRInput.Axis2D.SecondaryThumbstick; //OVR Input for turning
    [SerializeField] private GameObject _player; //VR player root object
    [SerializeField] private Transform _vrCamera; //VR Camera within VR player root object
    [SerializeField] private VignetteController _vignetteController; //controls OVR Vignette or VR Tunneling Pro
    [SerializeField] private GameEvent _eventStutterSteppedMovementBegin; //disables IK in Agent Avatars
    [SerializeField] private GameEvent _eventStutterSteppedMovementEnd; //enables IK in Agent Avatars

    [Header("Parameters")]
    public float _turningStep = 30f; //step size for one turn. If set to zero will use continuous turning instead
    public float _turningCooldown = 0.33f; //The interval between turning steps while the joystick is held
    public float _continuousTurningSpeed = 0.5f; //The fixed speed at which the vr Player is turned when _turningStep is set to zero
    public float _movementSpeed = 0.05f; //The speed at which the VR player is moved
    public float _movementCooldown = 0.33f; //The interval between teleport movement steps while the joystick is held
    public float _movementStep = 0.5f; //The distance of one teleport movement step
    public bool _vignetteDuringMovement;
    public bool _vignetteDuringTurning;

    private Vector2 _movementAxis = Vector2.zero;
    private Vector2 _turningAxis = Vector2.zero;
    private float _lastMovementTime; //Used to calculate when the next teleport movement step can be triggered by holding the joystick
    private bool _noMovementInputLastFrame; //If there was no movement input last frame a teleport movement step can be performed regardless of cooldown
    private float _lastTurnTime; //Used to calculate when the next turning step can be triggered by holding the joystick
    private bool _continuousTurningLeft; //Is the _turningStep zero and is the player currently turning left via button input?
    private bool _continuousTurningRight; //Is the _turningStep zero and is the player currently turning right via button input?
    private bool _noTurningInputLastFrame; //If there was no turning input last frame a turning step can be performed regardless of cooldown

    private bool _movementEnabled = true;
    private bool _turningEnabled = true;
    public float _movementMultiplier = 1f; //Used for continuous movement
    public float _turningMultiplier = 1f; //Used for continuous movement


    //FixedUpdate is called 50 times per second
    private void FixedUpdate()
    {
        if (_movementEnabled)
            CalculateMovement();
        if (_turningEnabled)
            CalculateRotation();
        UpdateVignette();
    }

    private void Update()
    {
        _movementAxis = OVRInput.Get(_movementJoystick);
        _turningAxis = OVRInput.Get(_turningJoystick);
    }

    //Read the joystick movement input and perform a continuous or teleport movement step
    private void CalculateMovement()
    {
        Vector3 movement = new Vector3(_movementAxis.x, 0, _movementAxis.y);

        if (movement.magnitude > 0)
        {
            PushPullBrain._instance.ChangeActiveStatusMovement(PushPullBrain.LocomotionMovement.JoystickMovement, true);
            if (_movementStep == 0)
            {
                movement *= _movementSpeed;
                movement *= _movementMultiplier;
                movement = _vrCamera.TransformDirection(movement);
                movement.y = 0f; //This constraints the movement to the XY-plane

                _player.transform.position += movement;
            }
            else
            {
                if (_noMovementInputLastFrame || movement.magnitude > 0 && Time.time > _lastMovementTime + _movementCooldown / _movementMultiplier)
                {
                    _eventStutterSteppedMovementBegin.Raise();
                    _lastMovementTime = Time.time;
                    movement = _vrCamera.TransformDirection(movement);
                    movement.y = 0f; //This constraints the movement to the XY-plane
                    movement.Normalize();
                    movement *= _movementStep;

                    _player.transform.position += movement;
                }
            }
        }
        else
        {
            PushPullBrain._instance.ChangeActiveStatusMovement(PushPullBrain.LocomotionMovement.JoystickMovement, false);
            if (!_noMovementInputLastFrame)
                _eventStutterSteppedMovementEnd.Raise();
        }

        _noMovementInputLastFrame = (movement.magnitude == 0);
    }

    //Read the joystick rotation input and perform a continuous or stepped rotation
    private void CalculateRotation()
    {
        Vector3 turning = new Vector3(_turningAxis.x, 0, _turningAxis.y);
        if (turning.magnitude > 0)
        {
            PushPullBrain._instance.ChangeActiveStatusTurning(PushPullBrain.LocomotionTurning.JoystickTurning, true);
        }
        else
        {
            PushPullBrain._instance.ChangeActiveStatusTurning(PushPullBrain.LocomotionTurning.JoystickTurning, false);
        }

        if (_turningStep == 0)
        {
            _continuousTurningRight = (turning.x > 0);
            _continuousTurningLeft = (turning.x < 0);
        }
        else
        {
            if (_noTurningInputLastFrame || Time.time > _lastTurnTime + _turningCooldown / _turningMultiplier)
            {
                if (turning.x > 0)
                {
                    _lastTurnTime = Time.time;
                    TurnRight();
                }
                else if (turning.x < 0)
                {
                    _lastTurnTime = Time.time;
                    TurnLeft();
                }
            }
        }

        //Can be triggered by the joystick _turnAction or the SteamVR_Behaviour_Boolean TurnLeft
        if (_continuousTurningLeft)
        {
            if (turning.x < 0) // true: triggered by joystick. false: triggered by (virtual) button
            {
                _player.transform.RotateAround(_vrCamera.position, Vector3.up, _turningMultiplier * _continuousTurningSpeed * turning.x);
            }
            else
            {
                _player.transform.RotateAround(_vrCamera.position, Vector3.up, _turningMultiplier * -_continuousTurningSpeed);
            }
        }

        //Can be triggered by the joystick _turnAction or the SteamVR_Behaviour_Boolean TurnRight
        if (_continuousTurningRight)
        {
            if (turning.x > 0) // true: triggered by joystick. false: triggered by (virtual) button
            {
                _player.transform.RotateAround(_vrCamera.position, Vector3.up, _turningMultiplier * _continuousTurningSpeed * turning.x);
            }
            else
            {
                _player.transform.RotateAround(_vrCamera.position, Vector3.up, _turningMultiplier * _continuousTurningSpeed);
            }
        }

        _noTurningInputLastFrame = (turning.magnitude == 0);
    }

    //Turns the VR player root object counter clockwise
    //Can be called by SteamVR_Behaviour_Boolean or through joystick input if _turningStep != 0
    public void TurnLeft()
    {
        if (_turningStep != 0)
        {
            _player.transform.RotateAround(_vrCamera.position, Vector3.up, -_turningStep);
        }
        else
        {
            _continuousTurningLeft = true;
        }
    }

    //Turns the VR player root object clockwise
    //Can be called by SteamVR_Behaviour_Boolean or through joystick input if _turningStep != 0
    public void TurnRight()
    {
        if (_turningStep != 0)
        {
            _player.transform.RotateAround(_vrCamera.position, Vector3.up, _turningStep);
        }
        else
        {
            _continuousTurningRight = true;
        }
    }

    //Called by SteamVR_Behaviour_Boolean script On Press Up
    public void StopTurning()
    {
        _continuousTurningLeft = false;
        _continuousTurningRight = false;
    }

    //Turns the VR player root object 180 degree
    //Can be called by SteamVR_Behaviour_Boolean
    public void TurnAround()
    {
        _player.transform.RotateAround(_vrCamera.position, Vector3.up, 180f);
    }

    private void UpdateVignette()
    {
        if ((_movementEnabled && _vignetteDuringMovement && _movementAxis.magnitude > 0) || (_turningEnabled && _vignetteDuringTurning && _turningAxis.magnitude > 0))
        {
            _vignetteController.AddTrigger(this);
        }
        else
        {
            _vignetteController.RemoveTrigger(this);
        }
    }

    public void EnableMovement(bool enable)
    {
        _movementEnabled = enable;
    }

    public void EnableTurning(bool enable)
    {
        _turningEnabled = enable;
    }
}
