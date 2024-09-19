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

        string json = JsonUtility.ToJson(save, true); // JSON 형식으로 변환
        File.WriteAllText(path, json); // 파일로 저장
        Debug.Log(json);
    }

    //프랍리스트 설정하는 함수
    public void SetHomePropsList(List<ServerProp> homeProps)
    {
        print("이거 실행 됨???????");
        propsList = homeProps;
    }

    //프랍리스트 얻는 함수
    public List<ServerProp> GetHomePropsList()
    {
        return propsList;
    }

    //탐험미탐험 장소 설정하는 함수
    public void SetHomeAdventurePlaceList(List<ServerAdventurePlace> adventurePlaces)
    {
        for (int i = 0; i < adventurePlaces.Count; i++)
        {
            adventurePlaces[i].name = ReplaceHanjaWithSpace(adventurePlaces[i].name);
        }
        adventurePlacesList = adventurePlaces;
    }

    //탐험미탐험 장소 얻는 함수
    public List<ServerAdventurePlace> GetHomeAdventurePlacesList()
    {
        return adventurePlacesList;
    }

    //관광정보 설정하는 함수
    public void SetHometourPlaceList(List<ServerTourInfo> hometourPlaces)
    {
        for (int i = 0; i < hometourPlaces.Count; i++)
        {
            hometourPlaces[i].title = ReplaceHanjaWithSpace(hometourPlaces[i].title);
        }

        tourPlacesList = hometourPlaces;
    }

    //관광정보 얻는 함수
    public List<ServerTourInfo> GetHometourPlacesList()
    {
        return tourPlacesList;
    }

    //GPS정보 설정하는 함수
    public void SetGPSInfo(LocationInfo info)
    {
        gpsInfo = info;
    }

    //GSP정보 얻는 함수
    public LocationInfo GetGPSInfo()
    {
        return gpsInfo;
    }

    //퀘스트 팝업 정보 설정하는 함수
    public void SetQuestInfo(QuestData popInfo)
    {
        questInfo = popInfo;
    }

    //퀘스트 팝업 정보 얻는 함수
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
        // 문자열의 각 문자를 순회하면서 한자를 찾고, 한자를 공백으로 대체
        return new string(input.Select(c => IsHanja(c) ? ' ' : c).ToArray());
    }

    // 문자가 한자인지 확인하는 메서드
    private bool IsHanja(char c)
    {
        int codePoint = c;
        // 한자 범위: 기본 한자, 확장 A, 확장 B
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
    //    if (status) //탐험 성공한 프랍 특정 데이터 찾을 때
    //    {
    //        return explorationQuestList[num - 1];
    //    }
    //    else //탐험 미성공시
    //    {
    //        return unExplorationQuestList[num - 1];
    //    }
    //}
    #endregion
}
