
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleLaserpointer : MonoBehaviour
{
    public OVRInput.Button _trigger = OVRInput.Button.PrimaryIndexTrigger;
    public OVRInput.Button _disableButton = OVRInput.Button.PrimaryThumbstick;
    private float _laserLengthmax = 15f;
    Button _buttonHit = null;

    [SerializeField] private Color _hitColor = Color.green;
    [SerializeField] private Color _defaultColor = Color.red;
    [SerializeField] private LineRenderer _lineRenderer;
    private Material _laserMaterial;

    private bool _enabled = true;


    // Start is called before the first frame update
    void Start()
    {
        _laserMaterial = _lineRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(_disableButton))
        {
            EnablePointer(false);
        }
        else if (OVRInput.GetUp(_disableButton))
        {
            EnablePointer(true);
        }

        if (_enabled)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _laserLengthmax, 1 << LayerMask.NameToLayer("UI")))
            {
                _buttonHit = hit.collider.GetComponentInParent<Button>();
                _laserMaterial.color = _hitColor;
                Vector3[] positions = new Vector3[2];
                positions[0] = transform.position;
                positions[1] = hit.point;
                _lineRenderer.SetPositions(positions);
            }
            else
            {
                _buttonHit = null;
                _laserMaterial.color = _defaultColor;
                Vector3[] positions = new Vector3[2];
                positions[0] = transform.position;
                positions[1] = transform.position + transform.TransformDirection(Vector3.forward) * _laserLengthmax;
                _lineRenderer.SetPositions(positions);
            }

            if (OVRInput.GetDown(_trigger) && _buttonHit)
            {
                Click();
            }
        }
    }

    public void Click()
    {
        _buttonHit.onClick.Invoke();
    }

    public void EnablePointer(bool enable)
    {
        _enabled = enable;
        _lineRenderer.enabled = enable;
    }
}
