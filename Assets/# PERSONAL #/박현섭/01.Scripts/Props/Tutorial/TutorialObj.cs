using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObj : MonoBehaviour
{
    private Transform _cloud;
    private Transform _coffee;

    private Transform _targetCoffee;

    private void Start()
    {
        _cloud = transform.GetChild(0);
        _coffee = transform.GetChild(1);
    }

    public void Setting()
    {
        //_targetCoffee = PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[]].transform;
    }
}
