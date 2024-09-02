using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCanvasOnOff : MonoBehaviour
{
    [SerializeField] private Canvas settingCanvas;

    public void SettingCanvasOn()
    {
        settingCanvas.enabled = true;
    }

    public void SettingCanvasOff()
    {
        settingCanvas.enabled = false;
    }
}
