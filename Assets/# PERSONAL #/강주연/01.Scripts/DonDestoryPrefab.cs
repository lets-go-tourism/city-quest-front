using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestoryPrefab : MonoBehaviour
{
    public static DonDestoryPrefab instance;
 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}
