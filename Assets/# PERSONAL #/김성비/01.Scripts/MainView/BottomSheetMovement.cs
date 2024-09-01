using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BottomSheetMovement : MonoBehaviour
{
    RectTransform rt;

    public static BottomSheetMovement instance;
    private void Awake()
    {
        instance = this;
        rt = GetComponent<RectTransform>();

        rt.anchoredPosition = new Vector2(0, 360);   
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
        // 바텀시트 올라가기
        //rt.anchoredPosition = new Vector2(0,948);
        transform.DOMove(new Vector3(540, 948, 0), 1f);

        // 버튼 터치 불가능

    }

    public void MoveDOWN() 
    {
        // 바텀시트 내려가기
        //rt.anchoredPosition = new Vector2(0, 360);
        transform.DOMove(new Vector3(540, 360, 0), 1f);

        // 버튼 터치 불가능

    }
}
