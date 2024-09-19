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
        contents[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = info.title.ToString();
        // 거리
        double latTour = double.Parse(info.latitude);   // 위도
        double lonTour = double.Parse(info.longitude);  // 경도
        contents[2].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour));    // 실시간 거리
        // 주소
        contents[3].GetComponent<TextMeshProUGUI>().text = info.addr;
        // 링크
        //contents[4].GetComponent<OpenKakaoMap>().url = info.url;

        // 장소사진
        // url 있을 때
        if (info.imageUrl != string.Empty)
        {
            contents[5].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);            // 이미지 배경 흰색
            contents[6].GetComponent<TextMeshProUGUI>().enabled = false;                    // 안내문 비활성
        }
        // url 없을 때
        else
        {
            contents[5].GetComponent<Image>().sprite = null;                                // 이미지 초기화
            contents[7].GetComponent<RectTransform>().sizeDelta = new Vector2(840, 576);    // parent 크기 초기화
            contents[5].GetComponent<RectTransform>().sizeDelta = new Vector2(840, 540);    // 관광정보 이미지 크기 초기화
            contents[5].GetComponent<Image>().color = new Color(1f, 0.98f, 0.96f, 1f);      // 색 바꾸기
            contents[6].GetComponent<TextMeshProUGUI>().enabled = true;                     // 안내문 활성화
        }

        // 팝업창 UI 활성화
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), false);
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

        if (tourInfo.imageUrl != string.Empty)
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
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                ProcessImage(texture);
            }
        }

        TourInfoSetting(tourInfo);
    }

    private void ProcessImage(Texture2D texture)
    {
        // 원본 이미지 크기
        float originalWidth = texture.width;
        float originalHeight = texture.height;

        // 원본 이미지 비율 계산
        float originalAspect = originalWidth / originalHeight;

        // 목표 가로 길이에 맞춰 세로 길이 계산
        float targetWidth = 840;
        float targetHeight = targetWidth / originalAspect;

        // RectTransform의 사이즈 조정
        if (contents[7].GetComponent<RectTransform>() != null)
        {
            contents[5].GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, targetHeight);
            contents[7].GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, targetHeight + 36);
        }
        // UI.Image에 텍스처 적용
        if (contents[5].GetComponent<Image>() != null)
        {
            contents[5].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, originalWidth, originalHeight), new Vector2(0.5f, 0.5f));
        }

        contents[7].gameObject.SetActive(false);                                        // 이미지 비활성화
        contents[7].gameObject.SetActive(true);
    }

    #region 미사용
    //// 장소명 자르기
    //string TextBreak(string str)
    //{
    //    string result = string.Empty;
    //    string[] splitStr = { " " };
    //    string tmp = str;
    //    string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

    //    char[] chars0 = str.ToCharArray();

    //    // 다섯글자
    //    if (chars0.Length < 5)
    //    {
    //        // 1줄
    //        if (nameSplit.Length < 2)
    //        {
    //            ChangeSizeDelta(132);
    //            result = nameSplit[0];
    //        }
    //        // 2줄
    //        else
    //        {
    //            ChangeSizeDelta(240);
    //            result = nameSplit[0] + "\n" + nameSplit[1];
    //        }
    //    }

    //    // 여섯글자 이상
    //    else if (chars0.Length >= 5)
    //    {
    //        ChangeSizeDelta(240);

    //        // 띄어쓰기 없는 경우
    //        if (nameSplit.Length < 2)
    //        {
    //            result = nameSplit[0];
    //        }

    //        // 띄어쓰기 있는 경우
    //        else
    //        {
    //            char[] chars1 = nameSplit[1].ToCharArray(); // 두번째 줄 몇글자인지   

    //            // 두번째 줄이 5글자 이하
    //            if (chars1.Length < 5)
    //            {
    //                result = nameSplit[0] + "\n" + nameSplit[1]; // 두번째 줄 다섯글자 이하
    //            }
    //            // 두번째 줄이 6글자 이상
    //            else
    //            {
    //                result = nameSplit[0] + "\n" + chars1[0] + chars1[1] + chars1[2] + chars1[3] + "..."; // 두번째 줄 다섯글자 초과
    //            }
    //        }
    //    }

    //    return result;
    //}

    // 장소명 SizeDelta 크기 조절
    //void ChangeSizeDelta(int delta)
    //{
    //    contents[0].GetComponent<RectTransform>().sizeDelta = new Vector2(840, delta);
    //}
    #endregion
}