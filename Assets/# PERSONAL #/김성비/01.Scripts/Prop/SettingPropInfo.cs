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

    // 서버 통신 받아와서 세팅 시작
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


    #region 미탐험 장소 데이터 세팅
    IEnumerator SettingNO()
    {
        // 컨텐트 만들기
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingNO));

        yield return StartCoroutine(nameof(NOInfoSetting));

        // 팝업창 열기
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
    }

    IEnumerator NOInfoSetting()
    {
        // 데이터 세팅 : (모델링,) 장소명, 거리, 주소명, 링크, 장소사진, (구분선,) 퀘스트 => 6가지
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

    #region 탐험한 장소 데이터 세팅
    IEnumerator SettingYES()
    {
        // 컨텐트 생성
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingYES));

        yield return StartCoroutine(nameof(YESInfoSetting));

        // 팝업창 열기
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
    }

    IEnumerator YESInfoSetting()
    {
        // 데이터 세팅 : 모델링, 장소명, 방문날짜, 주소명, 퀘스트사진, 장소사진 => 6가지
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(DataManager.instance.GetQuestInfo().locationName);
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().date.ToString("MM월 dd일");
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

    // 장소명 자르기
    string TextBreak(string str)
    {
        string result = string.Empty;
        string[] splitStr = { " " };
        string tmp = str;
        string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);

        char[] chars0 = str.ToCharArray();

        // 다섯글자
        if (chars0.Length < 5)
        {
            // 1줄
            if (nameSplit.Length < 2)
            {
                ChangeSizeDelta(132);
                result = nameSplit[0];
            }
            // 2줄
            else
            {
                ChangeSizeDelta(240);
                result = nameSplit[0] + "\n" + nameSplit[1];
            }
        }

        // 여섯글자 이상
        else if (chars0.Length >= 5)
        {
            ChangeSizeDelta(240);

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

    // 장소명 SizeDelta 크기 조절
    void ChangeSizeDelta(int delta)
    {
        SettingPropContent.instance.content[1].GetComponent<RectTransform>().sizeDelta = new Vector2(840, delta);
    }

    // 거리 계산
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


    // 서버에서 사진 받아오기
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