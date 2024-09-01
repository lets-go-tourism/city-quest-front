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

    // ������ ��
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

        // ������ ���� : �𵨸�, ��Ҹ�, �Ÿ�, �ּҸ�, ��һ���, (���м�,) ����Ʈ => 6����
        //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = prop.GetComponent<MeshRenderer>().material;
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = prop.PropData.name.ToString();
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = "99" + "km";
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = "��� 00�� 00�� 00�� 88";
        SettingPropContent.instance.content[4].GetChild(0).GetComponent<Image>().sprite = null;
        SettingPropContent.instance.content[6].GetChild(0).GetComponent<TextMeshProUGUI>().text = "� ����Ʈ�ϱ��";

        // UI Ȱ��ȭ
        Props_UI.instance.canvasProp.enabled = true;
        // 3D �𵨸�        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);
    }


    IEnumerator SettingYES(Prop prop)
    {
        // ����Ʈ ����
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingYES));

        // ������ ���� : �𵨸�, ��Ҹ�, �湮��¥, �ּҸ�, ����Ʈ����, ��һ��� => 6����
        //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = prop.GetComponent<MeshRenderer>().material;
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = prop.PropData.name.ToString();
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = "0�� 0�� �湮";
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = "��� 00�� 00�� 00�� 00";
        SettingPropContent.instance.content[4].GetChild(0).GetComponent<Image>().sprite = null;
        SettingPropContent.instance.content[5].GetChild(0).GetComponent<Image>().sprite = null;

        // UI Ȱ��ȭ
        Props_UI.instance.canvasProp.enabled = true;

        // 3D �𵨸�        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);
    }
}