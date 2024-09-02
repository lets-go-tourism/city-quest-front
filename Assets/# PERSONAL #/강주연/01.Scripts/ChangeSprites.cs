using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprites : MonoBehaviour
{
    //public enum Sort
    //{
    //    N,
    //    YES,
    //    NO
    //}
    // public Sort sort;

    //[HideInInspector] 
    public Transform[] place;
    //[HideInInspector] 
    public Transform[] tour;

    //public void ChangeSort()
    //{
    //    if(sort == Sort.N)
    //    {

    //    }
    //}

    public void ChangePlaceSprites(int no)
    {
        // UI
        for (int i = 0; i < place.Length; i++)
        {
            place[i].GetComponent<Image>().sprite = place[i].GetComponent<SpritesHolder>().sprites[0];  // 안눌림
        }

        place[no].GetComponent<Image>().sprite = place[no].GetComponent<SpritesHolder>().sprites[1];    // 눌림

        // 카드 활성화
        if (no == 0)
        {
            BottomSheetManager.instance.soringAll(true);
        }
        else if (no == 1)
        {
            BottomSheetManager.instance.SortingPlace("REVEAL");
        }
        else if (no == 2)
        {
            BottomSheetManager.instance.SortingPlace("UNREVEAL");
        }

        // 스크롤 초기화
        MainView_UI.instance.placeScrollRect.horizontalNormalizedPosition = 0;
    }

    public void ChangeTourSprites(int no)
    {
        // UI
        for (int i = 0; i < tour.Length; i++)
        {
            tour[i].GetComponent<Image>().sprite = tour[i].GetComponent<SpritesHolder>().sprites[0];    // 안눌림
        }

        tour[no].GetComponent<Image>().sprite = tour[no].GetComponent<SpritesHolder>().sprites[1];        // 눌림

        // 카드 활성화
        if (no == 0)    { BottomSheetManager.instance.soringAll(false); }
        else if (no == 1) { BottomSheetManager.instance.SortingTour("TouristSpot"); }
        else if (no == 2) { BottomSheetManager.instance.SortingTour("CulturalFacilities"); }
        else if (no == 3) { BottomSheetManager.instance.SortingTour("Festival"); }
        else if (no == 4) { BottomSheetManager.instance.SortingTour("TravelCource"); }
        else if (no == 5) { BottomSheetManager.instance.SortingTour("LeisureSports"); }
        else if (no == 6) { BottomSheetManager.instance.SortingTour("Lodgment"); }
        else if (no == 7) { BottomSheetManager.instance.SortingTour("Shopping"); }
        else if (no == 8) { BottomSheetManager.instance.SortingTour("Food"); }

        // 스크롤 초기화
        MainView_UI.instance.tourScrollRect.horizontalNormalizedPosition = 0;
    }
}