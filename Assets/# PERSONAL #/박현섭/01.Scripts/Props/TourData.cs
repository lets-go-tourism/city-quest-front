using Gpm.Common.ThirdParty.MessagePack.Resolvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourData : MonoBehaviour
{
    public ServerTourInfo ServerTourInfo { get; private set; }
    public GameObject MeshGO { get; private set; }

    public bool TourDataAcitve { get { return tourDataActive; }
        private set 
        {
            if (value == tourDataActive)
                return;

            tourDataActive = value;

            if (value)
            {
                MapUIController.Instance.NameTagContainer.AddTarget(this);
                MeshGO.SetActive(true);
            }
            else
            {
                MapUIController.Instance.NameTagContainer.RemoveTarget(this);
                MeshGO.SetActive(false);
            }
        }
    }

    private bool tourDataActive = false;

    public void Setting(ServerTourInfo serverTourInfo, GameObject go)
    {
        this.ServerTourInfo = serverTourInfo;
        this.MeshGO = Instantiate(go, transform);

        //for(int i = 0; i < MeshGO.transform.childCount; i++)
        //{
        //    MeshGO.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("CameraOverlay");
        //}
    }

    private float updateTime = 0;

    private void Update()
    {
        float sizeScale = Camera.main.transform.position.y / 500 * 0.55f;
        transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);

        updateTime += Time.deltaTime;

        if (updateTime <= 0.2f)
            return;

        if ((Camera.main.transform.position - new Vector3(0, Camera.main.transform.position.y, 0) - transform.position).sqrMagnitude > 100000)
            TourDataAcitve = false;
        else
            TourDataAcitve = true;

        updateTime = 0;
    }

}
