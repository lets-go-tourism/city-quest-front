using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BottomSheetManager : MonoBehaviour
{
    // 장소
    public GameObject cardPlace;
    public Transform contentPlace;

    // 관광정보
    public GameObject cardTour;
    public Transform contentTour;

    // 리스트
    List<HomeAdventurePlace> placeList;
    List<HometourPlace> tourList;

    public List<GameObject> placeGOList;
    public List<GameObject> tourGOList;

    public static BottomSheetManager instance;
    private void Awake()
    {
        instance = this;

        placeGOList = new List<GameObject>();
        tourGOList = new List<GameObject>();
    }

    private IEnumerator Start()
    {
        while (DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

        SettingList();
    }

    // 초기 세팅
    public void SettingList()
    {
        placeList = DataManager.instance.GetHomeAdventurePlacesList();
        tourList = DataManager.instance.GetHometourPlacesList();

        // 장소 카드 생성
        for (int i = 0; i < placeList.Count; i++)
        {
            GameObject go = Instantiate(cardPlace, contentPlace);

            placeGOList.Add(go);

            CardPlaceInfo cardinfo = placeGOList[i].GetComponent<CardPlaceInfo>();

            cardinfo.info[0].GetComponent<TextMeshProUGUI>().text = TextBreak(placeList[i].name);
            cardinfo.info[1].GetComponent<TextMeshProUGUI>().text = MtoKM(placeList[i].distance);
            cardinfo.StartCoroutine(nameof(cardinfo.GetTexture), placeList[i].imageUrl);
            cardinfo.SettingPlaceType(placeList[i].status);
        }

        // 관광정보 카드 생성
        for (int i = 0; i < tourList.Count; i++)
        {
            GameObject go = Instantiate(cardTour, contentTour);

            tourGOList.Add(go);

            CardTourInfo cardinfo = tourGOList[i].GetComponent<CardTourInfo>();
            cardinfo.info[0].GetComponent<TextMeshProUGUI>().text = TextBreak(tourList[i].title);
            cardinfo.info[1].GetComponent<TextMeshProUGUI>().text = MtoKM(double.Parse(tourList[i].distance));
            //cardinfo.StartCoroutine(nameof(cardinfo.GetTexture), tourList[i].imageUrl);
            cardinfo.SettingTourType(tourList[i].contenttypeid);
        }

        // 문자열 변환
        string TextBreak(string text)
        {
            string result = string.Empty;

            //// string -> char
            //char[] chars = text.ToCharArray();

            //print(chars);

            // char -> string


            string[] splitStr = { " " };
            string tmp = text;
            string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);


            if (nameSplit.Length > 1)
            {
                result = nameSplit[0] + "\n" + nameSplit[1];
            }
            else
            {
                char[] chars = nameSplit[0].ToCharArray();

                if (chars.Length > 5)
                {
                    if (chars.Length > 8)
                    {
                        string tmp1 = chars[0].ToString() + chars[1].ToString() + chars[2].ToString() + chars[3].ToString() + chars[4].ToString();
                        string tmp2 = chars[5].ToString() + chars[6].ToString() + chars[7].ToString() + chars[8].ToString();

                        result = tmp1 + "\n" + tmp2;
                    }
                    else
                    {
                        string tmp3 = chars[0].ToString() + chars[1].ToString() + chars[2].ToString() + chars[3].ToString();
                        string tmp4 = chars[4].ToString() + chars[5].ToString();

                        result = tmp3 + "\n" + tmp4;
                    }
                }
                else
                {
                    result = nameSplit[0];
                }
            }

            return result;
        }

        // 거리 단위 변환
        string MtoKM(double distance)
        {
            float adjDistance = 0;

            if (distance < 1000)
            {
                adjDistance = (float)distance;
                int tmp = Mathf.FloorToInt(adjDistance);

                adjDistance = tmp;
                return adjDistance.ToString() + "m";
            }
            else
            {
                adjDistance = (float)distance;
                int tmp = Mathf.FloorToInt(adjDistance);

                adjDistance = tmp / 1000;

                return adjDistance.ToString() + "km";
            }
        }
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