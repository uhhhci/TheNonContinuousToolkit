using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour
{
    protected Animator animator;

    private bool _ikEnabled = true;
    public bool ikActive = true;
    public bool handIK = true;
    public bool headIK = true;

    public Transform _headTarget = null;
    public Transform _leftHandTarget = null;
    public Transform _rightHandTarget = null;
    public float _footOffset = 0.1f;

    private float _ikWeight = 1f;
    private float _ikWeightChangeFactor = 0.1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (ikActive && _ikEnabled)
        {
            _ikWeight += _ikWeightChangeFactor;
        }
        else
        {
            _ikWeight -= _ikWeightChangeFactor;

        }
        _ikWeight = Mathf.Clamp01(_ikWeight);
    }

    public void SetIKWeightChangeFactor(float factor)
    {
        _ikWeightChangeFactor = factor;
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            //if (ikActive)
            //{

            if (headIK)
            {
                animator.SetLookAtWeight(_ikWeight);
                animator.SetLookAtPosition(_headTarget.position + _headTarget.forward);
            }
            else
            {
                animator.SetLookAtWeight(0);
            }

            if (handIK)
            {


                // Set the right hand target position and rotation, if one has been assigned
                if (_rightHandTarget != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _ikWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _ikWeight);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTarget.rotation);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (_leftHandTarget != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _ikWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _ikWeight);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTarget.rotation);
                }

                //}
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            //else
            //{
            //    animator.SetLookAtWeight(0);
            //}
        }
    }

    public void OverrideIKWeight(float weight)
    {
        _ikWeight = weight;
    }

    public void EnableIK(bool enable)
    {
        _ikEnabled = enable;
    }
}
