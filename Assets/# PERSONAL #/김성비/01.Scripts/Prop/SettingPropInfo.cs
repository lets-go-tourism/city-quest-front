using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class SettingPropInfo : MonoBehaviour
{
    // 퀘스트 사진 파라미터
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

    // 팝업창
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


    #region 미탐험 장소 팝업창 세팅
    IEnumerator SettingNO()
    {
        // 프랍 생성
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingNO));

        // 정보값 적용
        yield return StartCoroutine(nameof(NOInfoSetting));

        // UI
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
    }

    IEnumerator NOInfoSetting()
    {
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName.ToString();
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = ConvertDistance(DataManager.instance.GetQuestInfo().distance).ToString();
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

    #region 탐험완료 장소 팝업창
    IEnumerator SettingYES()
    {
        // 프랍 생성
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingYES));

        // 정보값 적용
        yield return StartCoroutine(nameof(YESInfoSetting));

        // UI
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
    }

    IEnumerator YESInfoSetting()
    {
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName.ToString();

        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = DateTime.Parse(DataManager.instance.GetQuestInfo().date).ToString("MM월 dd일"); 

        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().addr;
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

    // 거리 변환
    string ConvertDistance(double distance)
    {
        string result = string.Empty;

        double tmp = distance;
        
        if (tmp < 1)
        {
            tmp = distance * 100;
            result = (int)tmp  + "m";
        }
        else
        {
            tmp = (int)distance;
            result = tmp.ToString() + "km";
        }

        return result;
    }

    // 이미지 불러오기
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

    #region 미사용
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
    //    SettingPropContent.instance.content[1].GetComponent<RectTransform>().sizeDelta = new Vector2(840, delta);
    //}
    // �Ÿ� ���
    #endregion
}