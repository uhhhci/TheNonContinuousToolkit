using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageBoard : MonoBehaviour
{
    public Transform _vrCamera;
    public TextMeshPro _textBody;
    public TextMeshPro _textHeader;
    public GameObject _controllerIconOn;
    public GameObject _controllerIconOff;
    public GameObject _board;
    public GameObject _image;
    public GameObject _groupID;
    public TextMeshPro _groupIDText;
    private Material _imageMaterial;

    public bool _showControllerIcon;

    // Start is called before the first frame update
    void Start()
    {
        _imageMaterial = _image.GetComponent<Renderer>().material;
        StartCoroutine(FlashControllerIcon());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posCam = _vrCamera.position;
        posCam.y = 0;
        transform.position = posCam;

        Vector3 posQuad = _textBody.transform.position;
        posQuad.y = 0;

        if (Vector3.Angle(_vrCamera.forward, posQuad - posCam) > 30)
        {
            Vector3 lookatTarget = _vrCamera.forward;
            lookatTarget.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookatTarget, Vector3.up), Time.deltaTime);
        }
    }

    private IEnumerator FlashControllerIcon()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (_showControllerIcon)
            {
                _controllerIconOn.SetActive(_controllerIconOff.activeInHierarchy);
                _controllerIconOff.SetActive(!_controllerIconOff.activeInHierarchy);
            }
            else
            {
                _controllerIconOn.SetActive(false);
                _controllerIconOff.SetActive(false);
            }
        }
    }

    public void SetBodyText(string text)
    {
        text = text.Replace("\\n", "\n");
        _textBody.text = text;
    }

    public void SetHeaderText(string text)
    {
        text = text.Replace("\\n", "\n");
        _textHeader.text = text;
    }

    public void ShowBoard(bool show)
    {
        _board.SetActive(show);
    }

    public void ShowGroupID(bool show)
    {
        _groupID.SetActive(show);
    }

    public void SetGroupID(int ID)
    {
        _groupIDText.text = "" + ID;
    }

    public void SetImage(Texture2D image)
    {
        _imageMaterial.mainTexture = image;
    }

    public void ShowImage(bool show)
    {
        _image.SetActive(show);
    }

    public void ShowControllerIcon(bool show)
    {
        _showControllerIcon = show;

        if (_showControllerIcon)
        {
            _controllerIconOn.SetActive(_controllerIconOff.activeInHierarchy);
            _controllerIconOff.SetActive(!_controllerIconOff.activeInHierarchy);
        }
        else
        {
            _controllerIconOn.SetActive(false);
            _controllerIconOff.SetActive(false);
        }
    }
}
