using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sigtrap.VrTunnellingPro;

public class DissolveController : MonoBehaviour
{
    private float _currentDissolveValue = 0;
    private float _targetDissolveValue = 0;
    private float _step = 0.02f; // 50 fixedUpdates per second
    [SerializeField] private SkinnedMeshRenderer _dissolveMeshRenderer;
    [SerializeField] private MeshRenderer _dissolveAfterimageMeshRenderer;
    [SerializeField] private ParticleSystem _dissolveAfterimageParticles;
    private Material _dissolveMaterial;
    private Material _dissolveAfterimageMaterial;
    [SerializeField] private Vector3 _targetOffset = new Vector3(0, 0f, 0);



    private void Awake()
    {
        _dissolveMaterial = _dissolveMeshRenderer.material;
        _dissolveAfterimageMaterial = _dissolveAfterimageMeshRenderer.material;
    }

    private void Start()
    {
        _dissolveAfterimageMeshRenderer.transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        if (_currentDissolveValue < _targetDissolveValue)
        {
            _currentDissolveValue += _step;
        }
        else if (_currentDissolveValue > _targetDissolveValue)
        {
            _currentDissolveValue -= _step;
        }

        _currentDissolveValue = Mathf.Clamp01(_currentDissolveValue);
        _dissolveMaterial.SetFloat("_Dissolve", _currentDissolveValue);
        _dissolveAfterimageMaterial.SetFloat("_Dissolve", 1 - _currentDissolveValue);
    }

    public void PlayDissolve()
    {
        OverrideDissolveState(1f);
        SetDissolveTarget(0f);
        UpdateDissolveAfterimage();
    }

    private void OverrideDissolveState(float target)
    {
        _targetDissolveValue = target;
        _currentDissolveValue = target;
    }

    private void SetDissolveTarget(float target)
    {
        _targetDissolveValue = target;
    }

    private void UpdateDissolveAfterimage()
    {
        _dissolveAfterimageMeshRenderer.transform.position = _dissolveMeshRenderer.transform.position;
        _dissolveAfterimageMeshRenderer.transform.rotation = _dissolveMeshRenderer.transform.rotation;

        Mesh mesh = new Mesh();
        _dissolveMeshRenderer.BakeMesh(mesh);
        _dissolveAfterimageMeshRenderer.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void PlayDissolveParticles()
    {
        if (!gameObject.activeInHierarchy) { return; }

        Vector3 direction = (transform.position + _targetOffset) - _dissolveAfterimageMeshRenderer.transform.position;

        var velocity = _dissolveAfterimageParticles.velocityOverLifetime;

        AnimationCurve curveX = new AnimationCurve();
        curveX.AddKey(0.0f, direction.x * 4f);
        velocity.x = new ParticleSystem.MinMaxCurve(1.0f, curveX);
        velocity.xMultiplier = 100f;

        AnimationCurve curveY = new AnimationCurve();
        curveY.AddKey(0.0f, direction.y * 4f);
        velocity.y = new ParticleSystem.MinMaxCurve(1.0f, curveY);
        velocity.yMultiplier = 100f;

        AnimationCurve curveZ = new AnimationCurve();
        curveZ.AddKey(0.0f, direction.z * 4f);
        velocity.z = new ParticleSystem.MinMaxCurve(1.0f, curveZ);
        velocity.zMultiplier = 100f;

        velocity.speedModifierMultiplier = 0.01f;
        _dissolveAfterimageParticles.Play();
    }
}
