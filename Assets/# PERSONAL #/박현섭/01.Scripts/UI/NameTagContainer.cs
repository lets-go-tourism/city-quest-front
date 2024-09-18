using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NameTagContainer : MonoBehaviour
{
    public NameTagUI[] NameTagArr { get; private set; } = new NameTagUI[31];

    private int nameTagCount = 0;

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            NameTagArr[i] = transform.GetChild(i).GetComponent<NameTagUI>();
            nameTagCount++;
        }
    }

    public void AddTarget(Prop target)
    {
        for(int i = 0; i < nameTagCount; i++)
        {
            if (NameTagArr[i].enabled)
                continue;

            NameTagArr[i].Init(target);
            break;
        }
    }

    public void AddTarget(TourData target)
    {
        for (int i = 0; i < nameTagCount; i++)
        {
            if (NameTagArr[i].enabled)
                continue;

            NameTagArr[i].Init(target);
            break;
        }
    }

    public void RemoveTarget(Prop target)
    {
        for (int i  = 0; i < nameTagCount; i++)
        {
            if (NameTagArr[i].TargetProp != target)
                continue;

            NameTagArr[i].enabled = false;
            break;
        }
    }

    public void RemoveTarget(TourData target)
    {
        for (int i = 0; i < nameTagCount; i++)
        {
            if (NameTagArr[i].TourData != target)
                continue;

            NameTagArr[i].enabled = false;
            break;
        }
    }
}
