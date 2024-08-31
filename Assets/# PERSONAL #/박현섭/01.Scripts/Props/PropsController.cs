using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsController : MonoBehaviour
{
    public Dictionary<long, HomeProps> PropDic { get; private set; } = new Dictionary<long, HomeProps>();
    public List<HomeProps> PropList { get; private set; } = new List<HomeProps>();

    public Dictionary<long, HomeAdventurePlace> AdventurePlaceDic { get; private set; } = new Dictionary<long, HomeAdventurePlace>();

    [SerializeField] private GameObject propPref;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        PropList = DataManager.instance.GetHomePropsList();
        List<HomeAdventurePlace> placeList = DataManager.instance.GetHomeAdventurePlacesList();

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
    private void CreateProp(HomeProps propData, HomeAdventurePlace homeAdventurePlace)
    {
        GameObject obj = Instantiate(propPref, this.transform);
        Prop objProp = obj.GetComponent<Prop>();
        
        // ������ ���� �����͸� �־���
        objProp.Init(propData, homeAdventurePlace);
        
        // ������ ���� ��ǥ�� ����
        float x = (float)MercatorProjection.lonToX(propData.longitude);
        float y = (float)MercatorProjection.latToY(propData.latitude);

        Vector3 objPosition = new Vector3(x, 0, y) - MapReader.Instance.bounds.Center;

        obj.transform.position = objPosition;
    }


}
