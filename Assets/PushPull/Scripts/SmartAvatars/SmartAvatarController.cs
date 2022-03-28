using UnityStandardAssets.Characters.ThirdPerson;
using StandardAssets.Characters.ThirdPerson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StandardAssets.Characters.Physics;

public class SmartAvatarController : AgentAvatarController
{
    [Header("Sprint Values")]
    [SerializeField] private float _sprintSpeed = 5f;
    [SerializeField] private float _sprintAngularSpeed = 120f;
    [SerializeField] private float _sprintAccelleration = 100f;
    [SerializeField] private float _sprintIKWeightFactor = 0.1f;

    [HideInInspector] public bool _isStrafing;
    [SerializeField] private float _maximumStrafeDistance = 2f;


    override protected void FixedUpdate()
    {
        _setTarget = true;
        _isStrafing = (Vector3.Distance(transform.position, _vrCamera.position) < _maximumStrafeDistance);

        if (_isStrafing)
        {
            _agent.speed = _strafeSpeed;
            _agent.angularSpeed = _strafeAngularSpeed;
            _agent.acceleration = _strafeAccelleration;
            _ikControl.SetIKWeightChangeFactor(_strafeIKWeightFactor);
        }
        else
        {
            _agent.speed = _sprintSpeed;
            _agent.angularSpeed = _sprintAngularSpeed;
            _agent.acceleration = _sprintAccelleration;
            _ikControl.SetIKWeightChangeFactor(_sprintIKWeightFactor);
        }
    }

    // Update is called once per frame
    override protected void Update()
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
            _character.Move(_agent.desiredVelocity, false, false, _isStrafing, _isStrafing ? 1 : _sprintSpeed / _strafeSpeed);
            if (_isStrafing)
            {
                RotateBody();
            }
        }
    }
}
