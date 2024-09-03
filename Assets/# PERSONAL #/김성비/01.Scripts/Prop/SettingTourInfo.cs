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
        // �̸�
        contents[0].gameObject.SetActive(false);
        contents[0].gameObject.SetActive(true);
        contents[0].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(info.title);

        // �Ÿ�
        //contents[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(double.Parse(info.distance));
        double latTour = double.Parse(info.latitude);
        double lonTour = double.Parse(info.longitude);

        GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour);

        contents[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour));

        // �ּ�
        contents[2].GetComponent<TextMeshProUGUI>().text = info.addr;

        // ��ũ
        //contents[4].GetComponent<OpenKakaoMap>().url = info.url;

        // ����
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

        // �˾�â UI Ȱ��ȭ
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), false);
    }

    // ��Ҹ� �ڸ���
    string TextBreak(string str)
    {
        string result = string.Empty;
        string[] splitStr = { " " };
        string tmp = str;
        string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

        char[] chars0 = str.ToCharArray();

        // �ټ�����
        if (chars0.Length < 5)
        {
            // 1��
            if (nameSplit.Length < 2)
            {
                ChangeSizeDelta(132);
                result = nameSplit[0];
            }
            // 2��
            else
            {
                ChangeSizeDelta(240);
                result = nameSplit[0] + "\n" + nameSplit[1];
            }
        }

        // �������� �̻�
        else if (chars0.Length >= 5)
        {
            ChangeSizeDelta(240);

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

    // ��Ҹ� SizeDelta ũ�� ����
    void ChangeSizeDelta(int delta)
    {
        contents[0].GetComponent<RectTransform>().sizeDelta = new Vector2(840, delta);
    }

    // �Ÿ� ��ȯ
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

    // ��� �̹��� ��ȯ
    public IEnumerator GetTexture(ServerTourInfo tourInfo)
    {
        // ���� Ű��
        MainView_UI.instance.BackgroundDarkEnable();

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(tourInfo.imageUrl);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

            // ���� ����
            MainView_UI.instance.BackgroundDarkDisable();
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            contents[4].GetComponent<RawImage>().texture = myTexture;
            TourInfoSetting(tourInfo);
        }
    }

    // ���� ��ȯ
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