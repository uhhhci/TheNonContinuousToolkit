using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRemoteControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _movementTarget;
    [SerializeField] private Transform _vrCamera;
    [SerializeField] private TeleportArc _arc;
    [SerializeField] private OVRInput.Button _sendButton = OVRInput.Button.Three;
    [SerializeField] private OVRInput.Button _recallButton = OVRInput.Button.Four;
    [SerializeField] private OVRInput.Button _climbButton = OVRInput.Button.Two;
    [SerializeField] private OVRInput.Button _leapButton = OVRInput.Button.One;
    [SerializeField] private OVRInput.Button _actionButton = OVRInput.Button.None;
    [SerializeField] private bool _buttonActionsEnabled = false;
    [SerializeField] private Color _arcColor = Color.yellow;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _groundSlamEffect;
    [SerializeField] private RemoteIKControl _ikControl;

    [SerializeField] private GameObject _SwordModel;
    [SerializeField] private GameObject _AvatarSwordModel;
    [SerializeField] private GameObject _HandModelLeft;
    [SerializeField] private GameObject _ControllerModelLeft;
    [SerializeField] private GameObject _HandModelRight;
    [SerializeField] private GameObject _ControllerModelRight;

    public static AvatarRemoteControl _instance;

    public bool _hideHandsDuringGestureActions = true;
    private bool _showArc = false;
    private Vector3 _hitPosition = Vector3.zero;
    RaycastHit _hit;

    private bool _topZoneTriggeredLeft = false;
    private bool _topZoneTriggeredRight = false;
    private bool _bottomZoneTriggeredLeft = false;
    private bool _bottomZoneTriggeredRight = false;
    private bool _canTriggerAction = true;

    public bool _remoteControlsEnabled = true;
    public bool _gestureActionsEnabled = true;

    private bool _remoteAvatarSentOut = false;


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


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerAction();
        }

        if (_remoteControlsEnabled)
        {
            if (OVRInput.GetDown(_sendButton))
            {
                _showArc = true;
            }
            if (OVRInput.GetUp(_sendButton))
            {
                SendAvatar();
            }

            if (OVRInput.GetDown(_recallButton))
            {
                RecallAvatar();
            }

            if (OVRInput.GetDown(_climbButton) || Input.GetKeyDown(KeyCode.Return))
            {
                TriggerClimbAction();
            }

            if (OVRInput.GetDown(_leapButton) || Input.GetKeyDown(KeyCode.Backspace))
            {
                TriggerLeapAction();
            }

            if (OVRInput.GetDown(_actionButton))
            {
                TriggerAction();
            }
        }

        if (_showArc)
        {
            _arc.SetArcData(transform.position, transform.forward * 10f, true, false);
            _arc.SetColor(_arcColor);

            if (_arc.DrawArc(out _hit))
            {
                _hitPosition = _hit.point;
            }
        }
    }

    public void SendAvatar()
    {
        _remoteAvatarSentOut = true;

        _arc.SetArcData(Vector3.zero, Vector3.zero, false, false);
        _arc.DrawArc(out _hit);
        _arc.Hide();
        _showArc = false;
        _movementTarget.SetParent(null);
        _movementTarget.position = _hitPosition;
        _ikControl.setIKControlTargets(RemoteIKControl.IKTargetMode.Remote);
        GiveSwordToRemoteAvatar(true);
        //Invoke("TriggerAction", 2f);
        //Invoke("RecallAvatar", 4f);
    }

    public void RecallAvatar()
    {
        _remoteAvatarSentOut = false;

        _movementTarget.SetParent(_vrCamera);
        _movementTarget.localPosition = Vector3.zero;
        _ikControl.setIKControlTargets(RemoteIKControl.IKTargetMode.Local);
        GiveSwordToRemoteAvatar(false);
    }

    private void TriggerAction()
    {
        _canTriggerAction = false;
        _animator.Play("SwordAttack", 0);
        Invoke("GroundSlam", 1.3f);
        //disable IK for time of animation
        _ikControl.ikActive = false;
        _ikControl.handIK = false;
        _ikControl.headIK = false;
        _ikControl.OverrideIKWeight(0);
        GiveSwordToRemoteAvatar(true);

        Invoke("EnableIK", 2f);
        Invoke("ShowHands", 2.5f);

        if (_hideHandsDuringGestureActions)
        {
            _HandModelLeft.SetActive(false);
            _HandModelRight.SetActive(false);
            _ControllerModelLeft.SetActive(false);
            _ControllerModelRight.SetActive(false);
        }
    }

    private void TriggerClimbAction()
    {
        if (!_buttonActionsEnabled) return;

        _canTriggerAction = false;
        _animator.Play("Climb", 0);
        //disable IK for time of animation
        _ikControl.ikActive = false;
        _ikControl.handIK = false;
        _ikControl.headIK = false;
        _ikControl.OverrideIKWeight(0);
        Invoke("EnableIK", 2f);

        if (_AvatarSwordModel) _AvatarSwordModel.SetActive(false);
        if (_SwordModel) _SwordModel.SetActive(false);
        _HandModelLeft.SetActive(false);
        _HandModelRight.SetActive(false);
        _ControllerModelLeft.SetActive(false);
        _ControllerModelRight.SetActive(false);
        Invoke("ShowHands", 2.5f);
    }

    private void TriggerLeapAction()
    {
        if (!_buttonActionsEnabled) return;

        _canTriggerAction = false;
        _animator.Play("Leap", 0);
        //disable IK for time of animation
        _ikControl.ikActive = false;
        _ikControl.handIK = false;
        _ikControl.headIK = false;
        _ikControl.OverrideIKWeight(0);
        Invoke("EnableIK", 2f);

        if (_AvatarSwordModel) _AvatarSwordModel.SetActive(false);
        if (_SwordModel) _SwordModel.SetActive(false);
        _HandModelLeft.SetActive(false);
        _HandModelRight.SetActive(false);
        _ControllerModelLeft.SetActive(false);
        _ControllerModelRight.SetActive(false);
        Invoke("ShowHands", 2.5f);
    }

    private void GroundSlam()
    {
        _groundSlamEffect.Play();
        Collider[] cols = Physics.OverlapSphere(_groundSlamEffect.transform.position, 7.5f);

        foreach (Collider col in cols)
        {
            Sliceable sliceable = col.GetComponent<Sliceable>();
            if (sliceable)
            {
                sliceable.Slice(sliceable.transform);
            }
        }

        //collect anew, slicing produced more objects
        cols = Physics.OverlapSphere(_groundSlamEffect.transform.position, 7.5f);

        foreach (Collider col in cols)
        {
            Sliceable sliceable = col.GetComponent<Sliceable>();
            if (sliceable && sliceable.GetComponent<Rigidbody>())
            {
                sliceable.GetComponent<Rigidbody>().AddForce((sliceable.transform.position - _groundSlamEffect.transform.position).normalized * 3.5f, ForceMode.Impulse);
            }
        }
    }

    private void GiveSwordToRemoteAvatar(bool giveSword)
    {
        if (_AvatarSwordModel) _AvatarSwordModel.SetActive(giveSword);
        if (_SwordModel) _SwordModel.SetActive(!giveSword);
    }

    private void EnableIK()
    {
        _ikControl.ikActive = true;
        _ikControl.handIK = true;
        _ikControl.headIK = true;
        _canTriggerAction = true;
    }

    private void ShowHands()
    {
        if (!_remoteAvatarSentOut)
        {
            GiveSwordToRemoteAvatar(false);
        }
        _HandModelLeft.SetActive(true);
        _HandModelRight.SetActive(true);
        _ControllerModelLeft.SetActive(true);
        _ControllerModelRight.SetActive(true);
    }

    public void TopZoneTriggeredLeft()
    {
        if (!_gestureActionsEnabled) return;

        if (!_canTriggerAction)
        {
            ResetTriggers();
        }
        else
        {
            _topZoneTriggeredLeft = true;
            _bottomZoneTriggeredLeft = false;
            _bottomZoneTriggeredRight = false;
        }
    }
    public void TopZoneTriggeredRight()
    {
        if (!_gestureActionsEnabled) return;

        if (!_canTriggerAction)
        {
            ResetTriggers();
        }
        else
        {
            _topZoneTriggeredRight = true;
            _bottomZoneTriggeredLeft = false;
            _bottomZoneTriggeredRight = false;
        }
    }

    public void BottomZoneTriggeredLeft()
    {
        if (!_gestureActionsEnabled) return;

        if (!_canTriggerAction)
        {
            ResetTriggers();
        }
        else
        {
            if (_topZoneTriggeredLeft)
            {
                _topZoneTriggeredLeft = false;
                _bottomZoneTriggeredLeft = true;
            }
        }

        if (CheckTriggerStatus())
        {
            TriggerAction();
            ResetTriggers();
        }
    }

    public void BottomZoneTriggeredRight()
    {
        if (!_gestureActionsEnabled) return;

        if (!_canTriggerAction)
        {
            ResetTriggers();
        }
        else
        {
            if (_topZoneTriggeredRight)
            {
                _topZoneTriggeredRight = false;
                _bottomZoneTriggeredRight = true;
            }
        }

        if (CheckTriggerStatus())
        {
            TriggerAction();
            ResetTriggers();
        }
    }

    private void ResetTriggers()
    {
        _topZoneTriggeredLeft = false;
        _topZoneTriggeredRight = false;
        _bottomZoneTriggeredLeft = false;
        _bottomZoneTriggeredRight = false;
    }

    private bool CheckTriggerStatus()
    {
        return _bottomZoneTriggeredLeft && _bottomZoneTriggeredRight;
    }
}
