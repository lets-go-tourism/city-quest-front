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
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        MoveUP();
    //    }
    //    else if(Input.GetMouseButtonDown(1))
    //    {
    //        MoveDOWN();
    //    }
    //}

    public void MoveUP()
    {
        // ���ҽ�Ʈ �ö󰡱�
        rt.DOAnchorPosY(-696, 1f);
        Invoke(nameof(ChangeUP), 1);
    }

    public void MoveDOWN() 
    {
        // ���ҽ�Ʈ ��������
        rt.DOAnchorPosY(-1260, 1f);
        Invoke(nameof(ChangeDOWN), 1);
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
