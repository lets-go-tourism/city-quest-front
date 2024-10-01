using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsController : MonoBehaviour
{
    public static PropsController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public Dictionary<long, ServerProp> PropDic { get; private set; } = new Dictionary<long, ServerProp>();
    public List<ServerProp> PropList { get; private set; } = new List<ServerProp>();

    public Dictionary<long, ServerAdventurePlace> AdventurePlaceDic { get; private set; } = new Dictionary<long, ServerAdventurePlace>();
    public Dictionary<ServerAdventurePlace, Prop> ServerAdventurePlaceWorldDic { get; private set; } = new Dictionary<ServerAdventurePlace, Prop>();

    [SerializeField] private GameObject propPref;

    public Dictionary<long, GameObject> PropMeshDic { get; private set; } = new Dictionary<long, GameObject>();
    public Dictionary<long, Material> PropMaterial { get; private set; } = new Dictionary<long, Material>();

    public Prop TintProp { get { return tintProp; } set 
        { 
            tintProp = value;
            tintTourData = null;
        } 
    }
    private Prop tintProp;
    private Prop 몰루프랍;
    public TourData TintTourData { get { return tintTourData; } set 
        {
            tintTourData = value;
            tintProp = null;
            StartCoroutine(nameof(SetTintTourData));
        } 
    }

    private TourData tintTourData;
    private TourData 몰루투어데이터;

    private IEnumerator SetTintTourData()
    {
        yield return new WaitForSeconds(0.1f);
        MapUIController.Instance.NameTagContainer.CollisionUpdate();
    }

    private IEnumerator Start()
    {
        PropMeshDic.Add(1, (GameObject)Resources.Load("PropMeshData/SM_Crane"));
        //PropMeshDic.Add(3, (GameObject)Resources.Load("PropMeshData/SM_Pasta"));
        PropMeshDic.Add(2, (GameObject)Resources.Load("PropMeshData/SM_Churros"));
        PropMeshDic.Add(3, (GameObject)Resources.Load("PropMeshData/SM_coffee"));
        //PropMeshDic.Add(7, (GameObject)Resources.Load("PropMeshData/SM_PalDGate"));
        PropMeshDic.Add(4, (GameObject)Resources.Load("PropMeshData/SM_HhongGate"));
        PropMeshDic.Add(5, (GameObject)Resources.Load("PropMeshData/SM_Kbow"));

        while (DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

        PropList = DataManager.instance.GetHomePropsList();
        List<ServerAdventurePlace> placeList = DataManager.instance.GetHomeAdventurePlacesList();

        for (int i = 0; i < PropList.Count; i++)
        {
            PropDic.Add(PropList[i].propNo, PropList[i]);
        }

        for (int i = 0; i < placeList.Count; i++)
        {
            AdventurePlaceDic.Add(placeList[i].adventureNo, placeList[i]);
        }

        for (int i = 0; i < PropList.Count; i++)
        {
            CreateProp(PropList[i], AdventurePlaceDic[PropList[i].propNo]);
        }
    }

    // 프랍을 생성
    private void CreateProp(ServerProp propData, ServerAdventurePlace homeAdventurePlace)
    {
        GameObject obj = Instantiate(propPref, this.transform);
        Prop objProp = obj.GetComponent<Prop>();

        ServerAdventurePlaceWorldDic.Add(homeAdventurePlace, objProp);

        //GameObject propMesh = Resources.Load("PropMeshData/SM_" + )
        // 프랍에 프랍 데이터를 넣어줌

        GameObject meshProp = PropMeshDic.ContainsKey(propData.propNo) ? PropMeshDic[propData.propNo] : PropMeshDic[1];
        objProp.Init(propData, homeAdventurePlace, meshProp);
        
        // 프랍의 월드 좌표를 구함
        float x = (float)MercatorProjection.lonToX(propData.longitude);
        float y = (float)MercatorProjection.latToY(propData.latitude);

        Vector3 objPosition = new Vector3(x, 0, y) - MapReader.Instance.boundsCenter;

        obj.transform.position = objPosition;
    }
}
