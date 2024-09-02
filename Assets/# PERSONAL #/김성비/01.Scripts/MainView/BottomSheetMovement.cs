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
        // ���ҽ�Ʈ �ö󰡱�
        rt.DOAnchorPosY(-696, 0.5f).SetEase(Ease.OutBack);
        Invoke(nameof(ChangeUP), 0.5f);
    }

    public void MoveDOWN() 
    {
        // ���ҽ�Ʈ ��������
        rt.DOAnchorPosY(-1260, 0.4f);
        ChangeDOWN();
    }

    void ChangeUP()
    {
        state = State.UP;
    }

    void ChangeDOWN()
    {
        state = State.DOWN;
    }
}
