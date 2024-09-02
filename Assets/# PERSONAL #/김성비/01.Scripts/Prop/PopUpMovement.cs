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

        rtTour.anchoredPosition = new Vector2(0, -1770);
        rtPlace.anchoredPosition = new Vector2(0, -2060);
    }

    public IEnumerator MoveUP(bool place)
    {
        if (place) 
        { 
            rtPlace.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.5f);

            placeState = PlaceState.UP;
        }
        else 
        { 
            rtTour.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.5f);

            tourState = TourState.UP;
        }

        // UI È°¼ºÈ­
        Props_UI.instance.canvasProp.enabled = true;

        // 3D ¸ðµ¨¸µ        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);
    }

    public IEnumerator MoveDOWN(bool place)
    {
        if (place) 
        { 
            rtPlace.DOAnchorPosY(-2060, 0.5f);

            yield return new WaitForSeconds(0.5f);

            Props_UI.instance.canvasProp.enabled = false;

            placeState = PlaceState.DOWN;
        }
        else 
        {
            rtTour.DOAnchorPosY(-1770, 0.5f);

            yield return new WaitForSeconds(0.5f);

            Props_UI.instance.canvasTour.enabled = false;

            tourState = TourState.DOWN;
        }

        // 3D ¸ðµ¨¸µ        
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(false);
    }
}
