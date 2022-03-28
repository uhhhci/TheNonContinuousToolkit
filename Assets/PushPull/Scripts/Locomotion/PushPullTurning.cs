using UnityEngine;

/*
 * PushPull Turning by Jann Philipp Freiwald, 2020
 */
public class PushPullTurning : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OVRInput.Touch _touch = OVRInput.Touch.SecondaryThumbstick;
    [SerializeField] private OVRInput.Button _turn = OVRInput.Button.SecondaryThumbstick;
    [SerializeField] private GameObject _player; //VR player root object
    [SerializeField] private GameObject _vrCamera; //VR Camera within VR player
    [SerializeField] private Transform _rotationOriginPoint; //Child of VR player root object
    [SerializeField] private Transform _rotationCurrentPoint; //The hand this script is attached to
    [SerializeField] private VignetteController _vignetteController;

    [Header("Parameters")]
    public bool _invertDirection = true; //Should we invert the direction the player is turning
    public float _turningStep = 10f; //step size for one turn. If set to zero will use continuous turning instead
    public float _multiplier = 1.0f; //The angle multiplier. output angle = input angle * multiplier    
    public bool _vignetteDuringTurning;


    private bool _turning; //Is the player currently turning
    private float _totalAngle = 0f; //The total angle the player has input since the beginning of the turn. Used only when _turningStep != 0

    private bool _enabled = true;

    //FixedUpdate is called 50 times per second
    void FixedUpdate()
    {
        if (_turning)
        {
            CalculateRotation();
        }
    }

    private void Update()
    {
        if (!_enabled) return;

        if (OVRInput.GetDown(_touch))
        {
            StartRotation();
        }
        if (OVRInput.GetUp(_touch))
        {
            EndRotation();
        }

        if (OVRInput.GetDown(_turn))
        {
            Turn(180f);
        }
    }

    //Called by SteamVR_Behaviour_Boolean script On Press Down
    public void StartRotation()
    {
        _rotationOriginPoint.position = _rotationCurrentPoint.position;
        _turning = true;
        _totalAngle = 0f;


        if (_vignetteDuringTurning)
            _vignetteController.AddTrigger(this);
    }

    //Called by SteamVR_Behaviour_Boolean script On Press Up
    public void EndRotation()
    {
        _turning = false;

        _vignetteController.RemoveTrigger(this);
    }

    //Performs the player rotation based on the angle between the last sampled hand position and the current hand position
    private void CalculateRotation()
    {
        Vector3 currentDirectionLocal = _rotationCurrentPoint.position - _vrCamera.transform.position;
        currentDirectionLocal.y = 0; //This constraints the rotation to the Z-axis
        Vector3 originDirectionLocal = _rotationOriginPoint.position - _vrCamera.transform.position;
        originDirectionLocal.y = 0; //This constraints the rotation to the Z-axis

        float angle = Vector3.SignedAngle(originDirectionLocal, currentDirectionLocal, Vector3.up);
        angle *= _multiplier;

        if (_turningStep == 0f)
        {
            angle *= _invertDirection ? 1 : -1;
            Turn(angle);
        }
        else
        {
            _totalAngle += angle;

            if (Mathf.Abs(_totalAngle) > _turningStep)
            {
                angle = _turningStep * Mathf.Sign(_totalAngle);
                angle *= _invertDirection ? 1 : -1;
                Turn(angle);

                _totalAngle = 0f;
            }
        }

        _rotationOriginPoint.position = _rotationCurrentPoint.position;
    }

    //Rotates the VR player root object on the Z-axis
    public void Turn(float angle)
    {
        _player.transform.RotateAround(_vrCamera.transform.position, Vector3.up, angle);
    }

    public void EnableTurning(bool enable)
    {
        _enabled = enable;
        if (!enable)
            EndRotation();
    }
}
