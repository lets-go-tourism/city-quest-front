using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BottomSheetManager : MonoBehaviour
{
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

    public List<GameObject> placeGOList;
    public List<GameObject> tourGOList;

    public static BottomSheetManager instance;
    private void Awake()
    {
        instance = this;

        placeGOList = new List<GameObject>();
        tourGOList = new List<GameObject>();
    }

    CardPlaceInfo cardPlaceInfo;
    CardTourInfo cardTourInfo;
    private IEnumerator Start()
    {
        while (DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

        StartCoroutine(SettingList());
    }

    // 초기 세팅
    public IEnumerator SettingList()
    {
        propList = DataManager.instance.GetHomePropsList();
        placeList = DataManager.instance.GetHomeAdventurePlacesList();
        tourList = DataManager.instance.GetHometourPlacesList();

        // 장소 카드 생성
        for (int i = 0; i < placeList.Count; i++)
        {
            GameObject go = Instantiate(cardPlace, contentPlace);

            placeGOList.Add(go);

            cardPlaceInfo = placeGOList[i].GetComponent<CardPlaceInfo>();

            cardPlaceInfo.info[0].GetComponent<TextMeshProUGUI>().text = TextBreakPlace(placeList[i].name);
            cardPlaceInfo.info[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(propList[i].latitude, propList[i].longitude));
            cardPlaceInfo.StartCoroutine(nameof(cardPlaceInfo.GetTexture), placeList[i].imageUrl);
            cardPlaceInfo.SettingPlaceType(placeList[i].status);
            cardPlaceInfo.SetServerProp(placeList[i]);
            cardPlaceInfo.setPlaceProp(propList[i]);

            cardPlaceInfo.StartCoroutine(nameof(cardPlaceInfo.Start));
        }

        yield return StartCoroutine(GenTour());

        yield return null;
    }

    IEnumerator GenTour()
    {
        // 관광정보 카드 생성
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
                if (tourList[i].imageUrl != string.Empty)
                {
                    cardTourInfo.info[2].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
                    cardTourInfo.info[3].GetComponent<TextMeshProUGUI>().enabled = false;
                    cardTourInfo.StartCoroutine(nameof(cardTourInfo.GetTexture), tourList[i].imageUrl);
                }
                else
                {
                    cardTourInfo.info[2].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
                    cardTourInfo.info[3].GetComponent<TextMeshProUGUI>().enabled = true;
                }
                cardTourInfo.SettingTourType(tourList[i].contenttypeid);
                cardTourInfo.InputTourList(tourList[i]);
            }
            if (count == 120)
            {
                count += 10;
            }
            else
            {
                count += 30;
            }

            if(count >= tourList.Count) yield break;

            yield return new WaitForSeconds(1);
        }
        ///count += 50;
        ///yield return new WaitForSeconds(1);
        ///}
        ///while (count < tourList.Count)
        ///{
        ///    for (int i = count; i < 50 + count; i++)
        ///    {
        ///        if (double.Parse(tourList[i].distance) < 1500)
        ///        {
        ///            GameObject go = Instantiate(cardTour, contentTour);
        ///
        ///            tourGOList.Add(go);
        ///
        ///            CardTourInfo cardinfo = tourGOList[i].GetComponent<CardTourInfo>();
        ///            cardinfo.info[0].GetComponent<TextMeshProUGUI>().text = TextBreakTour(tourList[i].title);
        ///            cardinfo.info[1].GetComponent<TextMeshProUGUI>().text = MtoKM(double.Parse(tourList[i].distance));
        ///            if (tourList[i].imageUrl != string.Empty)
        ///            {
        ///                cardinfo.StartCoroutine(nameof(cardinfo.GetTexture), tourList[i].imageUrl);
        ///            }
        ///            else { print(TextBreakTour(tourList[i].title)); }
        ///            cardinfo.SettingTourType(tourList[i].contenttypeid);
        ///            cardinfo.InputTourList(tourList[i]);
        ///        }
        ///    }
        ///
        ///    count += 50;
        ///    yield return new WaitForSeconds(1);
        ///}
        /// GetComponent<ButtonActions>().ChangeBottomSheet(0);
    }

    // 거리 단위 변환
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

    // 텍스트브레이크 - 장소
    string TextBreakPlace(string text)
    {
        string result = string.Empty;
        string[] splitStr = { " " };
        string tmp = text;
        string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

        char[] chars0 = text.ToCharArray();

        // 다섯글자
        if (chars0.Length < 5)
        {
            // 1줄
            if (nameSplit.Length < 2)
            {
                result = nameSplit[0];
            }
            // 2줄
            else
            {
                result = nameSplit[0] + "\n" + nameSplit[1];
            }
        }

        // 여섯글자 이상
        else if (chars0.Length >= 5)
        {
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

    // 텍스트브레이크 - 관광정보
    string TextBreakTour(string text)
    {
        string result = string.Empty;
        string[] splitStr = { " " };
        string tmp = text;
        string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

        char[] chars0 = text.ToCharArray();

        // 다섯글자
        if (chars0.Length < 5)
        {
            // 1줄
            if (nameSplit.Length < 2)
            {
                result = nameSplit[0];
            }
            // 2줄
            else
            {
                result = nameSplit[0] + "\n" + nameSplit[1];
            }
        }

        // 여섯글자 이상
        else if (chars0.Length >= 5)
        {
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

    // 전체 활성화
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

    // 장소 탭 정렬
    public void SortingPlace(string str)
    {
        for (int i = 0; i < placeGOList.Count; i++)
        {

            if (placeGOList[i].GetComponent<CardPlaceInfo>().type.ToString() == str)
            {
                placeGOList[i].gameObject.SetActive(true);
            }
            else
            {
                placeGOList[i].gameObject.SetActive(false);
            }
        }
    }

    // 관광정보 탭 정렬
    public void SortingTour(string str)
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