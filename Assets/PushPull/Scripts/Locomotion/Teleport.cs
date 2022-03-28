using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _playerController;
    [SerializeField] private GameObject _vrCamera;
    [SerializeField] private TeleportArc _arc;
    [SerializeField] private OVRInput.Button _button = OVRInput.Button.PrimaryThumbstick;

    private bool _showArc = false;
    private Vector3 _hitPosition = Vector3.zero;
    RaycastHit _hit;
    [SerializeField] private GameEvent EventTeleportFinished;

    private bool _enabled = true;

    // Update is called once per frame
    void Update()
    {
        if (!_enabled) { return; }

        if (OVRInput.GetDown(_button))
        {
            _showArc = true;
            PushPullBrain._instance.ChangeActiveStatusMovement(PushPullBrain.LocomotionMovement.Teleport, true);
        }
        if (OVRInput.GetUp(_button))
        {
            TriggerTeleport();
            PushPullBrain._instance.ChangeActiveStatusMovement(PushPullBrain.LocomotionMovement.Teleport, false);
        }

        if (_showArc)
        {
            _arc.SetArcData(transform.position, transform.forward * 10f, true, false);

            if (_arc.DrawArc(out _hit))
            {
                _hitPosition = _hit.point;
            }
        }
    }

    public void TriggerTeleport()
    {
        _arc.SetArcData(Vector3.zero, Vector3.zero, false, false);
        _arc.DrawArc(out _hit);
        _arc.Hide();
        _showArc = false;
        _playerController.transform.position = _hitPosition;

        EventTeleportFinished.Raise();
    }

    public void EnableMovement(bool enable)
    {
        _enabled = enable;
        if (!enable)
        {
            _arc.SetArcData(Vector3.zero, Vector3.zero, false, false);
            _arc.DrawArc(out _hit);
            _arc.Hide();
            _showArc = false;
        }
    }
}
