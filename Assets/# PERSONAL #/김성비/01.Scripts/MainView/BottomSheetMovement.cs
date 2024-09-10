using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BottomSheetMovement : MonoBehaviour
{
    RectTransform rt;

    public enum State
    {
        UP, DOWN
    }
    public State state;

    public static BottomSheetMovement instance;
    private void Awake()
    {
        instance = this;
        rt = GetComponent<RectTransform>();
    }

    public void MoveUP()
    {
        // 바텀시트 올라가기
        rt.DOAnchorPosY(948, 0.5f).SetEase(Ease.OutBack);
        Invoke(nameof(ChangeUP), 0.5f);
    }

    public void MoveDOWN()
    {
        // 바텀시트 내려가기
        rt.DOAnchorPosY(360, 0.3f);
        ChangeDOWN();
    }

    void ChangeUP()
    {
        if (state == State.DOWN)
        {
            state = State.UP;
        }
    }

    void ChangeDOWN()
    {
        if (state == State.UP)
        {
            state = State.DOWN;
        }
    }
}
