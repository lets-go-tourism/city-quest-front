using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PropModeling : MonoBehaviour
{
    public static PropModeling instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject[] models;

    public GameObject[] clouds;

    public void ModelingActive(int propNo)
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].SetActive(false);
        }
        models[propNo].SetActive(true);

        for (int j = 0; j < clouds.Length; j++)
        {
            clouds[j].SetActive(false);
        }
        clouds[propNo].SetActive(true);
    }

    public void CloudsOff()
    {
        for(int i = 0;i < clouds.Length; i++)
        {
            clouds[i].SetActive(false);
        }
    }
}
