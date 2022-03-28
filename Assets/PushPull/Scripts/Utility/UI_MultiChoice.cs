using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MultiChoice : MonoBehaviour, ButtonObserver
{
    public string _settingName;
    public List<float> _options;
    public List<string> _optionsDisplayString;
    private int _currentIndex;
    public Text _displayText;
    public float _standardValue;


    // Start is called before the first frame update
    void Start()
    {
        PushPullSettingsManager._instance.RegisterButton(this);
        PullSettingFromManager();
    }

    public void ButtonClicked(string name)
    {
        if (name == "Left")
        {
            MoveIndex(-1);
            PushSettingToManager();
        }
        else if (name == "Right")
        {
            MoveIndex(1);
            PushSettingToManager();
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

    private void PushSettingToManager()
    {
        PushPullSettingsManager._instance.SetValue(_settingName, _options[_currentIndex]);
        _displayText.text = _optionsDisplayString[_currentIndex];
    }

    public void PullSettingFromManager()
    {
        float currentValue = PushPullSettingsManager._instance.GetValue(_settingName);

        for (int i = 0; i < _options.Count; i++)
        {
            if (_options[i] == currentValue)
            {
                _currentIndex = i;
                break;
            }
        }
        _displayText.text = _optionsDisplayString[_currentIndex];
    }
}
