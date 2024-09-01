using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudContainer : MonoBehaviour
{
    public static CloudContainer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public Cloud[] CloudArr { get; private set; }

    private void Start()
    {
        CloudArr = new Cloud[transform.childCount];
        for(int i = 0; i < CloudArr.Length; i++)
        {
            CloudArr[i] = transform.GetChild(i).GetComponent<Cloud>();
        }
    }

    public void AddTarget(Prop target)
    {
        for(int i = 0; i < CloudArr.Length; i++)
        {
            if (CloudArr[i].enabled)
                continue;

            CloudArr[i].StartSetting(target);
            break;
        }
    }

    public void RemoveTarget(Prop target)
    {
        for (int i = 0; i < CloudArr.Length; i++)
        {
            if (CloudArr[i].TargetProp != target)
                continue;

            CloudArr[i].enabled = false;
            break;
        }
    }
}
