using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public PushPullBrain _pushPullBrain;
    public MessageBoard _messageBoard;
    public GameObject _playerController;
    public GameObject _avatar;
    public OVRInput.Button _inputNext = OVRInput.Button.One;
    public OVRInput.Button _inputPrevious = OVRInput.Button.Two;
    public List<TutorialState> _tutorialStates = new List<TutorialState>();
    public TutorialState _currentTutorialState;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnterNextTutorialState", 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(_inputNext) || Input.GetKeyDown(KeyCode.Return))
        {
            EnterNextTutorialState();
        }

        if (OVRInput.GetDown(_inputPrevious) || Input.GetKeyDown(KeyCode.Backspace))
        {
            EnterPreviousTutorialState();
        }
    }

    private void EnterNextTutorialState()
    {
        if (!_currentTutorialState)
        {
            _currentTutorialState = _tutorialStates[0];
        }
        else if (_tutorialStates.IndexOf(_currentTutorialState) < _tutorialStates.Count - 1)
        {
            _currentTutorialState = _tutorialStates[_tutorialStates.IndexOf(_currentTutorialState) + 1];
        }

        ReadTutorialState();
    }

    private void EnterPreviousTutorialState()
    {
        if (_tutorialStates.IndexOf(_currentTutorialState) > 0)
        {
            _currentTutorialState = _tutorialStates[_tutorialStates.IndexOf(_currentTutorialState) - 1];
        }

        ReadTutorialState();
    }

    private void ReadTutorialState()
    {
        ResetPositions();

        PushPullBrain._instance.EnableLocomotionType(PushPullBrain.LocomotionMovement.JoystickMovement, _currentTutorialState._enableJoystickMovement);
        PushPullBrain._instance.EnableLocomotionType(PushPullBrain.LocomotionMovement.PushPullMovement, _currentTutorialState._enablePushPullMovement);
        PushPullBrain._instance.EnableLocomotionType(PushPullBrain.LocomotionMovement.Teleport, _currentTutorialState._enableTeleportMovement);
        PushPullBrain._instance.EnableLocomotionType(PushPullBrain.LocomotionTurning.JoystickTurning, _currentTutorialState._enableJoystickTurning);
        PushPullBrain._instance.EnableLocomotionType(PushPullBrain.LocomotionTurning.PushPullTurning, _currentTutorialState._enablePushPullTurning);
        _messageBoard.SetBodyText(_currentTutorialState._messageBoardText);
        _messageBoard.ShowBoard(_currentTutorialState._messageBoardText != "");
        _messageBoard.SetImage(_currentTutorialState._image);
        _messageBoard.ShowImage(_currentTutorialState._image != null);
        _currentTutorialState._event?.Raise();
    }

    public void ResetPositions()
    {
        _playerController.transform.position = Vector3.zero;
        _playerController.transform.rotation = Quaternion.identity;
        _avatar.transform.position = Vector3.zero;
        _avatar.transform.rotation = Quaternion.identity;
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}