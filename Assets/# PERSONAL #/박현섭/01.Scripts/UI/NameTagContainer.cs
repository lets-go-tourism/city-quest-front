using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NameTagContainer : MonoBehaviour
{
    public PropNameTagUI[] PropNameTagArr { get; private set; } = new PropNameTagUI[31];
    public TourNameTagUI[] TourNameTagArr { get; private set; } = new TourNameTagUI[300];

    public bool[] TourNameTagBoolArr { get; private set; } = new bool[300];

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

        prevHeight = Screen.height;
        prevWidth = Screen.width;

        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(prevWidth, prevHeight);
        transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(prevWidth, prevHeight);
    }

    private float time = 0;

    private float prevWidth = 0;
    private float prevHeight = 0;

    private void Update()
    {
        // 해상도가 바뀔경우 자식 0번 1번의 width height도 바꾼다
        if (prevHeight != Screen.height || prevWidth != Screen.width)
        {
            prevWidth = Screen.width;
            prevHeight = Screen.height;

            transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(prevWidth, prevHeight);
            transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(prevWidth, prevHeight);
        }

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

    public void CollisionUpdate()
    {
        for (int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].enabled == false)
                continue;

            //TourNameTagArr[i].OneFrameChangeVisible();
            TourNameTagBoolArr[i] = true;
        }

        for (int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].enabled == false)
                continue;

            UpdateCheckNameTagCollision(i);
        }

        for(int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].enabled == false)
                continue;

            TourNameTagArr[i].Visible = TourNameTagBoolArr[i];
        }
    }

    private RectTransform rect1;
    private Vector2 rect1SizeDelta;
    private Vector2 rect1AnchoredPosition;
    private float scale;

    private RectTransform rect2;
    private Vector2 rect2SizeDelta;
    private Vector2 rect2AnchoredPosition;

    private float minus;

    public void UpdateCheckNameTagCollision(int index)
    {
        rect1 = TourNameTagArr[index].RectTransform;
        rect1SizeDelta = rect1.sizeDelta;
        rect1AnchoredPosition = rect1.anchoredPosition;
        scale = rect1.localScale.x;

        for (int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].enabled == false || i == index) continue;

            rect2 = TourNameTagArr[i].RectTransform;
            rect2SizeDelta = rect2.sizeDelta;
            rect2AnchoredPosition = rect2.anchoredPosition;
            minus = rect1AnchoredPosition.y - rect2AnchoredPosition.y;

            if (minus < 0) continue;

            // rect1이 rect2 보다 78 + 100이상 (충돌안함) 만큼 떨어져 있을때 123 - 20
            if (minus > 103) continue;
            // rect1이 rect2 보다 78과 100 사이 위에는 이름표 아래는 아이콘이 겹칠경우
            else if (minus > 39)
            {
                if (PropsController.Instance.TintTourData == TourNameTagArr[index].TargetTour) continue;

                // 두 사각형이 겹치지 않는지 확인 (AABB 충돌 감지)
                if (rect1AnchoredPosition.x + rect1SizeDelta.x * 0.5f * scale < rect2AnchoredPosition.x - 36 * scale
                    || rect1AnchoredPosition.x - rect1SizeDelta.x * 0.5f * scale > rect2AnchoredPosition.x + 36 * scale)
                {
                    continue; // x축에서 겹치지 않음
                }

                TourNameTagBoolArr[index] = false;

                //if (minus > 0)
                //{
                //    if (PropsController.Instance.TintTourData == TourNameTagArr[index].TargetTour) continue;

                //    // 두 사각형이 겹치지 않는지 확인 (AABB 충돌 감지)
                //    if (rect1AnchoredPosition.x + rect1SizeDelta.x * 0.5f * scale < rect2AnchoredPosition.x - 36 * scale
                //        || rect1AnchoredPosition.x - rect1SizeDelta.x * 0.5f * scale > rect2AnchoredPosition.x + 36 * scale)
                //    {
                //        continue; // x축에서 겹치지 않음
                //    }

                //    TourNameTagBoolArr[index] = false;
                //}
                //else
                //{
                //    if (PropsController.Instance.TintTourData == TourNameTagArr[i].TargetTour) continue;

                //    // 두 사각형이 겹치지 않는지 확인 (AABB 충돌 감지)
                //    if (rect1AnchoredPosition.x + 36 * scale < rect2AnchoredPosition.x - rect2SizeDelta.x * 0.5f * scale
                //        || rect1AnchoredPosition.x - 36 * scale > rect2AnchoredPosition.x + rect2SizeDelta.x * 0.5f * scale)
                //    {
                //        continue; // x축에서 겹치지 않음
                //    }

                //    TourNameTagBoolArr[i] = false;
                //}
            }
            // rect1이 rect2 보다 0에서 78 사이 이름표끼리 겹칠경우
            else
            {
                if (TourNameTagBoolArr[i] == false && TourNameTagBoolArr[index] == false) continue;
                else if (TourNameTagBoolArr[i] == false)
                {
                    if (rect1AnchoredPosition.x + rect1SizeDelta.x * 0.5f * scale < rect2AnchoredPosition.x - 36 * scale
                    || rect1AnchoredPosition.x - rect1SizeDelta.x * 0.5f * scale > rect2AnchoredPosition.x + 36 * scale) continue;
                }
                else if (TourNameTagBoolArr[index] == false)
                {
                    if (rect1AnchoredPosition.x + 36 * scale < rect2AnchoredPosition.x - rect2SizeDelta.x * 0.5f * scale
                    || rect1AnchoredPosition.x - 36 * scale > rect2AnchoredPosition.x + rect2SizeDelta.x * 0.5f * scale) continue;
                }
                else
                {
                    if (rect1AnchoredPosition.x + rect1SizeDelta.x * 0.5f * scale < rect2AnchoredPosition.x - rect2SizeDelta.x * 0.5f * scale
                    || rect1AnchoredPosition.x - rect1SizeDelta.x * 0.5f * scale > rect2AnchoredPosition.x + rect2SizeDelta.x * 0.5f * scale) continue;


                    if (PropsController.Instance.TintTourData == TourNameTagArr[index].TargetTour)
                        TourNameTagBoolArr[i] = false;
                    else if (PropsController.Instance.TintTourData == TourNameTagArr[i].TargetTour)
                        TourNameTagBoolArr[index] = false;
                    else // if(minus > 0)
                    {
                        if (rect1AnchoredPosition.x - rect1SizeDelta.x * 0.5f * scale < rect2AnchoredPosition.x || rect1AnchoredPosition.x + rect1SizeDelta.x * 0.5f * scale > rect2AnchoredPosition.x)
                            TourNameTagBoolArr[index] = false;
                        else
                            TourNameTagBoolArr[i] = false;
                    }
                    //else
                    //{
                    //    if (rect2AnchoredPosition.x - rect2SizeDelta.x * 0.5f * scale < rect1AnchoredPosition.x || rect2AnchoredPosition.x + rect2SizeDelta.x * 0.5f * scale > rect1AnchoredPosition.x)
                    //        TourNameTagBoolArr[i] = false;
                    //    else
                    //        TourNameTagBoolArr[index] = false;
                    //}

                    continue;
                }

                if (PropsController.Instance.TintTourData == TourNameTagArr[index].TargetTour)
                    TourNameTagBoolArr[i] = false;
                else if (PropsController.Instance.TintTourData == TourNameTagArr[i].TargetTour)
                    TourNameTagBoolArr[index] = false;
                else
                    TourNameTagBoolArr[i] = false;
                //else if (minus > 0)
                //    TourNameTagBoolArr[i] = false;
                //else
                //    TourNameTagBoolArr[index] = false;

            }
            //{
            //    // 두 사각형이 겹치지 않는지 확인 (AABB 충돌 감지)
            //    if (rect1AnchoredPosition.x + rect1SizeDelta.x / 2 * scale < rect2AnchoredPosition.x - rect2SizeDelta.x / 2 * scale
            //        || rect1AnchoredPosition.x - rect1SizeDelta.x / 2 * scale > rect2AnchoredPosition.x + rect2SizeDelta.x / 2 * scale)
            //    {
            //        continue; // x축에서 겹치지 않음
            //    }
            //}


            //if (rect1AnchoredPosition.y + rect1SizeDelta.y * 1.5f * scale < rect2AnchoredPosition.y - rect2SizeDelta.y / 2 * scale
            //    || rect1AnchoredPosition.y - rect1SizeDelta.y / 2 * scale > rect2AnchoredPosition.y + rect2SizeDelta.y * 1.5f * scale)
            //{
            //    continue; // y축에서 겹치지 않음
            //}


            // 여기까지 오면 겹친거임

            //print(TourNameTagArr[i].TargetTour.ServerTourInfo.title + "와 " + TourNameTagArr[index].TargetTour.ServerTourInfo.title + "가 겹친다");
        }
    }
}
