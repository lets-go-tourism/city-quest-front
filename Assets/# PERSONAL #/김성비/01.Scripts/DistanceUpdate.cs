using UnityEngine;

public class DistanceUpdate : MonoBehaviour
{


    void Start()
    {
        InvokeRepeating("YourFunction", 5f, 1f);
    }


}
