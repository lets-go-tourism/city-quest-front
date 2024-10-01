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
        PopUpMovement.instance.skeleton = false;

        if (name == "prop" && PopUpMovement.instance.placeState == PopUpMovement.PlaceState.UP)
        {
            if (PopUpMovement.instance.adventured)
            {
                PopUpMovement.instance.skPlaceAD.anchoredPosition = new Vector2(0, -2600);
            }
            else
            {
                PopUpMovement.instance.skPlaceUN.anchoredPosition = new Vector2(0, -2600);
            }

            PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);
            Props_UI.instance.ResetScollView();
        }

        else if(name =="tour" && PopUpMovement.instance.tourState == PopUpMovement.TourState.UP)
        {
            PopUpMovement.instance.skTour.anchoredPosition = new Vector2(0, -2500);

            PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), false);
            Props_UI.instance.ResetScollView();
            //SettingTourInfo.instance.StopCoroutine(nameof(SettingTourInfo.instance.UpdateDistance));
        }
    }
}
