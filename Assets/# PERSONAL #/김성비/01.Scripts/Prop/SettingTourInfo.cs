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
        // �̹���
        contents[0].GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TourSprites/" + info.contenttypeid);
        // �̸�
        contents[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = info.title.ToString();
        // �Ÿ�
        latTour = double.Parse(info.latitude);   // ����
        lonTour = double.Parse(info.longitude);  // �浵
        contents[2].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour));    // �ǽð� �Ÿ�
        // �ּ�
        contents[3].GetComponent<TextMeshProUGUI>().text = info.addr;
        // ��ũ
        contents[4].GetComponent<OpenTourKakaoMap>().SetURL(info.addr);

        // ��һ���
        // url ���� ��
        if (info.imageUrl != string.Empty)
        {
            contents[5].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);            // �̹��� ��� ���
            contents[6].GetComponent<TextMeshProUGUI>().enabled = false;                    // �ȳ��� ��Ȱ��
        }
        // url ���� ��
        else
        {
            SettingSize(540f);                                                              // ������ ����
            contents[5].GetComponent<Image>().sprite = null;                                // �̹��� �ʱ�ȭ
            contents[5].GetComponent<Image>().color = new Color(1f, 0.98f, 0.96f, 1f);      // �� �ٲٱ�
            contents[6].GetComponent<TextMeshProUGUI>().enabled = true;                     // �ȳ��� Ȱ��ȭ
        }

        // �˾�â UI Ȱ��ȭ
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), false);

        //StartCoroutine(nameof(UpdateDistance));
    }

    void SettingSize(float CT)
    {
        float contentY = CT;

        // �̹��� ��Ȱ��ȭ
        contents[7].gameObject.SetActive(false);
        // parent 
        contents[7].GetComponent<RectTransform>().sizeDelta = new Vector2(840, contentY + 36);
        // �������� �̹��� ũ��
        contents[5].GetComponent<RectTransform>().sizeDelta = new Vector2(840, contentY);
        // �̹��� Ȱ��ȭ
        contents[7].gameObject.SetActive(true);

        // ��ũ�Ѻ� ũ��
        float scrollY = 720f + contentY - 60f;
        if(scrollY > 1608)
        {
            scrollY = 1608f;
        }
        PopUpMovement.instance.rtTour.sizeDelta = new Vector2(960, scrollY);
        // top ����ũ ũ��
        Props_UI.instance.masks[0].GetComponent<RectTransform>().sizeDelta = new Vector2(960, scrollY);
        // top ����ũ ä���
        Props_UI.instance.masks[0].fillAmount = 60f / scrollY;
        // bottom ����ũ ũ��
        Props_UI.instance.masks[1].GetComponent<RectTransform>().sizeDelta = new Vector2(960, scrollY);
        // bottom ����ũ ä���
        Props_UI.instance.masks[1].fillAmount = 60f / scrollY;
    }


    //public IEnumerator UpdateDistance()
    //{
    //    int t = 0;
    //    while(t == 0)
    //    {
    //        contents[2].GetComponent<TextMeshProUGUI>().text = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(latTour, lonTour));    // �ǽð� �Ÿ�

    //        yield return new WaitForSeconds(5);
    //    }
    //}

    #region ��ȯ
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

        if (tourInfo.imageUrl != string.Empty)
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
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                ProcessImage(texture);
            }
        }

        TourInfoSetting(tourInfo);
    }

    private void ProcessImage(Texture2D texture)
    {
        // ���� �̹��� ũ��
        float originalWidth = texture.width;
        float originalHeight = texture.height;

        // ���� �̹��� ���� ���
        float originalAspect = originalWidth / originalHeight;

        // ��ǥ ���� ���̿� ���� ���� ���� ���
        float targetWidth = 840;
        float targetHeight = targetWidth / originalAspect;

        // UI.Image�� �ؽ�ó ����
        if (contents[5].GetComponent<Image>() != null)
        {
            contents[5].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, originalWidth, originalHeight), new Vector2(0.5f, 0.5f));
        }

        SettingSize(targetHeight);

        print(contents[0].transform.parent.GetComponent<RectTransform>().sizeDelta.y);

        //// RectTransform�� ������ ����
        //contents[7].gameObject.SetActive(false);                                        // �̹��� ��Ȱ��ȭ
        
        //if (contents[7].GetComponent<RectTransform>() != null)
        //{
        //    contents[5].GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, targetHeight);
        //    contents[7].GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, targetHeight + 36);
        //}

        //contents[7].gameObject.SetActive(true);
    }
    #endregion

    #region �̻��
    //// ��Ҹ� �ڸ���
    //string TextBreak(string str)
    //{
    //    string result = string.Empty;
    //    string[] splitStr = { " " };
    //    string tmp = str;
    //    string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

    //    char[] chars0 = str.ToCharArray();

    //    // �ټ�����
    //    if (chars0.Length < 5)
    //    {
    //        // 1��
    //        if (nameSplit.Length < 2)
    //        {
    //            ChangeSizeDelta(132);
    //            result = nameSplit[0];
    //        }
    //        // 2��
    //        else
    //        {
    //            ChangeSizeDelta(240);
    //            result = nameSplit[0] + "\n" + nameSplit[1];
    //        }
    //    }

    //    // �������� �̻�
    //    else if (chars0.Length >= 5)
    //    {
    //        ChangeSizeDelta(240);

    //        // ���� ���� ���
    //        if (nameSplit.Length < 2)
    //        {
    //            result = nameSplit[0];
    //        }

    //        // ���� �ִ� ���
    //        else
    //        {
    //            char[] chars1 = nameSplit[1].ToCharArray(); // �ι�° �� ���������   

    //            // �ι�° ���� 5���� ����
    //            if (chars1.Length < 5)
    //            {
    //                result = nameSplit[0] + "\n" + nameSplit[1]; // �ι�° �� �ټ����� ����
    //            }
    //            // �ι�° ���� 6���� �̻�
    //            else
    //            {
    //                result = nameSplit[0] + "\n" + chars1[0] + chars1[1] + chars1[2] + chars1[3] + "..."; // �ι�° �� �ټ����� �ʰ�
    //            }
    //        }
    //    }

    //    return result;
    //}

    // ��Ҹ� SizeDelta ũ�� ����
    //void ChangeSizeDelta(int delta)
    //{
    //    contents[0].GetComponent<RectTransform>().sizeDelta = new Vector2(840, delta);
    //}
    #endregion
}