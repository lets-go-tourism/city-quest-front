using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourData : MonoBehaviour
{
    public ServerTourInfo ServerTourInfo { get; private set; }

    public void StartSetting(ServerTourInfo serverTourInfo)
    {
        this.ServerTourInfo = serverTourInfo;
    }

}
