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

    void TakingPicture()
    {
        // 모델링 비활성화
        Props_UI.instance.propModeling.gameObject.SetActive(false);

        // 팝업창 UI 비활성화
        Props_UI.instance.canvasProp.enabled = false;

        // 카메라 UI 활성화
        Props_UI.instance.CanvasCamera.enabled = true;
    }
}
