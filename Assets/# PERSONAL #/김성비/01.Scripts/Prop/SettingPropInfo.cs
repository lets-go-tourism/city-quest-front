using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

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


    // 수정할 것
    //public void PropInfoSetting(Prop prop)
    public IEnumerator PropInfoSetting()
    {
        while(DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

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


    // 미탐험 장소 데이터 세팅
    IEnumerator SettingNO()
    {
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingNO));


        // 데이터 세팅 : 모델링, 장소명, 거리, 주소명, 장소사진, (구분선,) 퀘스트 => 6가지
        //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = prop.GetComponent<MeshRenderer>().material;
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName;
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().distance.ToString(); 
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(DataManager.instance.GetQuestInfo().addr);
        Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 4);
        yield return StartCoroutine(nameof(GetTexture), parameters);
        SettingPropContent.instance.content[6].GetChild(1).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().questDesc;

        // UI 활성화
        Props_UI.instance.canvasProp.enabled = true;
        // 3D 모델링        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);
    }

    // 탐험한 장소 데이터 세팅
    IEnumerator SettingYES()
    {
        // 컨텐트 생성
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingYES));

        //print(DataManager.instance.GetQuestInfo().locationName);

        // 데이터 세팅 : 모델링, 장소명, 방문날짜, 주소명, 퀘스트사진, 장소사진 => 6가지
        //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = prop.GetComponent<MeshRenderer>().material;
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName;
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().date.ToString("MM월 dd일");
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(DataManager.instance.GetQuestInfo().addr);
        Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().questImage, 4);
        yield return StartCoroutine(GetTexture(parameters));
        parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 5);
        yield return StartCoroutine(GetTexture(parameters));

        // UI 활성화
        Props_UI.instance.canvasProp.enabled = true;

        // 3D 모델링        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);
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

    // 장소/주소명 자르기
    string TextBreak(string str)
    {
        string result = string.Empty;
        string[] splitStr = { " " };
        string tmp = str;
        string[] nameSplit = tmp.Split(splitStr, 2, StringSplitOptions.RemoveEmptyEntries);


        if (nameSplit.Length > 1)
        {
            result = nameSplit[0] + "\n" + nameSplit[1];
        }

        return result;
    }
}