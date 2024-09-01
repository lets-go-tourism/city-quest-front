using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsController : MonoBehaviour
{
    public Dictionary<long, ServerProp> PropDic { get; private set; } = new Dictionary<long, ServerProp>();
    public List<ServerProp> PropList { get; private set; } = new List<ServerProp>();

    public Dictionary<long, ServerAdventurePlace> AdventurePlaceDic { get; private set; } = new Dictionary<long, ServerAdventurePlace>();

    [SerializeField] private GameObject propPref;


    private IEnumerator Start()
    {
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

    // ������ ����
    private void CreateProp(ServerProp propData, ServerAdventurePlace homeAdventurePlace)
    {
        GameObject obj = Instantiate(propPref, this.transform);
        Prop objProp = obj.GetComponent<Prop>();
        
        // ������ ���� �����͸� �־���
        objProp.Init(propData, homeAdventurePlace);
        
        // ������ ���� ��ǥ�� ����
        float x = (float)MercatorProjection.lonToX(propData.longitude);
        float y = (float)MercatorProjection.latToY(propData.latitude);

        Vector3 objPosition = new Vector3(x, 0, y) - MapReader.Instance.boundsCenter;

        obj.transform.position = objPosition;
    }
}
