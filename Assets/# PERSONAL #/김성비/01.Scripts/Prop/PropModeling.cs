using UnityEngine;

public class PropModeling : MonoBehaviour
{
    public static PropModeling instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject[] models;

    public void ModelingActive(int propNo)
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].SetActive(false);
        }
        models[propNo].SetActive(true);
    }
}