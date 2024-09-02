using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourData : MonoBehaviour
{
    public ServerTourInfo ServerTourInfo { get; private set; }
    public GameObject MeshGO { get; private set; }

    public void Setting(ServerTourInfo serverTourInfo, GameObject go)
    {
        this.ServerTourInfo = serverTourInfo;
        this.MeshGO = Instantiate(go, transform);

    }

}
