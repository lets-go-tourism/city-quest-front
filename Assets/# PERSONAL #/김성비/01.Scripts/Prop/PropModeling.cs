using UnityEngine;

public class PropModeling : MonoBehaviour
{
    public static PropModeling instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject[] models;

    //public GameObject[] clouds;

    public GameObject goCloud;

    public void ModelingActive(int propNo, bool cloud)
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].SetActive(false);
        }
        models[propNo].SetActive(true);

        goCloud.SetActive(cloud);
    }

    //public void CloudsOnOff(bool cloud)
    //{
    //    for(int i = 0;i < clouds.Length; i++)
    //    {
    //        clouds[i].SetActive(cloud);
    //    }
    //}
}
