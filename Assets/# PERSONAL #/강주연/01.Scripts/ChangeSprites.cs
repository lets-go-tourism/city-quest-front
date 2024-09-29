using UnityEngine;
using UnityEngine.UI;

public class ChangeSprites : MonoBehaviour
{
    public static ChangeSprites instance;
    private void Awake()
    {
        instance = this;
    }

    //[HideInInspector] 
    public Transform[] place;
    //[HideInInspector] 
    public Transform[] tour;


    public void ChangePlaceSprites(int no)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
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
                BottomSheetManager.instance.SortingAll(true);
            }
            else if (no == 1)
            {
                print("1차 진입");
                BottomSheetManager.instance.FilteringPlace(CardPlaceInfo.Type.REVEAL);
            }
            else if (no == 2)
            {
                BottomSheetManager.instance.FilteringPlace(CardPlaceInfo.Type.UNREVEAL);
            }

            // 스크롤 초기화
            MainView_UI.instance.placeScrollRect.horizontalNormalizedPosition = 0;
        }
    }

    public void ChangeTourSprites(int no)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
        {
            // UI
            for (int i = 0; i < tour.Length; i++)
            {
                tour[i].GetComponent<Image>().sprite = tour[i].GetComponent<SpritesHolder>().sprites[0];    // 안눌림
            }

            tour[no].GetComponent<Image>().sprite = tour[no].GetComponent<SpritesHolder>().sprites[1];        // 눌림

            // 카드 활성화
            if (no == 0) { BottomSheetManager.instance.SortingAll(false); }
            else if (no == 1) { BottomSheetManager.instance.FilteringTour("TouristSpot"); }
            else if (no == 2) { BottomSheetManager.instance.FilteringTour("CulturalFacilities"); }
            else if (no == 3) { BottomSheetManager.instance.FilteringTour("Festival"); }
            else if (no == 4) { BottomSheetManager.instance.FilteringTour("TravelCource"); }
            else if (no == 5) { BottomSheetManager.instance.FilteringTour("LeisureSports"); }
            else if (no == 6) { BottomSheetManager.instance.FilteringTour("Lodgment"); }
            else if (no == 7) { BottomSheetManager.instance.FilteringTour("Shopping"); }
            else if (no == 8) { BottomSheetManager.instance.FilteringTour("Food"); }

            // 스크롤 초기화
            MainView_UI.instance.tourScrollRect.horizontalNormalizedPosition = 0;
        }
    }
}