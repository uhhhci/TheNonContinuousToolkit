using UnityEngine;

/*
 * PushPull Movement by Jann Philipp Freiwald, 2020
 */
public class PushPullMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OVRInput.Touch _touch = OVRInput.Touch.PrimaryThumbstick;
    [SerializeField] private GameObject _player; //VR player root object
    [SerializeField] private Transform _vrCamera; //VR Camera within VR player
    [SerializeField] private Transform _movementOriginPoint; //Child of VR player root object
    [SerializeField] private Transform _movementCurrentPoint; //The hand this script is attached to
    [SerializeField] private VignetteController _vignetteController;

    [Header("Parameters")]
    public bool _invertDirection = false; //Should we invert the direction the player is moving
    public float _movementStep = 0.5f; //step size for one teleport movement. If set to zero will use continuous movement instead
    public float _multiplier = 2.0f; //The angle multiplier. output angle = input angle * multiplier  
    public bool _vignetteDuringMovement = true;
    [SerializeField] private bool _useDynamicHeightMultiplier = true;
    [SerializeField] private float _maxHeightFactor = 3f;


    private Vector3 _totalMovement = Vector3.zero; //The total distance the player has input since the beginning of the movement. Used only when _movementStep != 0
    private bool _moving; //Is the player currently moving

    private bool _enabled = true;

    //FixedUpdate is called 50 times per second
    void FixedUpdate()
    {
        if (_moving)
        {
            CalculateMovement();
        }
    }

    private void Update()
    {
        if (!_enabled) return;

        if (OVRInput.GetDown(_touch))
        {
            StartMovement();
        }
        if (OVRInput.GetUp(_touch))
        {
            EndMovement();
        }
    }

    //Called by SteamVR_Behaviour_Boolean script On Press Down
    public void StartMovement()
    {
        _movementOriginPoint.position = _movementCurrentPoint.position;
        _moving = true;
        _totalMovement = Vector3.zero;

        if (_vignetteDuringMovement)
            _vignetteController.AddTrigger(this);
    }

    //Called by SteamVR_Behaviour_Boolean script On Press Up
    public void EndMovement()
    {
        _moving = false;

        _vignetteController.RemoveTrigger(this);
    }

    //Performs the player movement based on the positional offset between the last sampled hand position and the current hand position
    private void CalculateMovement()
    {
        Vector3 currentMovementLocal = _movementCurrentPoint.position;
        currentMovementLocal.y = 0; //This constraints the movement to the XY-plane
        Vector3 originMovementLocal = _movementOriginPoint.position;
        originMovementLocal.y = 0; //This constraints the movement to the XY-plane



        Vector3 movement = currentMovementLocal - originMovementLocal;
        movement *= _multiplier;

        if (_useDynamicHeightMultiplier)
        {
            float heightDifference = _vrCamera.position.y - _movementCurrentPoint.position.y;
            float heightFactor = 1f;

            if (heightDifference < 0.35f)
            {
                heightFactor = 1f;
            }
            else if (heightDifference > 0.35f && heightDifference < 2f)
            {
                heightFactor = remap(heightDifference, 0.35f, 2f, 1f, _maxHeightFactor);
            }
            else
            {
                heightFactor = _maxHeightFactor;
            }
            movement *= heightFactor;
        }


        if (_movementStep == 0f)
        {
            //Smooth movement
            movement *= _invertDirection ? 1 : -1;
            Move(movement);
        }
        else
        {
            //Stutter movement
            _totalMovement += movement;

            if (Mathf.Abs(_totalMovement.magnitude) > _movementStep)
            {
                movement = _movementStep * _totalMovement.normalized;
                movement *= _invertDirection ? 1 : -1;
                Move(movement);

                _totalMovement = Vector3.zero;
            }
        }

        _movementOriginPoint.position = _movementCurrentPoint.position;
    }

    //Moves the VR player root object
    public void Move(Vector3 offset)
    {
        _player.transform.position += offset;
    }

    public void EnableMovement(bool enable)
    {
        _enabled = enable;
        if (!enable)
            EndMovement();
    }

    private float remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
