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
    double latTour;
    double lonTour;

    public void TourInfoSetting(ServerTourInfo info)
    {
        // 이미지
        SettingBGColor(int.Parse(info.contenttypeid));
        contents[0].GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TourSprites/" + info.contenttypeid);
        // 이름
        contents[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = info.title.ToString();
        // 거리
        latTour = double.Parse(info.latitude);   // 위도
        lonTour = double.Parse(info.longitude);  // 경도
        contents[2].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour));    // 실시간 거리
        // 주소
        contents[3].GetComponent<TextMeshProUGUI>().text = info.addr;
        // 링크
        contents[4].GetComponent<OpenTourKakaoMap>().SetURL(info.addr);
        // 장소사진        
        if (info.imageUrl != string.Empty)                                                  // url 있을 때
        {
            contents[7].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);            // 이미지 배경 흰색
            contents[8].GetComponent<TextMeshProUGUI>().enabled = false;                    // 안내문 비활성
        }

        else                                                                                // url 없을 때
        {
            SettingSize(540f);                                                              // 사이즈 세팅
            contents[7].GetComponent<Image>().sprite = null;                                // 이미지 초기화
            contents[7].GetComponent<Image>().color = new Color(1f, 0.882f, 0.624f, 1f);      // 색 바꾸기
            contents[8].GetComponent<TextMeshProUGUI>().enabled = true;                     // 안내문 활성화
        }
    }

    void SettingSize(float CT)
    {
        float contentY = CT;
        // 이미지 비활성화
        contents[5].gameObject.SetActive(false);
        // 이미지
        contents[5].GetComponent<RectTransform>().sizeDelta = new Vector2(840, contentY + 36);          // 컨텐트 크기
        contents[6].GetComponent<RectTransform>().sizeDelta = new Vector2(840, contentY);               // 마스크 크기     
        contents[7].GetComponent<RectTransform>().sizeDelta = new Vector2(840, contentY);               // 이미지 크기
        // 스크롤뷰
        float scrollY = 720f + contentY - 54f;
        if (scrollY > 1500) scrollY = 1500f;
        PopUpMovement.instance.rtTour.sizeDelta = new Vector2(960, scrollY);                            // 스크롤뷰 크기  
        Props_UI.instance.masks[0].GetComponent<RectTransform>().sizeDelta = new Vector2(960, scrollY); // top 마스크 크기        
        Props_UI.instance.masks[0].fillAmount = 60f / scrollY;                                          // top 마스크 채우기        
        Props_UI.instance.masks[1].GetComponent<RectTransform>().sizeDelta = new Vector2(960, scrollY); // bottom 마스크 크기        
        Props_UI.instance.masks[1].fillAmount = 60f / scrollY;                                          // bottom 마스크 채우기
        // 이미지 활성화
        contents[5].gameObject.SetActive(true);
    }

    #region 변환
    // 아이콘 배경색 변환
    void SettingBGColor(int num)
    {
        if (num == 12) { contents[0].GetChild(0).GetComponent<Image>().color = new Color(0.674f, 0.78f, 0.145f, 1f); }
        else if (num == 14) { contents[0].GetChild(0).GetComponent<Image>().color = new Color(0.403f, 0.772f, 0.956f, 1f); }
        else if (num == 15) { contents[0].GetChild(0).GetComponent<Image>().color = new Color(0.87f, 0.513f, 0.839f, 1f); }
        else if (num == 25) { contents[0].GetChild(0).GetComponent<Image>().color = new Color(1f, 0.784f, 0.13f, 1f); }
        else if (num == 28) { contents[0].GetChild(0).GetComponent<Image>().color = new Color(0.478f, 0.549f, 1f, 1f); }
        else if (num == 32) { contents[0].GetChild(0).GetComponent<Image>().color = new Color(0.6f, 0.478f, 0.86f, 1f); }
        else if (num == 38) { contents[0].GetChild(0).GetComponent<Image>().color = new Color(0.21f, 0.796f, 0.698f, 1f); }
        else if (num == 39) { contents[0].GetChild(0).GetComponent<Image>().color = new Color(0.945f, 0.509f, 0.576f, 1f); }

    }

    // 거리 변환
    string ConvertDistance(double distance)
    {
        string result = string.Empty;

        double tmp = Math.Truncate(distance);

        if (tmp > 1000)
        {
            double calcultate = tmp / 1000;
            result = Math.Round(calcultate, 1).ToString() + "km";
        }
        else
        {
            double floor = Math.Floor(tmp);
            result = floor.ToString() + "m";
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

        // 팝업창 UI 활성화
        if (!PopUpMovement.instance.tourCancel)
        {
            yield return new WaitForSeconds(0.5f);
            PopUpMovement.instance.skTour.anchoredPosition = new Vector2(0, -2500);
            PopUpMovement.instance.skeleton = false;
            PopUpMovement.instance.rtTour.anchoredPosition = new Vector2(0, 0);

            PopUpMovement.instance.placeUNCancel = false;
            PopUpMovement.instance.placeADcancel = false;
        }
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

        // UI.Image에 텍스처 적용
        if (contents[7].GetComponent<Image>() != null)
        {
            contents[7].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, originalWidth, originalHeight), new Vector2(0.5f, 0.5f));
        }

        SettingSize(targetHeight);
    }
    #endregion
}