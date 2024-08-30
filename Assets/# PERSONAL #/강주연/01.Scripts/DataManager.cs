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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        KJY_ConnectionTMP.instance.OnClickHomeConnection();
    }

    public void SetHomePropsList(List<HomeProps> homeProps)
    {
        homePropsList = homeProps;
    }

    public List<HomeProps> GetHomePropsList()
    {
        return homePropsList;
    }

    public void SetHomeAdventurePlaceList(List<HomeAdventurePlace> adventurePlaces)
    {
        homeAdventurePlacesList = adventurePlaces;
    }

    public List<HomeAdventurePlace> GetHomeAdventurePlacesList()
    {
        return homeAdventurePlacesList;
    }

    public void SetHometourPlaceList(List<HometourPlace> hometourPlaces)
    {
        hometourPlacesList = hometourPlaces;
    }

    public List<HometourPlace> GetHometourPlacesList()
    {
        return hometourPlacesList;
    }

    public void SetGPSInfo(LocationInfo info)
    {
        gpsInfo = info;
    }

    public LocationInfo GetGPSInfo()
    {
        return gpsInfo;
    }
}
