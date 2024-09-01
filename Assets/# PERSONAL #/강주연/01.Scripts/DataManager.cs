using System.Collections;
using System.Collections.Generic;
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

    public List<string> testConnectin;

    // 박현섭
    [HideInInspector] 
    public bool requestSuccess = false;

    #region notUse
    [Header("QuestList")]
    private List<QuestData> questDataList;

    [Header("UnExplorationQuestList")]
    private List<QuestData> unExplorationQuestList;

    [Header("ExplorationQuestList")]
    private List<QuestData> explorationQuestList;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        KJY_ConnectionTMP.instance.OnClickHomeConnection();
    }

    //프랍리스트 설정하는 함수
    public void SetHomePropsList(List<ServerProp> homeProps)
    {
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
        this.loginData = loginData; 
    }

    public LoginResponse GetLoginData()
    {
        return loginData;
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
