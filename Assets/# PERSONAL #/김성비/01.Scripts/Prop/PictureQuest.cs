using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureQuest : MonoBehaviour
{
    Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => TakingPicture());
    }

    public int propnumber;

    void TakingPicture()
    {
        GameObject.Find("CameraController").GetComponent<CameraFeed>().SetWebCam();
        // À±ÁÖÇÑÅ× ÇÁ¶ø³Ñ¹ö ½î±â
        KJY_ConnectionTMP.instance.QuestNo(propnumber);
    }
}
