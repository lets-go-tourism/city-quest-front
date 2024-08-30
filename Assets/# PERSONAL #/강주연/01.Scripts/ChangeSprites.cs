using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprites : MonoBehaviour
{

    [HideInInspector] public Transform[] place;
    [HideInInspector] public Transform[] tour;

    public void ChangePlaceSprites(int no)
    {
        for (int i = 0; i < place.Length; i++) 
        {
            place[i].GetComponent<Image>().sprite = place[i].GetComponent<SpritesHolder>().sprites[0];  // 안눌림
        }

        place[no].GetComponent<Image>().sprite = place[no].GetComponent<SpritesHolder>().sprites[1];    // 눌림

    }

    public void ChangeTourSprites(int no)
    {
        for(int i = 0; i < tour.Length ; i++)
        {
            tour[i].GetComponent<Image>().sprite = tour[i].GetComponent<SpritesHolder>().sprites[0];    // 안눌림
        }

        tour[no].GetComponent<Image>().sprite = tour[no].GetComponent<SpritesHolder>().sprites[1];        // 눌림
    }
}
