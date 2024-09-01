using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPropContent : MonoBehaviour
{
    public List<Transform> content;

    public GameObject[] prefabsNO;
    public GameObject[] prefabsYES;
    public Transform parent;

    public static SettingPropContent instance;
    private void Awake()
    {
        instance = this;
        content = new List<Transform>();
    }

    //public enum State
    //{
    //    NO,YES
    //}
    //public State state;
    
    public IEnumerator SettingNO()
    {
        yield return StartCoroutine(ResetContent());

        for (int j = 0; j < prefabsNO.Length; j++)
        {
            GameObject go = Instantiate(prefabsNO[j], parent);

            content.Add(go.transform);
        }

        yield return null;
    }

    public IEnumerator SettingYES()
    {
        yield return StartCoroutine(ResetContent());

        for (int j = 0; j < prefabsYES.Length; j++)
        {
            GameObject go = Instantiate(prefabsYES[j], parent);

            content.Add(go.transform);
        }

        yield return null;
    }

    IEnumerator ResetContent()
    {
        if (parent.childCount > 0)
        {
            content.RemoveRange(0, parent.childCount);

            for (int i = 0; i < parent.childCount; i++)
            {
                GameObject dgo = parent.GetChild(i).gameObject;
                Destroy(dgo);
            }
        }

        yield break;
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if(state  == State.NO)
    //        SettingNO();
    //        else SettingYES();
    //    }

    //}
}