using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    Button btn;
    public string str;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => ClosePopUp(str));
    }

    void ClosePopUp(string name)
    {
        if (name == "prop")
        {
            Props_UI.instance.canvasProp.enabled = false;
        }
        else if(name =="tour")
        {
            Props_UI.instance.canvasTour.enabled = false;
        }

        // 3D ¸ðµ¨¸µ        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(false);
    }
}
