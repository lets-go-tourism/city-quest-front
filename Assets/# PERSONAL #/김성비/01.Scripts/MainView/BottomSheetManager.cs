using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BottomSheetManager : MonoBehaviour
{
    // ���
    public GameObject cardPlace;
    public Transform contentPlace;

    // ��������
    public GameObject cardTour;
    public Transform contentTour;

    // ����Ʈ
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

    //private void Update()
    //{
        
    //}

    private IEnumerator Start()
    {
        while (DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

        StartCoroutine(SettingList());
    }

    // �ʱ� ����
    public IEnumerator SettingList()
    {
        placeList = DataManager.instance.GetHomeAdventurePlacesList();
        tourList = DataManager.instance.GetHometourPlacesList();

        // ��� ī�� ����
        for (int i = 0; i < placeList.Count; i++)
        {
            GameObject go = Instantiate(cardPlace, contentPlace);

            placeGOList.Add(go);

            CardPlaceInfo cardinfo = placeGOList[i].GetComponent<CardPlaceInfo>();

            cardinfo.info[0].GetComponent<TextMeshProUGUI>().text = TextBreakPlace(placeList[i].name);
            cardinfo.info[1].GetComponent<TextMeshProUGUI>().text = MtoKM(placeList[i].distance);
            cardinfo.StartCoroutine(nameof(cardinfo.GetTexture), placeList[i].imageUrl);
            cardinfo.SettingPlaceType(placeList[i].status);
            cardinfo.SetServerProp(placeList[i]);
        }

        yield return StartCoroutine(GenTour());

        yield return null;
    }

    IEnumerator GenTour()
    {
        // �������� ī�� ����
        int count = 0;

        //while (count < tourList.Count)
        //{
        for (int i = count; i < 50 + count; i++)
        {
            GameObject go = Instantiate(cardTour, contentTour);

            tourGOList.Add(go);

            CardTourInfo cardinfo = tourGOList[i].GetComponent<CardTourInfo>();
            cardinfo.info[0].GetComponent<TextMeshProUGUI>().text = TextBreakTour(tourList[i].title);
            cardinfo.info[1].GetComponent<TextMeshProUGUI>().text = MtoKM(double.Parse(tourList[i].distance));
            if (tourList[i].imageUrl != string.Empty)
            {
                cardinfo.info[2].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
                cardinfo.info[3].GetComponent<TextMeshProUGUI>().enabled = false;
                cardinfo.StartCoroutine(nameof(cardinfo.GetTexture), tourList[i].imageUrl);
            }
            else 
            {
                cardinfo.info[2].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
                cardinfo.info[3].GetComponent<TextMeshProUGUI>().enabled = true;
            }
            cardinfo.SettingTourType(tourList[i].contenttypeid);
            cardinfo.InputTourList(tourList[i]);
        }

        yield return null;
        //count += 50;
        //yield return new WaitForSeconds(1);
        //}

        //while (count < tourList.Count)
        //{
        //    for (int i = count; i < 50 + count; i++)
        //    {
        //        if (double.Parse(tourList[i].distance) < 1500)
        //        {
        //            GameObject go = Instantiate(cardTour, contentTour);

        //            tourGOList.Add(go);

        //            CardTourInfo cardinfo = tourGOList[i].GetComponent<CardTourInfo>();
        //            cardinfo.info[0].GetComponent<TextMeshProUGUI>().text = TextBreakTour(tourList[i].title);
        //            cardinfo.info[1].GetComponent<TextMeshProUGUI>().text = MtoKM(double.Parse(tourList[i].distance));
        //            if (tourList[i].imageUrl != string.Empty)
        //            {
        //                cardinfo.StartCoroutine(nameof(cardinfo.GetTexture), tourList[i].imageUrl);
        //            }
        //            else { print(TextBreakTour(tourList[i].title)); }
        //            cardinfo.SettingTourType(tourList[i].contenttypeid);
        //            cardinfo.InputTourList(tourList[i]);
        //        }
        //    }

        //    count += 50;
        //    yield return new WaitForSeconds(1);
        //}
        // GetComponent<ButtonActions>().ChangeBottomSheet(0);
    }

    // �Ÿ� ���� ��ȯ
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

    // �ؽ�Ʈ�극��ũ - ���
    string TextBreakPlace(string text)
    {
        string result = string.Empty;
        string[] splitStr = { " " };
        string tmp = text;
        string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

        char[] chars0 = text.ToCharArray();

        // �ټ�����
        if (chars0.Length < 5)
        {
            // 1��
            if (nameSplit.Length < 2)
            {
                result = nameSplit[0]; 
            }
            // 2��
            else
            {
                result = nameSplit[0] + "\n" + nameSplit[1];
            }
        }

        // �������� �̻�
        else if (chars0.Length >= 5) 
        {
            // ���� ���� ���
            if (nameSplit.Length < 2) 
            {
                result = nameSplit[0];
            }

            // ���� �ִ� ���
            else
            {
                char[] chars1 = nameSplit[1].ToCharArray(); // �ι�° �� ���������

                // �ι�° ���� 5���� ����
                if (chars1.Length < 5)
                {
                    result = nameSplit[0] + "\n" + nameSplit[1]; // �ι�° �� �ټ����� ����
                }
                // �ι�° ���� 6���� �̻�
                else
                {
                    result = nameSplit[0] + "\n" + chars1[0] + chars1[1] + chars1[2] + chars1[3] + "..."; // �ι�° �� �ټ����� �ʰ�
                }   
            }
        }

        return result;
    }

    // �ؽ�Ʈ�극��ũ - ��������
    string TextBreakTour(string text)
    {
        string result = string.Empty;
        string[] splitStr = { " " };
        string tmp = text;
        string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

        char[] chars0 = text.ToCharArray();

        // �ټ�����
        if (chars0.Length < 5)
        {
            // 1��
            if (nameSplit.Length < 2)
            {
                result = nameSplit[0];
            }
            // 2��
            else
            {
                result = nameSplit[0] + "\n" + nameSplit[1];
            }
        }

        // �������� �̻�
        else if (chars0.Length >= 5)
        {
            // ���� ���� ���
            if (nameSplit.Length < 2)
            {
                result = nameSplit[0];
            }

            // ���� �ִ� ���
            else
            {
                char[] chars1 = nameSplit[1].ToCharArray(); // �ι�° �� ���������

                // �ι�° ���� 5���� ����
                if (chars1.Length < 5)
                {
                    result = nameSplit[0] + "\n" + nameSplit[1]; // �ι�° �� �ټ����� ����
                }
                // �ι�° ���� 6���� �̻�
                else
                {
                    result = nameSplit[0] + "\n" + chars1[0] + chars1[1] + chars1[2] + chars1[3] + "..."; // �ι�° �� �ټ����� �ʰ�
                }
            }
        }

        return result;
    }

    // ��ü Ȱ��ȭ
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

    // ��� �� ����
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

    // �������� �� ����
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