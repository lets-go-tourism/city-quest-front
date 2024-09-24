using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TriangleNet.Geometry;

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
        // 3D 모델링
        PropModeling.instance.models[(int)DataManager.instance.GetQuestInfo().propNo - 1].transform.rotation = Quaternion.Euler(0, 180, 0);
        PropModeling.instance.ModelingActive((int)DataManager.instance.GetQuestInfo().propNo - 1);
        // 그림자
        PropModeling.instance.Cloud.SetActive(true);
        // 장소명
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName.ToString();
        // 거리
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = ConvertDistance(DataManager.instance.GetQuestInfo().distance).ToString();
        // 장소명
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().addr;
        // 카카오지도 URL
        SettingPropContent.instance.content[3].GetChild(1).GetComponent<OpenPlaceKakaoMap>().SetURL(DataManager.instance.GetQuestInfo().kakaoMapUrl);
        // 장소 사진
        if (DataManager.instance.GetQuestInfo().imageUrl != string.Empty)
        {
            Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 4);
            yield return StartCoroutine(nameof(GetTexture), parameters);
        }
        // 퀘스트
        SettingPropContent.instance.content[6].GetChild(1).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().questDesc;
        // 퀘스트 배경이미지
        SettingPropContent.instance.content[6].GetChild(0).GetComponent<Image>().sprite = SettingPropContent.instance.content[6].GetComponent<SpritesHolder>().sprites[0];
        // 프랍 넘버
        SettingPropContent.instance.content[6].GetChild(0).GetComponent<PictureQuest>().propnumber = (int)DataManager.instance.GetQuestInfo().propNo;
        // 터치 가능
        SettingPropContent.instance.content[6].GetChild(0).GetComponent<Button>().enabled = true;

        //StartCoroutine(nameof(UpdateDistance));
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
        // 3D 모델링
        PropModeling.instance.models[(int)DataManager.instance.GetQuestInfo().propNo - 1].transform.rotation = Quaternion.Euler(0, 180, 0);
        PropModeling.instance.ModelingActive((int)DataManager.instance.GetQuestInfo().propNo - 1);
        // 그림자
        PropModeling.instance.Cloud.SetActive(false);
        // 장소명
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName.ToString();
        // 방문일자
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = DateTime.Parse(DataManager.instance.GetQuestInfo().date).ToString("MM월 dd일"); 
        // 장소명
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().addr;
        // 카카오지도
        SettingPropContent.instance.content[3].GetChild(1).GetComponent<OpenPlaceKakaoMap>().SetURL(DataManager.instance.GetQuestInfo().kakaoMapUrl);
        // 퀘스트 사진
        if (DataManager.instance.GetQuestInfo().questImage != string.Empty)
        {
            Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().questImage, 4);
            yield return StartCoroutine(GetTexture(parameters));
        }
        // 장소 사진
        if (DataManager.instance.GetQuestInfo().imageUrl != string.Empty)
        {
            Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 5);
            yield return StartCoroutine(GetTexture(parameters));
        }

        //StartCoroutine(nameof(UpdateDistance));
    }
    #endregion

    //public IEnumerator UpdateDistance()
    //{
    //    int t = 0;
    //    while (t == 0)
    //    {
    //        // 실시간 거리
    //        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = ;    

    //        yield return new WaitForSeconds(5);
    //    }
    //}


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
}