using UnityStandardAssets.Characters.ThirdPerson;
using StandardAssets.Characters.ThirdPerson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StandardAssets.Characters.Physics;

public class AgentAvatarController : MonoBehaviour
{
    [Header("External References, see tooltips")]
    [Tooltip("The root object of the XR player setup")]
    [SerializeField] protected Transform _playerController;
    [Tooltip("Object in XR player setup that holds the virtual camera. Oculus: CenterEyeAnchor")]
    [SerializeField] protected Transform _vrCamera;
    [Tooltip("Object in XR player setup that represents the left hand. Oculus: LeftHandAnchor")]
    [SerializeField] protected Transform _vrHandLeft;
    [Tooltip("Object in XR player setup that represents the right hand. Oculus: RightHandAnchor")]
    [SerializeField] protected Transform _vrHandRight;
    [Header("Internal References")]
    [SerializeField] protected ThirdPersonCharacter _character;
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected IKControl _ikControl;
    [Header("Strafe Values")]
    [SerializeField] protected float _strafeSpeed = 5f;
    [SerializeField] protected float _strafeAngularSpeed = 120f;
    [SerializeField] protected float _strafeAccelleration = 100f;
    [SerializeField] protected float _strafeIKWeightFactor = 0.1f;
    [SerializeField] protected float _IKdistance = 0.2f;

    protected bool _setTarget;
    protected bool _moving;


    private void OnEnable()
    {
        ResetPosition();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _agent.updateRotation = false;
        transform.SetParent(null);
    }

    protected virtual void FixedUpdate()
    {
        _setTarget = true;

        _agent.speed = _strafeSpeed;
        _agent.angularSpeed = _strafeAngularSpeed;
        _agent.acceleration = _strafeAccelleration;
        _ikControl.SetIKWeightChangeFactor(_strafeIKWeightFactor);

    }

    public virtual void StartMovement()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_vrCamera.position);
        _moving = true;
    }

    public void StopMovement()
    {
        RotateBody();

        _moving = false;
        _agent.isStopped = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_setTarget)
        {
            _setTarget = false;
            StartMovement();
        }

        _ikControl.ikActive = _agent.remainingDistance <= _IKdistance;

        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            _character.Move(Vector3.zero, false, false, false, 1f);
            StopMovement();
        }
        else
        {
            _character.Move(_agent.desiredVelocity, false, false, true, 1f);
            RotateBody();
        }
    }

    public void ResetPosition()
    {
        Vector3 pos = _vrCamera.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, 5, 1, QueryTriggerInteraction.Ignore))
        {
            pos = hit.point;
        }
        transform.position = pos;
    }

    protected void RotateBody()
    {
        Vector3 leftHandPos = _vrHandLeft.position - _vrCamera.position;
        Vector3 rightHandPos = _vrHandRight.position - _vrCamera.position;
        Vector3 centerHandPos = (leftHandPos + rightHandPos) / 2f;
        centerHandPos.y = 0f;

        Vector3 camForward = _vrCamera.forward;
        camForward.y = 0;

        Vector3 bodyForward = (camForward + centerHandPos) / 2f;

        Vector3 thisForward = transform.forward;
        thisForward.y = 0;
        float angle = Vector3.SignedAngle(thisForward, bodyForward, Vector3.up);
        transform.Rotate(0, Mathf.Lerp(0, angle, Time.deltaTime * 10f), 0);
        _character.Turn(angle);
    }

}
