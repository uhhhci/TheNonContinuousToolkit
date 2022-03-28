using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sigtrap.VrTunnellingPro;

public class VignetteController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OVRVignette _OVR_vignette;
    [SerializeField] private TunnellingMobile _tunneling_vignette;

    private float _currentVignetteValue = 0;
    private float _targetVignetteValue = 0;
    private float _step = 0.1f; // 50 fixedUpdates per second
    private float _vignetteFOV;
    private List<MonoBehaviour> _activeTriggers = new List<MonoBehaviour>();

    private void Start()
    {
        //_OVR_vignette.enabled = true;
        _vignetteFOV = _OVR_vignette.VignetteFieldOfView;
    }

    public void AddTrigger(MonoBehaviour trigger)
    {
        if (!_activeTriggers.Contains(trigger))
        {
            _activeTriggers.Add(trigger);
            UpdateVignette();
        }
    }

    public void RemoveTrigger(MonoBehaviour trigger)
    {
        if (_activeTriggers.Contains(trigger))
        {
            _activeTriggers.Remove(trigger);
            UpdateVignette();
        }
    }

    private void UpdateVignette()
    {
        if (_activeTriggers.Count > 0)
        {
            //_vignette.enabled = true;
            _targetVignetteValue = 1f;
        }
        else
        {
            //_vignette.enabled = false;
            _targetVignetteValue = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (_currentVignetteValue < _targetVignetteValue)
        {
            _currentVignetteValue += _step;
        }
        else if (_currentVignetteValue > _targetVignetteValue)
        {
            _currentVignetteValue -= _step;
        }

        _currentVignetteValue = Mathf.Clamp01(_currentVignetteValue);
        _tunneling_vignette._debugForceValue = _currentVignetteValue;

        _OVR_vignette.VignetteFieldOfView = 2 * _vignetteFOV - (_currentVignetteValue * _vignetteFOV);


    }
}
