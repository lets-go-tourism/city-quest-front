using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SettingPropInfo;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingTourInfo : MonoBehaviour
{
    public static SettingTourInfo instance;
    private void Awake()
    {
        instance = this;
    }

    public Transform[] contents;

    public void TourInfoSetting()
    {
        // �̸� : contents[0].GetComponent<TextMeshProUGUI>().text = ;
        // �Ÿ� : contents[1].GetComponent<TextMeshProUGUI>().text = ;
        // �ּ� : contents[2].GetComponent<TextMeshProUGUI>().text = ;
        // ��ũ : contents[4].GetComponent<OpenKakaoMap>().url = ;
        // ���� : StartCoroutine(nameof(GetTexture), );
    }

    public IEnumerator GetTexture(string str)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(str);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            contents[4].GetComponent<RawImage>().texture = myTexture;
        }
    }
}