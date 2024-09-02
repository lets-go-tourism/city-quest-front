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


    // ������ ��
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


    // ��Ž�� ��� ������ ����
    IEnumerator SettingNO()
    {
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingNO));


        // ������ ���� : �𵨸�, ��Ҹ�, �Ÿ�, �ּҸ�, ��һ���, (���м�,) ����Ʈ => 6����
        //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = prop.GetComponent<MeshRenderer>().material;
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName;
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().distance.ToString(); 
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(DataManager.instance.GetQuestInfo().addr);
        Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 4);
        yield return StartCoroutine(nameof(GetTexture), parameters);
        SettingPropContent.instance.content[6].GetChild(1).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().questDesc;

        // UI Ȱ��ȭ
        Props_UI.instance.canvasProp.enabled = true;
        // 3D �𵨸�        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);
    }

    // Ž���� ��� ������ ����
    IEnumerator SettingYES()
    {
        // ����Ʈ ����
        yield return SettingPropContent.instance.StartCoroutine(nameof(SettingPropContent.instance.SettingYES));

        //print(DataManager.instance.GetQuestInfo().locationName);

        // ������ ���� : �𵨸�, ��Ҹ�, �湮��¥, �ּҸ�, ����Ʈ����, ��һ��� => 6����
        //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = prop.GetComponent<MeshRenderer>().material;
        SettingPropContent.instance.content[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().locationName;
        SettingPropContent.instance.content[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.instance.GetQuestInfo().date.ToString("MM�� dd��");
        SettingPropContent.instance.content[3].GetChild(0).GetComponent<TextMeshProUGUI>().text = TextBreak(DataManager.instance.GetQuestInfo().addr);
        Parameters parameters = new Parameters(DataManager.instance.GetQuestInfo().questImage, 4);
        yield return StartCoroutine(GetTexture(parameters));
        parameters = new Parameters(DataManager.instance.GetQuestInfo().imageUrl, 5);
        yield return StartCoroutine(GetTexture(parameters));

        // UI Ȱ��ȭ
        Props_UI.instance.canvasProp.enabled = true;

        // 3D �𵨸�        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);
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

    // ���/�ּҸ� �ڸ���
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