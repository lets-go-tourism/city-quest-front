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
    public GameObject cardPlace;    // 프랍
    public Transform contentPlace;  // 컨텐트

    // 관광정보
    public GameObject cardTour;     // 프랍
    public Transform contentTour;   // 컨텐트

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
        yield return StartCoroutine(GenPlace());

        // 관광정보 바텀시트
        yield return StartCoroutine(GenTour());

        // 검은 화면 끄기   ====================================== 스켈레톤 UI 로 대체하기 ======================================
        MainView_UI.instance.BackgroundDarkDisable();

        //KJY추가 
        LoadingTest loading = GameObject.FindFirstObjectByType<LoadingTest>();
        loading.connectionFinish = true;
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
        for (int i = 0; i < tourList.Count; i++)
        {
            Instantiate(cardTour, contentTour);
        }

        yield return StartCoroutine(nameof(settingTourCard));
    }

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

        while (count < tourList.Count)
        {
            int end = Mathf.Min(count + 20, tourList.Count);

            if(end == 100)
            {
                end = tourList.Count;
            }

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

            count = end;

            if (count <= 100)
            {
                yield return new WaitForSeconds(1);
            }
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
            double calculate = Math.Round(tmp / a, 1);
            result = calculate.ToString() + "km";
        }
        else
        {
            double calculate = Math.Floor(tmp * 10f) / 10f;
            result = calculate.ToString() + "m";
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SortingPlaceCards();
            SortingTourCards();
        }
    }

    #region 바텀시트 재정렬

    private int placeCount = 0;

    public void SortingPlaceCards()
    {
        DataManager.instance.SortPropAdventureList();
        propList = DataManager.instance.GetHomePropsList();
        //placeList = DataManager.instance.GetHomeAdventurePlacesList();

        placeCount = contentPlace.childCount;

        for (int i = 0; i < propList.Count; i++)
        {
            for (int j = 0; j < placeCount; j++)
            {
                if (propList[i].propNo != contentPlace.GetChild(j).GetComponent<CardPlaceInfo>().ServerProp.propNo)
                {
                    continue;
                }
                else
                {
                    contentPlace.GetChild(j).SetAsLastSibling();

                    //TextMeshProUGUI ditance = contentPlace.GetChild(j).GetComponent<CardPlaceInfo>().info[1].GetComponent<TextMeshProUGUI>();
                    //ditance.text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(propList[i].latitude, propList[i].longitude));
                    placeCount--;
                    break;
                }
            }
        }

        MainView_UI.instance.placeScrollRect.horizontalNormalizedPosition = 0;
    }

    private int tourCount = 0;

    public void SortingTourCards()
    {
        DataManager.instance.SortTourList();
        tourList = DataManager.instance.GetHometourPlacesList();
        tourCount = contentTour.childCount;

        for (int i = 0; i < tourList.Count; i++)
        {
            for (int j = 0; j < tourCount; j++)
            {
                if (tourList[i].idx != contentTour.GetChild(j).GetComponent<CardTourInfo>().ServerTourInfo.idx)
                {
                    continue;
                }
                else
                {
                    contentTour.GetChild(j).SetAsLastSibling();
                    //TextMeshProUGUI ditance = contentTour.GetChild(j).GetComponent<CardTourInfo>().info[1].GetComponent<TextMeshProUGUI>();
                    //ditance.text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(double.Parse(tourList[i].latitude), double.Parse(tourList[i].longitude)));
                    tourCount--;
                    break;
                }
            }
        }

        MainView_UI.instance.tourScrollRect.horizontalNormalizedPosition = 0;
        //StartCoroutine(nameof(SortingTourCardsCor));
    }

    private IEnumerator SortingTourCardsCor()
    {
        yield return null;
    }
    #endregion

    #region 태그 및 필터 관리
    // 전체 보기
    public void SortingAll(bool place)
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