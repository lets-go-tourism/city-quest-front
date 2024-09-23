using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BottomSheetManager : MonoBehaviour
{
    public static BottomSheetManager instance;
    private void Awake()
    {
        instance = this;

        placeGOList = new List<GameObject>();
        tourGOList = new List<GameObject>();
    }

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

    // Sorting 리스트
    public List<GameObject> placeGOList;
    public List<GameObject> tourGOList;

    private IEnumerator Start()
    {
        while (DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

        StartCoroutine(SettingList());
    }

    #region 바텀시트 만들기
    public IEnumerator SettingList()
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
            GameObject go = Instantiate(cardPlace, contentPlace);

            //placeGOList.Add(go);
            cardPlaceInfo = contentPlace.GetChild(i).GetComponent<CardPlaceInfo>();
            cardPlaceInfo.info[0].GetComponent<TextMeshProUGUI>().text = placeList[i].name.ToString();
            cardPlaceInfo.info[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(propList[i].latitude, propList[i].longitude));
            cardPlaceInfo.StartCoroutine(nameof(cardPlaceInfo.GetTexture), placeList[i].imageUrl);

            cardPlaceInfo.SettingPlaceType(placeList[i].status);
            cardPlaceInfo.SetServerProp(placeList[i]);
            cardPlaceInfo.setPlaceProp(propList[i]);

            //cardPlaceInfo.StartCoroutine(nameof(cardPlaceInfo.Start2));
        }

        yield return null;
    }

    // 관광정보 바텀시트
    IEnumerator GenTour()
    {
        int count = 0;

        while (count < tourList.Count)
        {
            int end = Mathf.Min(count + 30, tourList.Count);

            for (int i = count; i < end; i++)
            {
                GameObject go = Instantiate(cardTour, contentTour);

                tourGOList.Add(go);

                cardTourInfo = tourGOList[i].GetComponent<CardTourInfo>();

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
            if (count == 120)
            {
                count += 10;
            }
            else
            {
                count += 30;
            }

            if (count >= tourList.Count) break;

            yield return new WaitForSeconds(1);
        }
    }
    #endregion

    #region 거리 변환
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
    #endregion

    #region 글자수 제한
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
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            
        }
    }

    //#region 카드 재정렬
    //public void SortingPlaceCards(ServerProp list)
    //{
    //    for (int i = 0; i < contentPlace.childCount; i++)
    //    {


    //        if(list.propNo == contentPlace.GetChild(i).GetComponent<CardPlaceInfo>().ServerProp.propNo)
    //        contentPlace.GetChild(i).SetSiblingIndex(i);
       
    //    }
    //}

    //void SortingTourCards()
    //{

    //}
    //#endregion




    #region 태그 전환
    // 전체 보기
    public void soringAll(bool place)
    {
        if (place)
        {
            for (int i = 0; i < placeGOList.Count; i++)
            {
                placeGOList[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < tourGOList.Count; i++)
            {
                tourGOList[i].gameObject.SetActive(true);
            }
        }
    }

    // 장소 탭 카드 활성/비활성
    public void SortingPlace(CardPlaceInfo.Type type)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
        {
            for (int i = 0; i < placeGOList.Count; i++)
            {
                print("2차 진입 " + i);
                CardPlaceInfo card = placeGOList[i].GetComponent<CardPlaceInfo>();

                print("의 타입은 " + card.type);
                if (placeGOList[i].GetComponent<CardPlaceInfo>().type == type)
                {
                    print(placeGOList[i].name);
                    placeGOList[i].gameObject.SetActive(true);
                }
                else
                {
                    placeGOList[i].gameObject.SetActive(false);
                }
            }
        }
    }

    // 관광정보 탭 카드 활성/비활성
    public void SortingTour(string str)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
        {
            for (int i = 0; i < tourGOList.Count; i++)
            {
                if (tourGOList[i].GetComponent<CardTourInfo>().type.ToString() == str)
                {
                    tourGOList[i].gameObject.SetActive(true);
                }
                else
                {
                    tourGOList[i].gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion

    //// 장소 글자 변환
    //string TextBreakPlace(string text)
    //{
    //    string result = string.Empty;
    //    string[] splitStr = { " " };
    //    string tmp = text;
    //    string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

    //    char[] chars0 = text.ToCharArray();

    //    // 5글자 이하
    //    if (chars0.Length < 5)
    //    {
    //        // 1줄
    //        if (nameSplit.Length < 2)
    //        {
    //            result = nameSplit[0];
    //        }
    //        // 2줄
    //        else
    //        {
    //            result = nameSplit[0] + "\n" + nameSplit[1];
    //        }
    //    }

    //    // 6글자 이상
    //    else if (chars0.Length >= 5)
    //    {
    //        // 1줄
    //        if (nameSplit.Length < 2)
    //        {
    //            result = nameSplit[0];
    //        }

    //        // 2줄
    //        else
    //        {
    //            char[] chars1 = nameSplit[1].ToCharArray();

    //            // 두번째 줄 5글자 이하
    //            if (chars1.Length < 5)
    //            {
    //                result = nameSplit[0] + "\n" + nameSplit[1]; // �ι�° �� �ټ����� ����
    //            }
    //            // 두번째 줄 6글자 이상
    //            else
    //            {
    //                result = nameSplit[0] + "\n" + chars1[0] + chars1[1] + chars1[2] + chars1[3] + "..."; // �ι�° �� �ټ����� �ʰ�
    //            }
    //        }
    //    }

    //    return result;
    //}

    // 관광정보 글자 변환
}