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
    }

    public IEnumerator MoveUP(bool place)
    {
        //KJY �߰�
        SettingManager.instance.BackGroundSound_InProp();

        if (place)
        {
            // UI Ȱ��ȭ
            //Props_UI.instance.canvasProp.enabled = true;

            // 3D �𵨸�        

            rtPlace.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.5f);

            placeState = PlaceState.UP;
        }
        else
        {
            // UI Ȱ��ȭ
            //Props_UI.instance.canvasTour.enabled = true;

            // 3D �𵨸�        
            //Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);

            rtTour.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.5f);

            tourState = TourState.UP;
        }

        tmpTouch.instance.StartCoroutine(nameof(tmpTouch.instance.ChangeState),tmpTouch.State.Pop);
    }

    public IEnumerator MoveDOWN(bool place)
    {
        Props_UI.instance.ResetScollView();

        //KJY �߰�
        SettingManager.instance.EffectSound_PopDown();
        SettingManager.instance.BackGroundSound_Original();

        if (place)
        {
            rtPlace.DOAnchorPosY(-2600, 0.38f);

            yield return new WaitForSeconds(0.5f);

            //Props_UI.instance.canvasProp.enabled = false;

            placeState = PlaceState.DOWN;
        }
        else
        {
            rtTour.DOAnchorPosY(-2300, 0.38f);

            yield return new WaitForSeconds(0.5f);

            //Props_UI.instance.canvasTour.enabled = false;

            tourState = TourState.DOWN;
        }

        // ��� ���� ����
        MainView_UI.instance.BackgroundDarkDisable();
    }
}
