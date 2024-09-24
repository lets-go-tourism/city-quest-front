using UnityEngine;
using UnityEngine.UI;

public class OpenTourKakaoMap : MonoBehaviour
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
        Application.OpenURL("https://map.kakao.com/?q=" + str);
    }
}
