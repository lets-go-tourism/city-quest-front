using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BottomSheetManager : MonoBehaviour
{
    #region 변수
    // 장소
    public GameObject cardPlace;
    public Transform contentPlace;

    // 관광정보
    public GameObject cardTour;
    public Transform contentTour;

    // 리스트
    List<ServerProp> propList;
    List<ServerAdventurePlace> placeList;
    List<ServerTourInfo> tourList;

    // 각 카드들의 컴포넌트
    CardPlaceInfo cardPlaceInfo;
    CardTourInfo cardTourInfo;
    #endregion

    #region Awake&Start
    public static BottomSheetManager instance;
    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        while (DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

        StartCoroutine(SettingBottomSheet());
    }
    #endregion

    #region 바텀시트 생성
    public IEnumerator SettingBottomSheet()
    {
        propList = DataManager.instance.GetHomePropsList();
        placeList = DataManager.instance.GetHomeAdventurePlacesList();
        tourList = DataManager.instance.GetHometourPlacesList();

        // 장소 바텀시트
        print("장소 바텀시트");
        yield return StartCoroutine(GenPlace());

        // 관광정보 바텀시트
        print("관광정보 바텀시트");
        yield return StartCoroutine(GenTour());

        print("검은 화면 끄기");
        // 검은 화면 끄기   ====================================== 스켈레톤 UI 로 대체하기 ======================================
        MainView_UI.instance.BackgroundDarkDisable();
    }

    // 장소 바텀시트
    IEnumerator GenPlace()
    {
        for (int i = 0; i < placeList.Count; i++)
        {
            Instantiate(cardPlace, contentPlace);
        }

        SettingPlaceCard();

        yield return null;
    }

    // 관광정보 바텀시트
    IEnumerator GenTour()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate(cardTour, contentTour);
        }

        yield return StartCoroutine(nameof(settingTourCard));
    }
    #endregion

    #region 바텀시트 정보
    public void SettingPlaceCard()
    {
        for (int i = 0; i < placeList.Count; i++)
        {
            cardPlaceInfo = contentPlace.GetChild(i).GetComponent<CardPlaceInfo>();
            cardPlaceInfo.info[0].GetComponent<TextMeshProUGUI>().text = placeList[i].name.ToString();
            cardPlaceInfo.info[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(propList[i].latitude, propList[i].longitude));
            cardPlaceInfo.StartCoroutine(nameof(cardPlaceInfo.GetTexture), placeList[i].imageUrl);

            cardPlaceInfo.SettingPlaceType(placeList[i].status);
            cardPlaceInfo.SetServerProp(placeList[i]);
            cardPlaceInfo.setPlaceProp(propList[i]);
        }
    }

    public IEnumerator settingTourCard()
    {
        int count = 0;

        while (count < 100)
        {
            int end = Mathf.Min(count + 20, 100);

            for (int i = count; i < end; i++)
            {
                cardTourInfo = contentTour.GetChild(i).GetComponent<CardTourInfo>();    //tourGOList[i].GetComponent<CardTourInfo>();

                cardTourInfo.info[0].GetComponent<TextMeshProUGUI>().text = TextBreakTour(tourList[i].title);
                cardTourInfo.info[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(double.Parse(tourList[i].latitude), double.Parse(tourList[i].longitude)));
                if (tourList[i].imageUrl.Length > 5)
                {
                    cardTourInfo.info[2].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    cardTourInfo.info[3].GetComponent<TextMeshProUGUI>().enabled = false;
                    cardTourInfo.StartCoroutine(nameof(cardTourInfo.GetTexture), tourList[i].imageUrl);
                }
                else
                {
                    cardTourInfo.info[2].GetComponent<Image>().color = new Color(1f, 0.98f, 0.96f, 1f);
                    cardTourInfo.info[3].GetComponent<TextMeshProUGUI>().enabled = true;
                }
                cardTourInfo.SettingTourType(tourList[i].contenttypeid);
                cardTourInfo.InputTourList(tourList[i]);

                //cardTourInfo.StartCoroutine(nameof(cardTourInfo.Start2));
            }
            if (count < 100)
            {
                count += 20;
            }

            yield return new WaitForSeconds(1);
        }
    }

    string ConvertDistance(double distance)
    {
        //print(distance);
        string result = string.Empty;

        double tmp = distance;
        double a = 1000;
        if (tmp > a)
        {
            double calcultate = Math.Round(tmp / a, 1);
            result = calcultate.ToString() + "km";
        }
        else
        {
            result = tmp.ToString() + "m";
        }

        return result;
    }

    string TextBreakTour(string text)
    {
        string result = string.Empty;
        char[] chars = text.ToCharArray();

        // _ 체크
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '_') { chars[i] = ' '; }
        }

        result = chars.ArrayToString();
        //if(chars.Length > 10)
        //{
        //    result = chars[0].ToString() + chars[1].ToString() + chars[2].ToString() + chars[3].ToString() + chars[4].ToString() + chars[5].ToString() + chars[6].ToString() + chars[7].ToString() + chars[8].ToString() + chars[9].ToString() + chars[10].ToString() + "...";
        //}

        return result;
    }
    #endregion

    #region 바텀시트 재정렬
    public void SortingPlaceCards()
    {
        DataManager.instance.SortPropAdventureList();

        SettingPlaceCard();

        MainView_UI.instance.placeScrollRect.horizontalNormalizedPosition = 0;
    }

    void SortingTourCards()
    {
        DataManager.instance.SortTourList();

        StartCoroutine(nameof(settingTourCard));

        MainView_UI.instance.tourScrollRect.horizontalNormalizedPosition = 0;
    }
    #endregion

    #region 태그 및 필터 관리
    // 전체 보기
    public void soringAll(bool place)
    {
        MainView_UI.instance.BSGuideUI.enabled = false;

        if (place)
        {
            for (int i = 0; i < contentPlace.childCount; i++)
            {
                contentPlace.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < contentTour.childCount; i++)
            {
                contentTour.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    // 장소 탭 카드 활성/비활성
    public void FilteringPlace(CardPlaceInfo.Type type)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
        {
            for (int i = 0; i < contentPlace.childCount; i++)
            {
                //print("2차 진입 " + i);
                CardPlaceInfo card = contentPlace.GetChild(i).GetComponent<CardPlaceInfo>();

                //print("의 타입은 " + card.type);
                if (contentPlace.GetChild(i).GetComponent<CardPlaceInfo>().type == type)
                {
                    //print(contentPlace.GetChild(i).name);
                    contentPlace.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    contentPlace.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (CheckChildActivated(contentPlace))
            {
                MainView_UI.instance.BSGuideUI.enabled = true;

                if (type == CardPlaceInfo.Type.UNREVEAL)
                {
                    MainView_UI.instance.BSGuideUI.text = "미탐험 장소가 없습니다." + "\n" + "업데이트를 기다려주세요!";
                }
                else if (type == CardPlaceInfo.Type.REVEAL)
                {
                    MainView_UI.instance.BSGuideUI.text = "탐험한 장소가 없습니다.";
                }
            }
            else
            {
                MainView_UI.instance.BSGuideUI.enabled = false;
            }
        }
    }

    // 관광정보 탭 카드 활성/비활성
    public void FilteringTour(string str)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
        {
            for (int i = 0; i < contentTour.childCount; i++)
            {
                if (contentTour.GetChild(i).GetComponent<CardTourInfo>().type.ToString() == str)
                {
                    contentTour.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    contentTour.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (CheckChildActivated(contentTour))
            {
                MainView_UI.instance.BSGuideUI.text = "관광정보가 없습니다.";
                MainView_UI.instance.BSGuideUI.enabled = true;
            }
            else
            {
                MainView_UI.instance.BSGuideUI.enabled = false;
            }
        }
    }

    bool CheckChildActivated(Transform parent)
    {
        // 하나라도 활성화 되어있으면 false 
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.activeSelf)
            {
                return false;
            }
        }

        // 전부 비활성화 되어있으면
        return true;
    }
    #endregion
}