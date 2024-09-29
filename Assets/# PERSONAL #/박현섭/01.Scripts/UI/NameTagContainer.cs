using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NameTagContainer : MonoBehaviour
{
    public PropNameTagUI[] PropNameTagArr { get; private set; } = new PropNameTagUI[31];
    public TourNameTagUI[] TourNameTagArr { get; private set; } = new TourNameTagUI[80];

    private int _propNameTagCount = 0;
    private int _tourNameTagCount = 0;

    private CheckNameTagCollision _checkNameTagCollision;

    private void Start()
    {
        _checkNameTagCollision = GetComponent<CheckNameTagCollision>();

        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            PropNameTagArr[i] = transform.GetChild(1).GetChild(i).GetComponent<PropNameTagUI>();
            _propNameTagCount++;
        }

        for(int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            TourNameTagArr[i] = transform.GetChild(0).GetChild(i).GetComponent<TourNameTagUI>();
            _tourNameTagCount++;
        }
    }

    public bool collision = false;

    private float time = 0;


    private void Update()
    {
        if (MapCameraController.Instance.m_IsMoving == false)
            return;

        time += Time.deltaTime;

        if (time > 0.2f)
        {
            time = 0;
            CollisionUpdate();
        }
    }

    public void AddTarget(Prop target)
    {
        for(int i = 0; i < _propNameTagCount; i++)
        {
            if (PropNameTagArr[i].enabled)
                continue;

            PropNameTagArr[i].Init(target);
            break;
        }
    }

    public void AddTarget(TourData target)
    {
        for (int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].enabled)
                continue;

            TourNameTagArr[i].Init(target);
            break;
        }
    }

    public void RemoveTarget(Prop target)
    {
        for (int i  = 0; i < _propNameTagCount; i++)
        {
            if (PropNameTagArr[i].TargetProp != target)
                continue;

            PropNameTagArr[i].enabled = false;
            break;
        }
    }

    public void RemoveTarget(TourData target)
    {
        for (int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].TargetTour != target)
                continue;

            TourNameTagArr[i].enabled = false;
            break;
        }
    }

    private void CollisionUpdate()
    {
        for (int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].enabled == false)
                continue;

            TourNameTagArr[i].Visible = true;
        }

        for (int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].enabled == false)
                continue;

            UpdateCheckNameTagCollision(i);
        }
    }

    private RectTransform rect1;
    private Vector2 rect1SizeDelta;
    private Vector2 rect1AnchoredPosition;
    private float scale;

    private RectTransform rect2;
    private Vector2 rect2SizeDelta;
    private Vector2 rect2AnchoredPosition;

    public void UpdateCheckNameTagCollision(int index)
    {
        rect1 = TourNameTagArr[index].RectTransform;
        rect1SizeDelta = rect1.sizeDelta;
        rect1AnchoredPosition = rect1.anchoredPosition;
        scale = rect1.localScale.x;

        for (int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].enabled == false || i == index)
                continue;

            else if (TourNameTagArr[index].Visible == false || TourNameTagArr[i].Visible == false)
                continue;

            TourNameTagArr[i].CustomeUpdate();
            rect2 = TourNameTagArr[i].RectTransform;
            rect2SizeDelta = rect2.sizeDelta;
            rect2AnchoredPosition = rect2.anchoredPosition;
            

            // 두 사각형이 겹치지 않는지 확인 (AABB 충돌 감지)
            if (rect1AnchoredPosition.x + rect1SizeDelta.x / 2 * scale < rect2AnchoredPosition.x - rect2SizeDelta.x / 2 * scale
                || rect1AnchoredPosition.x - rect1SizeDelta.x / 2 * scale > rect2AnchoredPosition.x + rect2SizeDelta.x / 2 * scale)
            {
                continue; // x축에서 겹치지 않음
            }

            if (rect1AnchoredPosition.y + rect1SizeDelta.y / 2 * scale < rect2AnchoredPosition.y - rect2SizeDelta.y / 2 * scale
                || rect1AnchoredPosition.y - rect1SizeDelta.y / 2 * scale > rect2AnchoredPosition.y + rect2SizeDelta.y / 2 * scale)
            {
                continue; // y축에서 겹치지 않음
            }

            // 여기까지 오면 겹친거임

            //print(TourNameTagArr[i].TargetTour.ServerTourInfo.title + "와 " + TourNameTagArr[index].TargetTour.ServerTourInfo.title + "가 겹친다");

            if (rect2SizeDelta.x > rect1SizeDelta.x)
                TourNameTagArr[index].Visible = false;
            else
                TourNameTagArr[i].Visible = false;
        }
    }
}
