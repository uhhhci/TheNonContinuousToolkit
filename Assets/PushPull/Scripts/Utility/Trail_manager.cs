using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail_manager : MonoBehaviour
{

    public SmartAvatarController _avatarController;

    [Header("target SkinnedMeshRenderer")]
    public GameObject game_obj_target;

    private Vector3 v3_position_game_obj_target_before;

    [Header("how many trails")]
    public int trail_count;

    [Header("The initial transparency of the tail")]
    [Range(0f, 1f)]
    public float trail_alpha;

    [Header("The speed at which each tail disappears")]
    public float trail_disappear_speed;

    public float _minDistance = 1f;
    public float _delayFactor = 5f;

    private List<Trail_control> _trails = new List<Trail_control>();

    public Material _material;

    public Transform _target;
    public Transform _origin;

    private bool _active = true;


    void Start()
    {
        transform.SetParent(null);
        if (this.trail_count > 0 && this.game_obj_target != null)
        {
            for (int i = 0; i < this.trail_count; i++)
            {
                MakeTrail();
            }

        }

    }

    private Trail_control MakeTrail()
    {
        GameObject trail = new GameObject("trail");
        trail.transform.SetParent(null);
        trail.AddComponent<MeshFilter>();
        trail.AddComponent<MeshRenderer>();
        trail.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trail.layer = gameObject.layer;

        trail.GetComponent<MeshRenderer>().material = new Material(_material);
        Trail_control trail_control = trail.AddComponent<Trail_control>();
        _trails.Add(trail_control);
        trail.SetActive(false);

        return trail_control;
    }

    private Trail_control GetFreeTrail()
    {
        for (int i = 0; i < _trails.Count; i++)
        {
            if (!_trails[i].gameObject.activeInHierarchy)
            {
                return _trails[i];
            }
        }
        return MakeTrail();
    }

    public void SetTrailActive(bool active)
    {
        _active = active;
    }

    private void Update()
    {
        if (!_active)
        {
            transform.position = game_obj_target.transform.position;
            return;
        }

        if (Vector3.Distance(transform.position, game_obj_target.transform.position) > _minDistance)
        {
            transform.position = game_obj_target.transform.position;

            Trail_control trail = GetFreeTrail();
            trail.gameObject.SetActive(true);
            trail.transform.position = game_obj_target.transform.position;
            trail.transform.rotation = game_obj_target.transform.rotation;

            float delay = 1f;
            if (_target && _origin) delay = (1 - (Vector3.Distance(transform.position, _target.position) / (Mathf.Max(Vector3.Distance(_origin.position, _target.position), 0.01f)))) * Vector3.Distance(_origin.position, _target.position) / _delayFactor;
            trail.init(this.trail_disappear_speed, this.game_obj_target.GetComponent<SkinnedMeshRenderer>(), this.trail_alpha, delay);
        }
    }
}

