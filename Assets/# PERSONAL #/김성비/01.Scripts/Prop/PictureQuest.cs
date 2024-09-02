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
        // �𵨸� ��Ȱ��ȭ
        Props_UI.instance.propModeling.gameObject.SetActive(false);

        // �˾�â UI ��Ȱ��ȭ
        Props_UI.instance.canvasProp.enabled = false;

        // ī�޶� UI Ȱ��ȭ
        Props_UI.instance.CanvasCamera.enabled = true;
    }
}
