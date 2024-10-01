using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.Mathematics;

public class PopUpMovement : MonoBehaviour
{
    public RectTransform rtPlace;
    public RectTransform rtTour;

    public RectTransform placeUN;
    public RectTransform placeAD;
    public RectTransform tour;

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

    public bool adventured;

    public IEnumerator MoveUP(bool place)
    {
        if (place)
        {
            // UI 활성화

            if (adventured)
            {
                placeAD.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);
                skeleton = true;
                StartCoroutine(nameof(DoSkeleton), true);
            }
            else
            {
                placeUN.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);
                skeleton = true;
                StartCoroutine(nameof(DoSkeleton), true);
            }

            yield return new WaitForSeconds(0.4f);

            rtPlace.anchoredPosition = new Vector2(0, 0);

            placeState = PlaceState.UP;
        }
        else
        {
            tour.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);

            skeleton = true;
            StartCoroutine(nameof(DoSkeleton), false);

            yield return new WaitForSeconds(0.4f);

            rtTour.anchoredPosition = new Vector2(0, 0);

            tourState = TourState.UP;
        }

        tmpTouch.instance.StartCoroutine(nameof(tmpTouch.instance.ChangeState), tmpTouch.State.Pop);
    }

    public bool skeleton;

    IEnumerator DoSkeleton(bool place)
    {
        float t = 0;
        float d = 0.5f;

        if (place)
        {
            if (adventured)
            {
                while (skeleton)
                {
                    while (t < d)
                    {
                        placeAD.transform.GetChild(8).GetComponent<Image>().fillAmount = Mathf.Lerp(0, 1, t / d);
                        t += Time.deltaTime;
                        yield return null;
                    }

                    placeAD.transform.GetChild(8).GetComponent<Image>().fillAmount = 0f;
                    t = 0;
                }
            }
            else
            {
                while (skeleton)
                {
                    t = 0;
                    placeUN.GetChild(10).transform.GetComponent<Image>().fillAmount = 0f;
                    placeUN.GetChild(10).transform.GetComponent<Image>().fillOrigin = 0;

                    while (t < d)
                    {
                        placeUN.GetChild(10).transform.GetComponent<Image>().fillAmount = Mathf.Lerp(0, 1, t / d);
                        t += Time.deltaTime;
                        yield return null;
                    }

                    placeUN.GetChild(10).transform.GetComponent<Image>().fillAmount = 1f;
                    placeUN.GetChild(10).transform.GetComponent<Image>().fillOrigin = 1;
                    t = 0;

                    while (t < d)
                    {
                        placeUN.GetChild(10).transform.GetComponent<Image>().fillAmount = Mathf.Lerp(1, 0, t / d);
                        t += Time.deltaTime;
                        yield return null;
                    }
                }
            }
        }
        else
        {
            yield break;
        }
    }

    public IEnumerator MoveDOWN(bool place)
    {
        Props_UI.instance.ResetScollView();

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

        // 배경 암전 끄기
        MainView_UI.instance.BackgroundDarkDisable();
    }
}
