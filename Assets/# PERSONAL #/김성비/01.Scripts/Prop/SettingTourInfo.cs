using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

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
        // 이미지
        contents[0].GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TourSprites/" + info.contenttypeid);

        // 이름
        contents[1].gameObject.SetActive(false);
        contents[1].gameObject.SetActive(true);
        contents[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(info.title);

        // 거리
        //contents[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(double.Parse(info.distance));
        double latTour = double.Parse(info.latitude);
        double lonTour = double.Parse(info.longitude);

        contents[2].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour));

        // 주소
        contents[3].GetComponent<TextMeshProUGUI>().text = info.addr;

        // 링크
        //contents[4].GetComponent<OpenKakaoMap>().url = info.url;

        // 사진
        if (info.imageUrl != string.Empty)
        {
            contents[5].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
            //yield return StartCoroutine(nameof(GetTexture), info.imageUrl);
            contents[6].GetComponent<TextMeshProUGUI>().enabled = false;
        }
        else
        {
            contents[5].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
            contents[5].GetComponent<RawImage>().texture = null;
            contents[6].GetComponent<TextMeshProUGUI>().enabled = true;
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
        string result = string.Empty;

        double tmp = Math.Truncate(distance);

        if (tmp > 1000)
        {
            double calcultate = tmp / 1000;
            result = (Math.Round(calcultate, 1)).ToString() + "km";
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

        if(tourInfo.imageUrl != string.Empty)
        {
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

                contents[5].GetComponent<RawImage>().texture = myTexture;
                
            }
        }
        TourInfoSetting(tourInfo);
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