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
    [SerializeField]
    private List<ServerTourInfo> tourPlacesList;
    //private Dictionary<long, ServerTourInfo> tourPlaceDiction;

    [Header("currentGPS")]
    private LocationInfo gpsInfo;

    [Header("QuestPopUp")]
    private QuestData questInfo;

    [Header("LoginData")]
    private LoginResponse loginData;

    [Header("tutorial")]
    public bool clearTutorial = false;

    [HideInInspector]
    public bool requestSuccess = false;

    [Header("SaveUserTokenData")]
    private string path;

    private List<string> removeList = new List<string> {"장안문", "진미통닭", "수원선경도서관", "플라잉수원", "화성어차"};

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
        public bool isClearTutorial;
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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        SortTourAdventureList();
    //    }

    //    //  if (Input.GetKeyDown(KeyCode.Alpha2))
    //    //    {
    //    //        SortTourPlaceList();
    //    //    }
    //}

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
                clearTutorial = saveData.isClearTutorial;

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
        save.isClearTutorial = clearTutorial;

        string json = JsonUtility.ToJson(save, true); // JSON 형식으로 변환
        File.WriteAllText(path, json); // 파일로 저장
    }

    //튜토리얼 정보
    public void SaveTutorialInfo()
    {
        clearTutorial = true;
        JsonSave();
    }

    //프랍리스트 설정하는 함수
    public void SetHomePropsList(List<ServerProp> homeProps)
    {
        propsList = homeProps;

        for (int i = 0; i < propsList.Count; i++)
        {
            float x = (float)MercatorProjection.lonToX(propsList[i].longitude);
            float y = (float)MercatorProjection.latToY(propsList[i].latitude);

            Vector3 objPosition = new Vector3(x, 0, y) - MapReader.Instance.boundsCenter;
            propsList[i].position = objPosition;
        }
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

        SortPropAdventureList();
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

            if (RemoveTourList(hometourPlaces[i].title))
            {
                hometourPlaces.Remove(hometourPlaces[i]);
            }
            hometourPlaces[i].idx = i;
        }
        tourPlacesList = hometourPlaces;

        for (int i = 0; i < tourPlacesList.Count; i++)
        {
            float x = (float)MercatorProjection.lonToX(double.Parse(tourPlacesList[i].longitude));
            float y = (float)MercatorProjection.latToY(double.Parse(tourPlacesList[i].latitude));

            Vector3 objPosition = new Vector3(x, 0, y) - MapReader.Instance.boundsCenter;
            tourPlacesList[i].position = objPosition;
        }

        SortTourList();
    }

    public bool RemoveTourList(string tour)
    {
        for (int i = 0; i < removeList.Count; i++)
        {
            if (tour == removeList[i])
            {
                return true;
            }
        }
        return false;
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
        }

        if (HttpManager.instance != null)
        {
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

    //public void SortTourPlaceList()
    //{
    //    Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);

    //    Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);

    //    tourPlacesList = tourPlacesList
    //        .OrderBy(place => Vector3.Distance(worldCenter, place.position))
    //        .ToList();
    //}

    public void SortPropAdventureList()
    {
        Vector3 worldCenter = MapCameraController.Instance.GetTargetCenter();

        var tmpList = new List<(ServerProp Prop, ServerAdventurePlace Adventure)>();
        
        for (int i = 0; i < propsList.Count; i++)
        {
           tmpList.Add((propsList[i], adventurePlacesList[i]));
        }

         var sortedTmpList = tmpList
        .OrderBy(item => Vector3.Distance(worldCenter, item.Prop.position))
        .ToList();

        propsList = sortedTmpList.Select(item => item.Prop).ToList();
        adventurePlacesList = sortedTmpList.Select(item => item.Adventure).ToList();
    }

    public void SortTourList()
    {
        Vector3 worldCenter = MapCameraController.Instance.GetTargetCenter();

        tourPlacesList = tourPlacesList
            .OrderBy(place => Vector3.Distance(worldCenter, place.position))
            .ToList();
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
