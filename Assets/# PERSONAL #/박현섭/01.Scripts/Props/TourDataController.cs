using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourDataController : MonoBehaviour
{
    public Dictionary<long, ServerTourInfo> HomeTourPlaceDic { get; private set; } = new Dictionary<long, ServerTourInfo>();

    private IEnumerator Start()
    {
        while (DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

        List<ServerTourInfo> tourList = DataManager.instance.GetHometourPlacesList();

        for(int i = 0; i < tourList.Count; i++)
        {
            //
        }

        //PropList = DataManager.instance.GetHomePropsList();
        //List<HomeAdventurePlace> placeList = DataManager.instance.GetHomeAdventurePlacesList();

        //for (int i = 0; i < PropList.Count; i++)
        //{
        //    PropDic.Add(PropList[i].propNo, PropList[i]);
        //}

        //for (int i = 0; i < placeList.Count; i++)
        //{
        //    AdventurePlaceDic.Add(placeList[i].adventureNo, placeList[i]);
        //}

        //for (int i = 0; i < PropList.Count; i++)
        //{
        //    CreateProp(PropList[i], AdventurePlaceDic[PropList[i].propNo]);
        //}
    }
}
