using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header("PropDataList")]
    private List<HomeProps> homePropsList;

    [Header("AdventurePlaceList")]
    private List<HomeAdventurePlace> homeAdventurePlacesList;

    [Header("tourPlaceList")]
    private List<HometourPlace> hometourPlacesList;

    [Header("currentGPS")]
    private LocationInfo gpsInfo;

    [Header("QuestPopUp")]
    private QuestData questInfo;

    [Header("LoginData")]
    private LoginData loginData;

    // ������
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

    //��������Ʈ �����ϴ� �Լ�
    public void SetHomePropsList(List<HomeProps> homeProps)
    {
        homePropsList = homeProps;
    }

    //��������Ʈ ��� �Լ�
    public List<HomeProps> GetHomePropsList()
    {
        return homePropsList;
    }

    //Ž���Ž�� ��� �����ϴ� �Լ�
    public void SetHomeAdventurePlaceList(List<HomeAdventurePlace> adventurePlaces)
    {
        homeAdventurePlacesList = adventurePlaces;
    }

    //Ž���Ž�� ��� ��� �Լ�
    public List<HomeAdventurePlace> GetHomeAdventurePlacesList()
    {
        return homeAdventurePlacesList;
    }

    //�������� �����ϴ� �Լ�
    public void SetHometourPlaceList(List<HometourPlace> hometourPlaces)
    {
        hometourPlacesList = hometourPlaces;
    }

    //�������� ��� �Լ�
    public List<HometourPlace> GetHometourPlacesList()
    {
        return hometourPlacesList;
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

    public void SetLoginData(LoginData loginData)
    {
        this.loginData = loginData; 
    }

    public LoginData GetLoginData()
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
