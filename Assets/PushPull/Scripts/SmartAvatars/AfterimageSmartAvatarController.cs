using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimageSmartAvatarController : SmartAvatarController
{
    public Trail_manager _trailManager;

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        _trailManager.SetTrailActive(!_isStrafing);
    }
}
