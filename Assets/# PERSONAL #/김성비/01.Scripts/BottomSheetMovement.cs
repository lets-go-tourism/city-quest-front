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
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveUP();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            MoveDOWN();
        }
    }
    public void MoveUP()
    {
        rt.anchoredPosition = new Vector2(0,948);
    }

    public void MoveDOWN() 
    {
        rt.anchoredPosition = new Vector2(0, 360);
    }
}
