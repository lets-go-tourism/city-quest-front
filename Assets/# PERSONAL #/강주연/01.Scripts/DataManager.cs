using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header("PropDataList")]
    private List<ServerProp> propsList;

    [Header("AdventurePlaceList")]
    private List<ServerAdventurePlace> adventurePlacesList;

    [Header("tourPlaceList")]
    private List<ServerTourInfo> tourPlacesList;

    [Header("currentGPS")]
    private LocationInfo gpsInfo;

    [Header("QuestPopUp")]
    private QuestData questInfo;

    [Header("LoginData")]
    private LoginResponse loginData;


    [HideInInspector]
    public bool requestSuccess = false;

    [Header("SaveUserTokenData")]
    private string path;

    public bool isLogout = false;
    #region notUse
    [Header("QuestList")]
    private List<QuestData> questDataList;

    [Header("UnExplorationQuestList")]
    private List<QuestData> unExplorationQuestList;

    [Header("ExplorationQuestList")]
    private List<QuestData> explorationQuestList;
    #endregion

    private class SaveData
    {
        public string timeStamp;
        public string status;
        public string accessToken;
        public string refreshToken;
        public bool agreed;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);

#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "database.json");
#elif UNITY_EDITOR
        path = Path.Combine(Application.dataPath, "database.json");
#endif
        JsonLoad();
    }

    public void JsonLoad()
    {
        SaveData saveData;

        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                LoginResponse loginResponse = new LoginResponse();
                loginResponse.data = new LoginData();

                loginResponse.timeStamp = DateTime.Now;
                loginResponse.status = saveData.status;
                loginResponse.data.accessToken = saveData.accessToken;
                loginResponse.data.refreshToken = saveData.refreshToken;
                loginResponse.data.agreed = saveData.agreed;

                SetLoginData(loginResponse);
            }
        }
    }

    public void JsonSave()
    {
        SaveData save = new SaveData();
        save.timeStamp = loginData.timeStamp.ToString();
        save.status = loginData.status;
        save.accessToken = loginData.data.accessToken;
        save.refreshToken = loginData.data.refreshToken;
        save.agreed = loginData.data.agreed;

        string json = JsonUtility.ToJson(save, true); // JSON �������� ��ȯ
        File.WriteAllText(path, json); // ���Ϸ� ����
        Debug.Log(json);
    }

    //��������Ʈ �����ϴ� �Լ�
    public void SetHomePropsList(List<ServerProp> homeProps)
    {
        print("�̰� ���� ��???????");
        propsList = homeProps;
    }

    //��������Ʈ ��� �Լ�
    public List<ServerProp> GetHomePropsList()
    {
        return propsList;
    }

    //Ž���Ž�� ��� �����ϴ� �Լ�
    public void SetHomeAdventurePlaceList(List<ServerAdventurePlace> adventurePlaces)
    {
        for (int i = 0; i < adventurePlaces.Count; i++)
        {
            adventurePlaces[i].name = ReplaceHanjaWithSpace(adventurePlaces[i].name);
        }
        adventurePlacesList = adventurePlaces;
    }

    //Ž���Ž�� ��� ��� �Լ�
    public List<ServerAdventurePlace> GetHomeAdventurePlacesList()
    {
        return adventurePlacesList;
    }

    //�������� �����ϴ� �Լ�
    public void SetHometourPlaceList(List<ServerTourInfo> hometourPlaces)
    {
        for (int i = 0; i < hometourPlaces.Count; i++)
        {
            hometourPlaces[i].title = ReplaceHanjaWithSpace(hometourPlaces[i].title);
        }

        tourPlacesList = hometourPlaces;
    }

    //�������� ��� �Լ�
    public List<ServerTourInfo> GetHometourPlacesList()
    {
        return tourPlacesList;
    }

    //GPS���� �����ϴ� �Լ�
    public void SetGPSInfo(LocationInfo info)
    {
        gpsInfo = info;
    }

    //GSP���� ��� �Լ�
    public LocationInfo GetGPSInfo()
    {
        return gpsInfo;
    }

    //����Ʈ �˾� ���� �����ϴ� �Լ�
    public void SetQuestInfo(QuestData popInfo)
    {
        questInfo = popInfo;
    }

    //����Ʈ �˾� ���� ��� �Լ�
    public QuestData GetQuestInfo()
    {
        return questInfo;
    }

    public void SetLoginData(LoginResponse loginData)
    {
        if (loginData != null)
        {
            this.loginData = loginData;
            Debug.Log("have_logindata");
        }

        if (HttpManager.instance != null)
        {
            Debug.Log("Have_httpManager");
            HttpManager.instance.loginData = loginData;
        }
    }

    public LoginResponse GetLoginData()
    {
        return loginData;
    }

    public string GetPathData()
    {
        return path;
    }

    public string ReplaceHanjaWithSpace(string input)
    {
        // ���ڿ��� �� ���ڸ� ��ȸ�ϸ鼭 ���ڸ� ã��, ���ڸ� �������� ��ü
        return new string(input.Select(c => IsHanja(c) ? ' ' : c).ToArray());
    }

    // ���ڰ� �������� Ȯ���ϴ� �޼���
    private bool IsHanja(char c)
    {
        int codePoint = c;
        // ���� ����: �⺻ ����, Ȯ�� A, Ȯ�� B
        return (codePoint >= 0x4E00 && codePoint <= 0x9FFF) ||
               (codePoint >= 0x3400 && codePoint <= 0x4DBF) ||
               (codePoint >= 0x20000 && codePoint <= 0x2A6DF) ||
               (codePoint == '(' || codePoint == ')' || codePoint == '<' || codePoint == '>');
    }

    #region notUse
    //public void SetAllQuestList(List<QuestData> list)
    //{
    //    questDataList = list;
    //}

    //public List<QuestData> GetAllQuestList()
    //{
    //    return questDataList;
    //}

    //public void SetUnExplorationQuestList(List<QuestData> list)
    //{
    //    unExplorationQuestList = list;
    //}

    //public List<QuestData> GetUnExplorationQuestList()
    //{
    //    return unExplorationQuestList;
    //}

    //public void SetExplorationQuestList(List<QuestData> list)
    //{
    //    explorationQuestList = list;
    //}

    //public List<QuestData> GetExplorationQuestList()
    //{
    //    return explorationQuestList;
    //}

    //public QuestData GetSpecificQuestList(int num, bool status)
    //{
    //    if (status) //Ž�� ������ ���� Ư�� ������ ã�� ��
    //    {
    //        return explorationQuestList[num - 1];
    //    }
    //    else //Ž�� �̼�����
    //    {
    //        return unExplorationQuestList[num - 1];
    //    }
    //}
    #endregion
}
