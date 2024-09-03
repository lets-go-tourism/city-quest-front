using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourDataController : MonoBehaviour
{
    public static TourDataController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public GameObject tourDataPref;

    public Dictionary<long, ServerTourInfo> ServerTourInfoDic { get; private set; } = new Dictionary<long, ServerTourInfo>();
    public Dictionary<ServerTourInfo, TourData> TourInfoWordList { get; private set; } = new Dictionary<ServerTourInfo, TourData>();
    public Dictionary<long, GameObject> ContentTypeGODic { get; private set; } = new Dictionary<long, GameObject>();

    private IEnumerator Start()
    {
        ContentTypeGODic.Add(12, (GameObject)Resources.Load("TourDataIconMesh/Icon_Mountain"));
        ContentTypeGODic.Add(14, (GameObject)Resources.Load("TourDataIconMesh/Icon_Art"));
        ContentTypeGODic.Add(15, (GameObject)Resources.Load("TourDataIconMesh/Icon_Community"));
        ContentTypeGODic.Add(25, (GameObject)Resources.Load("TourDataIconMesh/Icon_Travel"));
        ContentTypeGODic.Add(28, (GameObject)Resources.Load("TourDataIconMesh/Icon_Leisure"));
        ContentTypeGODic.Add(32, (GameObject)Resources.Load("TourDataIconMesh/Icon_Car"));
        ContentTypeGODic.Add(38, (GameObject)Resources.Load("TourDataIconMesh/Icon_Store"));
        ContentTypeGODic.Add(39, (GameObject)Resources.Load("TourDataIconMesh/Icon_Food"));

        while (DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

        List<ServerTourInfo> tourList = DataManager.instance.GetHometourPlacesList();

        for(int i = 0; i < tourList.Count; i++)
        { 
            GameObject obj = Instantiate(tourDataPref, transform);

            if (obj == null)
                Debug.Log("컨텐츠 타입이 잘못되었습니다");

            float x = (float)MercatorProjection.lonToX(double.Parse(tourList[i].longitude));
            float y = (float)MercatorProjection.latToY(double.Parse(tourList[i].latitude));

            Vector3 objPosition = new Vector3(x, 0, y) - MapReader.Instance.boundsCenter;
            obj.transform.position = objPosition + new Vector3(0, 10, 0);

            TourInfoWordList.Add(tourList[i], obj.GetComponent<TourData>());
            obj.GetComponent<TourData>().Setting(tourList[i], ContentTypeGODic[int.Parse(tourList[i].contenttypeid)]);
        }
    }
}
