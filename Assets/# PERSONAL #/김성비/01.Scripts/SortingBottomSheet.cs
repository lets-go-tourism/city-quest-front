using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingBottomSheet : MonoBehaviour
{
    // ���
    public GameObject cardPlace;
    public Transform contentPlace;

    // ��������
    public GameObject cardTour;
    public Transform contentTour;

    public static SortingBottomSheet instance;
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < 5; i++)
        {
            Instantiate(cardPlace, contentPlace);
            Instantiate(cardTour, contentTour);
        }
    }

    public void SortingPlace()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(cardPlace, contentPlace);
        }
    }
    public void SortingTour()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(cardTour, contentTour);
        }
    }
}
