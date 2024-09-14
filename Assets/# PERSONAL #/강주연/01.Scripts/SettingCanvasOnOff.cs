using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingCanvasOnOff : MonoBehaviour
{
    [SerializeField] private Canvas settingCanvas;
    [SerializeField] private GameObject ksb_setting;

    private void Start()
    {
        ksb_setting = GameObject.Find("Settings");
        ksb_setting.GetComponent<Button>().onClick.AddListener(SettingCanvasOn);
    }

    public void SettingCanvasOn()
    {
        settingCanvas.enabled = true;
        ksb_setting.transform.root.GetComponent<tmpTouch>().state = tmpTouch.State.Setting;
    }

    public void SettingCanvasOff()
    {
        settingCanvas.enabled = false;
    }
}
