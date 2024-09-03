using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SettingPropInfo;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class SettingTourInfo : MonoBehaviour
{
    public static SettingTourInfo instance;
    private void Awake()
    {
        instance = this;
    }

    public Transform[] contents;

    public void TourInfoSetting(ServerTourInfo info)
    {
        // 이름
        contents[0].gameObject.SetActive(false);
        contents[0].gameObject.SetActive(true);
        contents[0].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(info.title);

        // 거리
        //contents[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(double.Parse(info.distance));
        double latTour = double.Parse(info.latitude);
        double lonTour = double.Parse(info.longitude);

        GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour);

        contents[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour));

        // 주소
        contents[2].GetComponent<TextMeshProUGUI>().text = info.addr;

        // 링크
        //contents[4].GetComponent<OpenKakaoMap>().url = info.url;

        // 사진
        if (info.imageUrl != string.Empty)
        {
            contents[5].GetComponent<TextMeshProUGUI>().enabled = false;
            contents[4].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
            //yield return StartCoroutine(nameof(GetTexture), info.imageUrl);
        }
        else
        {
            contents[5].GetComponent<TextMeshProUGUI>().enabled = true;
            contents[4].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
            contents[4].GetComponent<RawImage>().texture = null;
        }

        // 팝업창 UI 활성화
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), false);
    }

    // 장소명 자르기
    string TextBreak(string str)
    {
        string result = string.Empty;
        string[] splitStr = { " " };
        string tmp = str;
        string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

        char[] chars0 = str.ToCharArray();

        // 다섯글자
        if (chars0.Length < 5)
        {
            // 1줄
            if (nameSplit.Length < 2)
            {
                ChangeSizeDelta(132);
                result = nameSplit[0];
            }
            // 2줄
            else
            {
                ChangeSizeDelta(240);
                result = nameSplit[0] + "\n" + nameSplit[1];
            }
        }

        // 여섯글자 이상
        else if (chars0.Length >= 5)
        {
            ChangeSizeDelta(240);

            // 띄어쓰기 없는 경우
            if (nameSplit.Length < 2)
            {
                result = nameSplit[0];
            }

            // 띄어쓰기 있는 경우
            else
            {
                char[] chars1 = nameSplit[1].ToCharArray(); // 두번째 줄 몇글자인지   

                // 두번째 줄이 5글자 이하
                if (chars1.Length < 5)
                {
                    result = nameSplit[0] + "\n" + nameSplit[1]; // 두번째 줄 다섯글자 이하
                }
                // 두번째 줄이 6글자 이상
                else
                {
                    result = nameSplit[0] + "\n" + chars1[0] + chars1[1] + chars1[2] + chars1[3] + "..."; // 두번째 줄 다섯글자 초과
                }
            }
        }

        return result;
    }

    // 장소명 SizeDelta 크기 조절
    void ChangeSizeDelta(int delta)
    {
        contents[0].GetComponent<RectTransform>().sizeDelta = new Vector2(840, delta);
    }

    // 거리 변환
    string ConvertDistance(double distance)
    {
        string result = "";

        int tmp = Mathf.FloorToInt((float)distance);

        if (tmp > 1000)
        {
            result = (tmp / 1000).ToString() + "km";
        }
        else
        {
            result = tmp.ToString() + "m";
        }

        return result;
    }

    // 장소 이미지 변환
    public IEnumerator GetTexture(ServerTourInfo tourInfo)
    {
        // 암전 키고
        MainView_UI.instance.BackgroundDarkEnable();

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(tourInfo.imageUrl);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

            // 암전 끄고
            MainView_UI.instance.BackgroundDarkDisable();
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            contents[4].GetComponent<RawImage>().texture = myTexture;
            TourInfoSetting(tourInfo);
        }
    }

    // 색깔 변환
    bool HextoColor(string hex, out Color color)
    {
        hex = hex.Replace("#", "");

        byte r = System.Convert.ToByte(hex.Substring(0, 2), 16);
        byte g = System.Convert.ToByte(hex.Substring(2, 2), 16);
        byte b = System.Convert.ToByte(hex.Substring(4, 2), 16);

        color = new Color(r, g, b, 255);

        return true;
    }
}