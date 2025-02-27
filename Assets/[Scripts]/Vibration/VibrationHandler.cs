using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationHandler : Singleton<VibrationHandler>
{
    
    public void Vibrate()
    {
#if UNITY_IOS || UNITY_ANDROID
        Handheld.Vibrate();
#endif
    }
    
}
