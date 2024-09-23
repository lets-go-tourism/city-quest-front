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
        // 할일 : str을 이용해 링크 연결하기
        print("카카오맵 링크 : " + str);
    }
}
