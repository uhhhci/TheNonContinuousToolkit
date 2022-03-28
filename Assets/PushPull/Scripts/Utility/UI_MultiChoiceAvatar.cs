using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MultiChoiceAvatar : MonoBehaviour, ButtonObserver
{
    public string _playerPref;
    public List<float> _options;
    public List<string> _optionsDisplayString;
    private int _currentIndex;
    public Text _displayText;
    public float _standardValue;

    public List<GameObject> _avatars;

    public void ButtonClicked(string name)
    {
        if (name == "Left")
        {
            MoveIndex(-1);
            UpdateSelectedOption();
        }
        else if (name == "Right")
        {
            MoveIndex(1);
            UpdateSelectedOption();
        }

    }

    private void MoveIndex(int direction)
    {
        _currentIndex += direction;
        if (_currentIndex >= _options.Count)
        {
            _currentIndex = 0;
        }
        else if (_currentIndex < 0)
        {
            _currentIndex = _options.Count - 1;
        }
    }

    private void UpdateSelectedOption()
    {
        PlayerPrefs.SetFloat(_playerPref, _options[_currentIndex]);

        _displayText.text = _optionsDisplayString[_currentIndex];

        for (int i = 0; i < _avatars.Count; i++)
        {
            _avatars[i].SetActive(i == _currentIndex);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        float currentValue = 0;
        currentValue = PlayerPrefs.GetFloat(_playerPref, _standardValue);
        for (int i = 0; i < _options.Count; i++)
        {
            if (_options[i] == currentValue)
            {
                _currentIndex = i;
                break;
            }
        }
        UpdateSelectedOption();
    }
}
