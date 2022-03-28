using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingImitator : MonoBehaviour
{
    public Transform _originalVRCamera;
    public Transform _originalVRHandLeft;
    public Transform _originalVRHandRight;
    public Transform _imitatorVRCamera;
    public Transform _imitatorVRHandLeft;
    public Transform _imitatorVRHandRight;
    public Transform _avatarTargetMarker;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(_originalVRCamera.position, Vector3.down, out hit, 5f, 1 << LayerMask.NameToLayer("Ground")))
        {
            Vector3 groundPos = hit.point;
            Vector3 groundToHMD = _originalVRCamera.position - groundPos;

            _imitatorVRCamera.position = _avatarTargetMarker.position + groundToHMD;
            _imitatorVRCamera.rotation = _originalVRCamera.rotation;

            _imitatorVRHandLeft.position = _imitatorVRCamera.position + (_originalVRHandLeft.position - _originalVRCamera.position);
            _imitatorVRHandLeft.rotation = _originalVRHandLeft.rotation;
            _imitatorVRHandLeft.localRotation *= Quaternion.Euler(0, 0, 90);

            _imitatorVRHandRight.position = _imitatorVRCamera.position + (_originalVRHandRight.position - _originalVRCamera.position);
            _imitatorVRHandRight.rotation = _originalVRHandRight.rotation;
            _imitatorVRHandRight.localRotation *= Quaternion.Euler(0, 0, -90);
        }

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(_originalVRCamera.position, 0.25f);
    //    Gizmos.DrawSphere(_originalVRHandLeft.position, 0.25f);
    //    Gizmos.DrawSphere(_originalVRHandRight.position, 0.25f);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(_imitatorVRCamera.position, 0.25f);
    //    Gizmos.DrawSphere(_imitatorVRHandLeft.position, 0.25f);
    //    Gizmos.DrawSphere(_imitatorVRHandRight.position, 0.25f);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(_avatarTargetMarker.position, 0.25f);

    //}
}
