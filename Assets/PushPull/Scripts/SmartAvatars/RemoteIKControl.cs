using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteIKControl : IKControl
{
    public enum IKTargetMode { Local, Remote };
    public Transform _localHeadTarget = null;
    public Transform _localLeftHandTarget = null;
    public Transform _localRightHandTarget = null;

    public Transform _remoteHeadTarget = null;
    public Transform _remoteLeftHandTarget = null;
    public Transform _remoteRightHandTarget = null;

    public void setIKControlTargets(IKTargetMode targetMode)
    {
        if (targetMode == IKTargetMode.Local)
        {
            _headTarget = _localHeadTarget;
            _leftHandTarget = _localLeftHandTarget;
            _rightHandTarget = _localRightHandTarget;
        }
        else
        {
            _headTarget = _remoteHeadTarget;
            _leftHandTarget = _remoteLeftHandTarget;
            _rightHandTarget = _remoteRightHandTarget;
        }
    }
}
