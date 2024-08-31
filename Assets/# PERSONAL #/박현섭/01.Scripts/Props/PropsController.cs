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

    // 프랍을 생성
    private void CreateProp(HomeProps propData, HomeAdventurePlace homeAdventurePlace)
    {
        GameObject obj = Instantiate(propPref, this.transform);
        Prop objProp = obj.GetComponent<Prop>();
        
        // 프랍에 프랍 데이터를 넣어줌
        objProp.Init(propData, homeAdventurePlace);
        
        // 프랍의 월드 좌표를 구함
        float x = (float)MercatorProjection.lonToX(propData.longitude);
        float y = (float)MercatorProjection.latToY(propData.latitude);

        Vector3 objPosition = new Vector3(x, 0, y) - MapReader.Instance.bounds.Center;

        obj.transform.position = objPosition;
    }


}
