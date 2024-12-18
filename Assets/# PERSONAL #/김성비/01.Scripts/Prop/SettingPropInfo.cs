using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using System.Reflection;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;

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

    public PropMovement move;

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
        //print(DataManager.instance.GetQuestInfo().status);
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

    public void SetActiveRendererFeature<T>(bool active) where T : ScriptableRendererFeature
    {
        // URP Asset의 Renderer List에서 0번 인덱스 RendererData 참조
        ScriptableRendererData rendererData = GetRendererData(0);
        if (rendererData == null) return;

        List<ScriptableRendererFeature> rendererFeatures = rendererData.rendererFeatures;
        if (rendererFeatures == null || rendererFeatures.Count <= 0) return;

        for (int i = 0; i < rendererFeatures.Count; i++)
        {
            ScriptableRendererFeature rendererFeature = rendererFeatures[i];
            if (!rendererFeature) continue;
            if (rendererFeature is T) rendererFeature.SetActive(active);
        }
#if UNITY_EDITOR
        rendererData.SetDirty();
#endif
    }

    public ScriptableRendererData GetRendererData(int rendererIndex = 0)
    {
        // 현재 Quality 옵션에 세팅된 URP Asset 참조
        UniversalRenderPipelineAsset pipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (!pipelineAsset) return null;

        // URP Renderer List 리플렉션 참조 (Internal 변수라서 그냥 참조 불가능)
        FieldInfo propertyInfo = pipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
        ScriptableRendererData[] rendererDatas = (ScriptableRendererData[])propertyInfo.GetValue(pipelineAsset);
        if (rendererDatas == null || rendererDatas.Length <= 0) return null;
        if (rendererIndex < 0 || rendererDatas.Length <= rendererIndex) return null;

        return rendererDatas[rendererIndex];
    }

    #region 미탐험 장소 팝업창 세팅
    IEnumerator SettingNO()
    {
        // 렌더링 세팅 변경
        SetActiveRendererFeature<RenderObjects>(true);

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

            move.SettingModeling();
        }
    }

    IEnumerator NOInfoSetting()
    {
        // 3D 모델링, 그림자
        PropModeling.instance.models[(int)DataManager.instance.GetQuestInfo().propNo - 1].transform.rotation = Quaternion.Euler(0, 195, 0);
        PropModeling.instance.ModelingActive((int)DataManager.instance.GetQuestInfo().propNo - 1);
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
        // 렌더링 세팅 변경
        SetActiveRendererFeature<RenderObjects>(false);

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

            move.SettingModeling();
        }
    }

    IEnumerator YESInfoSetting()
    {
        // 3D 모델링, 그림자
        PropModeling.instance.models[(int)DataManager.instance.GetQuestInfo().propNo - 1].transform.rotation = Quaternion.Euler(0, 195, 0);
        PropModeling.instance.ModelingActive((int)DataManager.instance.GetQuestInfo().propNo - 1);
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

        if (DataManager.instance.GetQuestInfo() == null)
        {
            StopCoroutine(GetTexture(null));
        }
        else
        {
            // 장소 사진
            if (DataManager.instance.GetQuestInfo().imageUrl != string.Empty)
            {
                Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 5, "yes");
                yield return StartCoroutine(GetTexture(parameters));
            }
        }
        //}

        //StartCoroutine(nameof(UpdateDistance));
    }
    #endregion

    // 거리 변환
    string ConvertDistance(double distance)
    {
        string result;

        double tmp = distance;

        if (tmp < 1)
        {
            int inttmp = (int)(tmp * 1000);
            result = inttmp.ToString() + "m";
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