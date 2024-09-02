using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SettingTourInfo : MonoBehaviour
{
    public RectTransform rtPlace;
    public RectTransform rtTour;

    // Start is called before the first frame update
    void Start()
    {
        rtTour.anchoredPosition = new Vector2(0, -1770);
        rtPlace.anchoredPosition = new Vector2(0, -2060);

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(3f);

        MoveUP(false);
    }

    void MoveUP(bool place)
    {
        if (place) { rtPlace.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack); }
        else { rtTour.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack); }
    }

    void MoveDOWN(bool place)
    {
        if (place) { rtPlace.DOAnchorPosY(-2060, 0.5f).SetEase(Ease.OutBack); }
        else { rtTour.DOAnchorPosY(-1770, 0.5f).SetEase(Ease.OutBack); }
    }
}
