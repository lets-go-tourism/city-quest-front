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
        SettingBGColor(int.Parse(info.contenttypeid));
        contents[0].GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TourSprites/" + info.contenttypeid);
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
        if (info.imageUrl != string.Empty)                                                  // url ���� ��
        {
            contents[7].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);            // �̹��� ��� ���
            contents[8].GetComponent<TextMeshProUGUI>().enabled = false;                    // �ȳ��� ��Ȱ��
        }

        else                                                                                // url ���� ��
        {
            SettingSize(540f);                                                              // ������ ����
            contents[7].GetComponent<Image>().sprite = null;                                // �̹��� �ʱ�ȭ
            contents[7].GetComponent<Image>().color = new Color(1f, 0.882f, 0.624f, 1f);      // �� �ٲٱ�
            contents[8].GetComponent<TextMeshProUGUI>().enabled = true;                     // �ȳ��� Ȱ��ȭ
        }
    }

    void SettingSize(float CT)
    {
        float contentY = CT;
        // �̹��� ��Ȱ��ȭ
        contents[5].gameObject.SetActive(false);
        // �̹���
        contents[5].GetComponent<RectTransform>().sizeDelta = new Vector2(840, contentY + 36);          // ����Ʈ ũ��
        contents[6].GetComponent<RectTransform>().sizeDelta = new Vector2(840, contentY);               // ����ũ ũ��     
        contents[7].GetComponent<RectTransform>().sizeDelta = new Vector2(840, contentY);               // �̹��� ũ��
        // ��ũ�Ѻ�
        float scrollY = 720f + contentY - 54f;
        if (scrollY > 1500) scrollY = 1500f;
        PopUpMovement.instance.rtTour.sizeDelta = new Vector2(960, scrollY);                            // ��ũ�Ѻ� ũ��  
        Props_UI.instance.masks[0].GetComponent<RectTransform>().sizeDelta = new Vector2(960, scrollY); // top ����ũ ũ��        
        Props_UI.instance.masks[0].fillAmount = 60f / scrollY;                                          // top ����ũ ä���        
        Props_UI.instance.masks[1].GetComponent<RectTransform>().sizeDelta = new Vector2(960, scrollY); // bottom ����ũ ũ��        
        Props_UI.instance.masks[1].fillAmount = 60f / scrollY;                                          // bottom ����ũ ä���
        // �̹��� Ȱ��ȭ
        contents[5].gameObject.SetActive(true);
    }

    #region ��ȯ
    // ������ ���� ��ȯ
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

    // �Ÿ� ��ȯ
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

        // �˾�â UI Ȱ��ȭ
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
        // ���� �̹��� ũ��
        float originalWidth = texture.width;
        float originalHeight = texture.height;

        // ���� �̹��� ���� ���
        float originalAspect = originalWidth / originalHeight;

        // ��ǥ ���� ���̿� ���� ���� ���� ���
        float targetWidth = 840;
        float targetHeight = targetWidth / originalAspect;

        // UI.Image�� �ؽ�ó ����
        if (contents[7].GetComponent<Image>() != null)
        {
            contents[7].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, originalWidth, originalHeight), new Vector2(0.5f, 0.5f));
        }

        SettingSize(targetHeight);
    }
    #endregion
}