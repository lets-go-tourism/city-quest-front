using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenKakaoMap : MonoBehaviour
{
    Button btn;
    public string url;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => KakaoMapLink(url));
    }

    public void SetURL(string str)
    {
        url = str;
    }

    void KakaoMapLink(string str)
    {
        // ���� : str�� �̿��� ��ũ �����ϱ�
        print("īī���� ��ũ : " + str);
    }
}
