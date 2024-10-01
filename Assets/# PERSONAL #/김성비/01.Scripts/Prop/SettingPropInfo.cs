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
        public string type;

        public Parameters(string url, int index, string type)
        {
            this.url = url;
            this.index = index;
            this.type = type;
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
        //MainView_UI.instance.BackgroundDarkEnable();
        print("kdjflskflsdkjflskdj");
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

        if (!PopUpMovement.instance.placeUNCancel)
        {
            yield return new WaitForSeconds(0.2f);
            PopUpMovement.instance.skeleton = false;
            PopUpMovement.instance.rtPlace.anchoredPosition = new Vector2(0, 0);
            PopUpMovement.instance.skPlaceUN.anchoredPosition = new Vector2(0, -2600);

            PopUpMovement.instance.placeADcancel = false;
            PopUpMovement.instance.tourCancel = false;
        }
    }

    IEnumerator NOInfoSetting()
    {
        // 3D 모델링, 그림자
        PropModeling.instance.models[(int)DataManager.instance.GetQuestInfo().propNo - 1].transform.rotation = Quaternion.Euler(0, 180, 0);
        PropModeling.instance.ModelingActive((int)DataManager.instance.GetQuestInfo().propNo - 1, true);
        // 장소명
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName.ToString();
        // 거리
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = ConvertDistance(DataManager.instance.GetQuestInfo().distance).ToString();
        // 장소명
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().addr;
        // 카카오지도 URL
        SettingPropContent.instance.content[3].GetChild(1).GetComponent<OpenPlaceKakaoMap>().SetURL(DataManager.instance.GetQuestInfo().kakaoMapUrl);
        // 장소 사진
        //if (tutorial) // 튜토리얼 
        //{
        //    SettingPropContent.instance.content[4].GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TutorialPlace");
        //}

        //else // 실제
        //{
        if (DataManager.instance.GetQuestInfo().imageUrl != string.Empty)
        {
            Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 4, "no");
            yield return StartCoroutine(nameof(GetTexture), parameters);
        }
        //}
        // 퀘스트
        SettingPropContent.instance.content[6].GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0.184f, 0.114f, 0.024f, 1f);
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

        if (!PopUpMovement.instance.placeADcancel)
        {
            yield return new WaitForSeconds(0.2f);
            PopUpMovement.instance.skeleton = false;
            PopUpMovement.instance.rtPlace.anchoredPosition = new Vector2(0, 0);
            PopUpMovement.instance.skPlaceAD.anchoredPosition = new Vector2(0, -2600);

            PopUpMovement.instance.placeUNCancel = false;
            PopUpMovement.instance.tourCancel = false;
        }
    }

    IEnumerator YESInfoSetting()
    {
        // 3D 모델링, 그림자
        PropModeling.instance.models[(int)DataManager.instance.GetQuestInfo().propNo - 1].transform.rotation = Quaternion.Euler(0, 180, 0);
        PropModeling.instance.ModelingActive((int)DataManager.instance.GetQuestInfo().propNo - 1, false);

        // 장소명
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName.ToString();
        // 방문일자
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = DateTime.Parse(DataManager.instance.GetQuestInfo().date).ToString("MM월 dd일") + " 방문";
        // 장소명
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().addr;
        // 카카오지도
        SettingPropContent.instance.content[3].GetChild(1).GetComponent<OpenPlaceKakaoMap>().SetURL(DataManager.instance.GetQuestInfo().kakaoMapUrl);
        // 사진
        //if (tutorial) // 튜토리얼
        //{
        //    // 퀘스트사진
        //    SettingPropContent.instance.content[4].GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Crop");
        //    // 장소사진
        //    SettingPropContent.instance.content[5].GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TutorialPlace");
        //}
        //else // 실제
        //{
        // 퀘스트 사진
        if (DataManager.instance.GetQuestInfo().questImage != string.Empty)
        {
            Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().questImage, 4, "yes");
            yield return StartCoroutine(GetTexture(parameters));
        }

        // 장소 사진
        if (DataManager.instance.GetQuestInfo().imageUrl != string.Empty)
        {
            Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 5, "yes");
            yield return StartCoroutine(GetTexture(parameters));
        }
        //}

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
            result = (int)tmp + "m";
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
            //Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D myTexture = DownloadHandlerTexture.GetContent(www);

            int originW = myTexture.width;
            int originH = myTexture.height;

            // 탐험 완
            if (raw.type == "yes")
            {
                if (raw.index == 4) // 퀘스트 사진
                {
                    //FinalScale(686, 686);

                    SettingPropContent.instance.content[raw.index].GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(myTexture, new Rect(0, 0, originW, originH), new Vector2(0.5f, 0.5f));
                }
                else if (raw.index == 5) // 장소사진
                {
                    //FinalScale(700, 450);

                    SettingPropContent.instance.content[raw.index].GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(myTexture, new Rect(0, 0, originW, originH), new Vector2(0.5f, 0.5f));
                }
            }
            // 미탐험
            else if (raw.type == "no")
            {
                SettingPropContent.instance.content[raw.index].GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(myTexture, new Rect(0, 0, originW, originH), new Vector2(0.5f, 0.5f));
            }
        }
    }
}