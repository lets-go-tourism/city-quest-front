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
        // �̹���
        contents[0].GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TourSprites/" + info.contenttypeid);

        // �̸�
        contents[1].gameObject.SetActive(false);
        contents[1].gameObject.SetActive(true);
        contents[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(info.title);

        // �Ÿ�
        //contents[1].GetComponent<TextMeshProUGUI>().text = ConvertDistance(double.Parse(info.distance));
        double latTour = double.Parse(info.latitude);
        double lonTour = double.Parse(info.longitude);

        contents[2].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour));

        // �ּ�
        contents[3].GetComponent<TextMeshProUGUI>().text = info.addr;

        // ��ũ
        //contents[4].GetComponent<OpenKakaoMap>().url = info.url;

        // ����
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

    // ��� �̹��� ��ȯ
    public IEnumerator GetTexture(ServerTourInfo tourInfo)
    {
        // ���� Ű��
        MainView_UI.instance.BackgroundDarkEnable();

        if(tourInfo.imageUrl != string.Empty)
        {
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

                contents[5].GetComponent<RawImage>().texture = myTexture;
                
            }
        }
        TourInfoSetting(tourInfo);
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