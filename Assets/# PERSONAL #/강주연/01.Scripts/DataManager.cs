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

    private List<string> removeList = new List<string> {"��ȹ�", "�������", "�������浵����", "�ö��׼���", "ȭ������"};

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

        string json = JsonUtility.ToJson(save, true); // JSON �������� ��ȯ
        File.WriteAllText(path, json); // ���Ϸ� ����
    }

    //Ʃ�丮�� ����
    public void SaveTutorialInfo()
    {
        clearTutorial = true;
        JsonSave();
    }

    //��������Ʈ �����ϴ� �Լ�
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

        SortPropAdventureList();
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
