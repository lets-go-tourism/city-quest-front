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
        if (name == "prop" && PopUpMovement.instance.placeState == PopUpMovement.PlaceState.UP)
        {
            PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);
        }
        else if(name =="tour" && PopUpMovement.instance.tourState == PopUpMovement.TourState.UP)
        {
            PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), false);
        }
    }
}
