using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopUpMovement : MonoBehaviour
{
    public RectTransform rtPlace;
    public RectTransform rtTour;

    public enum PlaceState
    {
        DOWN, UP
    }
    public PlaceState placeState;

    public enum TourState
    {
        DOWN, UP
    }
    public TourState tourState;

    public static PopUpMovement instance;
    private void Awake()
    {
        instance = this;

        //rtTour.anchoredPosition = new Vector2(0, -1800);
        //rtPlace.anchoredPosition = new Vector2(0, -2300);
    }

    public IEnumerator MoveUP(bool place)
    {
        if (place)
        {
            // UI 활성화
            //Props_UI.instance.canvasProp.enabled = true;

            // 3D 모델링        

            rtPlace.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.5f);

            placeState = PlaceState.UP;
        }
        else
        {
            // UI 활성화
            //Props_UI.instance.canvasTour.enabled = true;

            // 3D 모델링        
            //Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);

            rtTour.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.5f);

            tourState = TourState.UP;
        }
    }

    public IEnumerator MoveDOWN(bool place)
    {
        Props_UI.instance.ResetScollView();

        if (place)
        {
            rtPlace.DOAnchorPosY(-2100, 0.38f);

            yield return new WaitForSeconds(0.5f);

            //Props_UI.instance.canvasProp.enabled = false;

            placeState = PlaceState.DOWN;
        }
        else
        {
            rtTour.DOAnchorPosY(-1800, 0.38f);

            yield return new WaitForSeconds(0.5f);

            //Props_UI.instance.canvasTour.enabled = false;

            tourState = TourState.DOWN;
        }

        // 배경 암전 끄기
        MainView_UI.instance.BackgroundDarkDisable();
    }
}
