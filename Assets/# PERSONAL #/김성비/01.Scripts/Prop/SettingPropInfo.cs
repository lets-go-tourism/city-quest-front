using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class SettingPropInfo : MonoBehaviour
{
    public class Parameters
    {
        public string url;
        public int index;

        public Parameters(string url, int index)
        {
            this.url = url;
            this.index = index;
        }
    }

    QuestData questData;

    public static SettingPropInfo instance;
    private void Awake()
    {
        instance = this;
    }

    // ���� ��� �޾ƿͼ� ���� ����
    public void PropInfoSetting()
    {
        MainView_UI.instance.BackgroundDarkEnable();

        if (DataManager.instance.GetQuestInfo().status)
        {
            StopCoroutine(SettingNO());
            StartCoroutine(SettingYES());
        }
        else
        {
            StopCoroutine(SettingYES());
            StartCoroutine(SettingNO());
        }
    }


    #region ��Ž�� ��� ������ ����
    IEnumerator SettingNO()
    {
        // ����Ʈ �����
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingNO));

        yield return StartCoroutine(nameof(NOInfoSetting));

        // �˾�â ����
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
    }

    IEnumerator NOInfoSetting()
    {
        // ������ ���� : (�𵨸�,) ��Ҹ�, �Ÿ�, �ּҸ�, ��ũ, ��һ���, (���м�,) ����Ʈ => 6����
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(DataManager.instance.GetQuestInfo().locationName);
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = ConvertDistance(DataManager.instance.GetQuestInfo().distance);
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().addr;
        SettingPropContent.instance.content[3].GetChild(1).GetComponent<OpenKakaoMap>().url = DataManager.instance.GetQuestInfo().kakaoMapUrl;
        if (DataManager.instance.GetQuestInfo().imageUrl != string.Empty)
        {
            Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 4);
            yield return StartCoroutine(nameof(GetTexture), parameters);
        }
        SettingPropContent.instance.content[6].GetChild(1).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().questDesc;
        SettingPropContent.instance.content[6].GetChild(0).GetComponent<Image>().sprite = SettingPropContent.instance.content[6].GetComponent<SpritesHolder>().sprites[0];
        SettingPropContent.instance.content[6].GetChild(0).GetComponent<PictureQuest>().propnumber = (int)DataManager.instance.GetQuestInfo().propNo;
        SettingPropContent.instance.content[6].GetChild(0).GetComponent<Button>().enabled = true;
    }
    #endregion

    #region Ž���� ��� ������ ����
    IEnumerator SettingYES()
    {
        // ����Ʈ ����
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingYES));

        yield return StartCoroutine(nameof(YESInfoSetting));

        // �˾�â ����
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
    }

    IEnumerator YESInfoSetting()
    {
        // ������ ���� : �𵨸�, ��Ҹ�, �湮��¥, �ּҸ�, ����Ʈ����, ��һ��� => 6����
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(DataManager.instance.GetQuestInfo().locationName);
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().date.ToString("MM�� dd��");
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(DataManager.instance.GetQuestInfo().addr);
        SettingPropContent.instance.content[3].GetChild(1).GetComponent<OpenKakaoMap>().url = DataManager.instance.GetQuestInfo().kakaoMapUrl;
        if (DataManager.instance.GetQuestInfo().questImage != string.Empty)
        {
            Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().questImage, 4);
            yield return StartCoroutine(GetTexture(parameters));
        }
        if (DataManager.instance.GetQuestInfo().imageUrl != string.Empty)
        {
            Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 5);
            yield return StartCoroutine(GetTexture(parameters));
        }
    }
    #endregion

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
        SettingPropContent.instance.content[1].GetComponent<RectTransform>().sizeDelta = new Vector2(840, delta);
    }

    // �Ÿ� ���
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


    // �������� ���� �޾ƿ���
    public IEnumerator GetTexture(Parameters raw)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(raw.url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            SettingPropContent.instance.content[raw.index].GetChild(0).GetComponent<RawImage>().texture = myTexture;
        }
    }
}