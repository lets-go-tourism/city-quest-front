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
        // �������� �����ѹ� ���
        KJY_ConnectionTMP.instance.QuestNo(propnumber);
    }
}
