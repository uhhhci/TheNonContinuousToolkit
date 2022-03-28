using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControlledSmartAvatar : SmartAvatarController
{
    public Transform _movementTarget;

    public override void StartMovement()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_movementTarget.position);
        _moving = true;
    }
}
