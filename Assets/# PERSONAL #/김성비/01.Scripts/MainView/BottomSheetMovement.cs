using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BottomSheetMovement : MonoBehaviour
{
    RectTransform rt;

    public enum State
    {
        UP, DOWN
    }
    public State state;

    public ScrollRect[] scrollRects;
    public Button[] btns;

    public static BottomSheetMovement instance;
    private void Awake()
    {
        instance = this;
        rt = GetComponent<RectTransform>();
    }

    public void MoveUP()
    {
        // 바텀시트 올라가기
        rt.DOPause();
        rt.DOAnchorPosY(948, 0.5f).SetEase(Ease.OutBack);
        Invoke(nameof(ChangeUP), 0.5f);
    }

    public void MoveDOWN()
    {
        // 바텀시트 내려가기
        rt.DOPause();
        rt.DOAnchorPosY(360, 0.3f);
        ChangeDOWN();
    }

    void ChangeUP()
    {
        if (state == State.DOWN)
        {
            state = State.UP;
            MapCameraController.Instance.isBottom = true;
        }

        for (int i = 0; i < scrollRects.Length; i++)
        {
            scrollRects[i].horizontal = true;
            btns[i].enabled = true;
        }
    }

    void ChangeDOWN()
    {
        if (state == State.UP)
        {
            state = State.DOWN;
            MapCameraController.Instance.isBottom = false;
        }

        for (int i = 0; i < scrollRects.Length; i++)
        {
            scrollRects[i].horizontal = false;
            btns[i].enabled = false;
        }
    }
}
