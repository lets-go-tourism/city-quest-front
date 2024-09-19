using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NameTagContainer : MonoBehaviour
{
    public PropNameTagUI[] PropNameTagArr { get; private set; } = new PropNameTagUI[31];
    public TourNameTagUI[] TourNameTagArr { get; private set; } = new TourNameTagUI[31];

    private int _propNameTagCount = 0;
    private int _tourNameTagCount = 0;

    private CheckNameTagCollision _checkNameTagCollision;

    private void Start()
    {
        _checkNameTagCollision = GetComponent<CheckNameTagCollision>();

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            PropNameTagArr[i] = transform.GetChild(0).GetChild(i).GetComponent<PropNameTagUI>();
            _propNameTagCount++;
        }

        for(int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            TourNameTagArr[i] = transform.GetChild(1).GetChild(i).GetComponent<TourNameTagUI>();
            _tourNameTagCount++;
        }

        StartCoroutine(nameof(CollisionUpdate));
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

    private IEnumerator CollisionUpdate()
    {
        while(true)
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
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void UpdateCheckNameTagCollision(int index)
    {
        RectTransform rect1 = TourNameTagArr[index].RectTransform;

        for (int i = 0; i < _tourNameTagCount; i++)
        {
            if (TourNameTagArr[i].enabled == false || i == index)
                continue;

            TourNameTagArr[i].CustomeUpdate();
            RectTransform rect2 = TourNameTagArr[i].RectTransform;
            

            // �� �簢���� ��ġ�� �ʴ��� Ȯ�� (AABB �浹 ����)
            if (rect1.anchoredPosition.x + rect1.sizeDelta.x / 2 < rect2.anchoredPosition.x - rect2.sizeDelta.x / 2 || rect1.anchoredPosition.x - rect1.sizeDelta.x / 2 > rect2.anchoredPosition.x + rect2.sizeDelta.x / 2)
            {
                continue; // x�࿡�� ��ġ�� ����
            }

            if (rect1.anchoredPosition.y + rect1.sizeDelta.y / 2 < rect2.anchoredPosition.y - rect2.sizeDelta.y / 2 || rect1.anchoredPosition.y - rect1.sizeDelta.y / 2 > rect2.anchoredPosition.y + rect2.sizeDelta.y / 2)
            {
                continue; // y�࿡�� ��ġ�� ����
            }

            // ������� ���� ��ģ����

            //print(TourNameTagArr[i].TargetTour.ServerTourInfo.title + "�� " + TourNameTagArr[index].TargetTour.ServerTourInfo.title + "�� ��ģ��");

            if (TourNameTagArr[i].RectTransform.sizeDelta.x > TourNameTagArr[index].RectTransform.sizeDelta.x)
                TourNameTagArr[index].Visible = false;
            else
                TourNameTagArr[i].Visible = false;
        }
    }
}
