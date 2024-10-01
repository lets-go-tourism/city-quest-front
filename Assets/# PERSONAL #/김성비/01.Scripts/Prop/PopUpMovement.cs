using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PopUpMovement : MonoBehaviour
{
    // ½ºÅ©·Ñºä
    public RectTransform rtPlace;
    public RectTransform rtTour;

    // ½ºÄÌ·¹Åæ
    public RectTransform skPlaceUN;
    public RectTransform skPlaceAD;
    public RectTransform skTour;

    public bool cancel;         // Åë½Å Ãë¼Ò ¿©ºÎ
    public bool skeleton;       // ½ºÄÌ·¹Åæ °¡´É ¿©ºÎ
    public bool adventured;     // Å½Çè¿Ï·á ¿©ºÎ

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
        cancel = false;

        //KJY Ãß°¡
        SettingManager.instance.BackGroundSound_InProp();

        if (place)
        {
            if (adventured)
            {
                skPlaceAD.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);
                skeleton = true;
                StartCoroutine(nameof(PlaceSkeleton));
            }
            else
            {
                skPlaceUN.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);
                skeleton = true;
                StartCoroutine(nameof(PlaceSkeleton));
            }

            yield return new WaitForSeconds(0.4f);

            placeState = PlaceState.UP;
        }
        else
        {
            skTour.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);
            skeleton = true;
            StartCoroutine(nameof(TourSkeleton));

            yield return new WaitForSeconds(0.4f);

            tourState = TourState.UP;
        }

        tmpTouch.instance.StartCoroutine(nameof(tmpTouch.instance.ChangeState), tmpTouch.State.Pop);
    }

    public IEnumerator MoveDOWN(bool place)
    {
        Props_UI.instance.ResetScollView();

        //KJY Ãß°¡
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
            rtTour.DOAnchorPosY(-2500, 0.38f);

            yield return new WaitForSeconds(0.5f);

            //Props_UI.instance.canvasTour.enabled = false;

            tourState = TourState.DOWN;
        }

        // ¹è°æ ¾ÏÀü ²ô±â
        MainView_UI.instance.BackgroundDarkDisable();
    }

    #region ½ºÄÌ·¹Åæ ¾Ö´Ï¸ÞÀÌ¼Ç
    public IEnumerator PlaceSkeleton()
    {
        float t;
        float d = 0.5f;

        if (adventured)
        {
            while (skeleton)
            {
                t = 0;
                skPlaceAD.transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                skPlaceAD.transform.GetChild(1).GetComponent<Image>().fillOrigin = 0;

                while (t < d)
                {
                    skPlaceAD.transform.GetChild(1).GetComponent<Image>().fillAmount = Mathf.Lerp(0, 1, t / d);
                    t += Time.deltaTime;
                    yield return null;
                }

                skPlaceAD.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f;
                skPlaceAD.transform.GetChild(1).GetComponent<Image>().fillOrigin = 1;
                t = 0;

                while (t < d)
                {
                    skPlaceAD.transform.GetChild(1).GetComponent<Image>().fillAmount = Mathf.Lerp(1, 0, t / d);
                    t += Time.deltaTime;
                    yield return null;
                }
            }
        }
        else
        {
            while (skeleton)
            {
                t = 0;
                skPlaceUN.transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                skPlaceUN.transform.GetChild(1).GetComponent<Image>().fillOrigin = 0;

                while (t < d)
                {
                    skPlaceUN.transform.GetChild(1).GetComponent<Image>().fillAmount = Mathf.Lerp(0, 1, t / d);
                    t += Time.deltaTime;
                    yield return null;
                }

                skPlaceUN.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f;
                skPlaceUN.transform.GetChild(1).GetComponent<Image>().fillOrigin = 1;
                t = 0;

                while (t < d)
                {
                    skPlaceUN.transform.GetChild(1).GetComponent<Image>().fillAmount = Mathf.Lerp(1, 0, t / d);
                    t += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }

    public IEnumerator TourSkeleton()
    {
        float t;
        float d = 0.5f;

        while (skeleton)
        {
            t = 0;
            skTour.GetChild(1).transform.GetComponent<Image>().fillAmount = 0f;
            skTour.GetChild(1).transform.GetComponent<Image>().fillOrigin = 0;

            while (t < d)
            {
                skTour.GetChild(1).transform.GetComponent<Image>().fillAmount = Mathf.Lerp(0, 1, t / d);
                t += Time.deltaTime;
                yield return null;
            }

            skTour.GetChild(1).transform.GetComponent<Image>().fillAmount = 1f;
            skTour.GetChild(1).transform.GetComponent<Image>().fillOrigin = 1;
            t = 0;

            while (t < d)
            {
                skTour.GetChild(1).transform.GetComponent<Image>().fillAmount = Mathf.Lerp(1, 0, t / d);
                t += Time.deltaTime;
                yield return null;
            }
        }
    }
    #endregion
}
