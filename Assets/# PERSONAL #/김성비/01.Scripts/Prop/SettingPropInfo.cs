using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPropInfo : MonoBehaviour
{
    public static SettingPropInfo instance;
    QuestData questData;
    public List<QuestData> quests;

    private void Awake()
    {
        instance = this;
    }

    // 수정할 것
    public void PropInfoSetting(Prop prop)
    {
        KJY_ConnectionTMP.instance.OnConnectionQuest((int)prop.PropData.propNo);
        questData = DataManager.instance.GetQuestInfo();
        quests = new List<QuestData>();

        print(questData);

        if (prop.HomeAdventurePlaceData.status)
        {
            StopCoroutine(nameof(SettingNO));
            StartCoroutine(nameof(SettingYES), prop);
        }
        else
        {
            StopCoroutine(nameof(SettingYES));
            StartCoroutine(nameof(SettingNO), prop);
        }
    }

    IEnumerator SettingNO(Prop prop)
    {
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingNO));

        // 데이터 세팅 : 모델링, 장소명, 거리, 주소명, 장소사진, (구분선,) 퀘스트 => 6가지
        //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = prop.GetComponent<MeshRenderer>().material;
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = prop.PropData.name.ToString();
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = "99" + "km";
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = "경기 00시 00구 00로 88";
        SettingPropContent.instance.content[4].GetChild(0).GetComponent<Image>().sprite = null;
        SettingPropContent.instance.content[6].GetChild(0).GetComponent<TextMeshProUGUI>().text = "어떤 퀘스트일까요";

        // UI 활성화
        Props_UI.instance.canvasProp.enabled = true;
        // 3D 모델링        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);
    }


    IEnumerator SettingYES(Prop prop)
    {
        // 컨텐트 생성
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingYES));

        // 데이터 세팅 : 모델링, 장소명, 방문날짜, 주소명, 퀘스트사진, 장소사진 => 6가지
        //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = prop.GetComponent<MeshRenderer>().material;
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = prop.PropData.name.ToString();
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = "0월 0일 방문";
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = "경기 00시 00구 00로 00";
        SettingPropContent.instance.content[4].GetChild(0).GetComponent<Image>().sprite = null;
        SettingPropContent.instance.content[5].GetChild(0).GetComponent<Image>().sprite = null;

        // UI 활성화
        Props_UI.instance.canvasProp.enabled = true;

        // 3D 모델링        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);
    }
}