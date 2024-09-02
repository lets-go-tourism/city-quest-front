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


    public Dictionary<long, ServerTourInfo> ServerTourInfoDic { get; private set; } = new Dictionary<long, ServerTourInfo>();
    public Dictionary<ServerTourInfo, TourData> TourInfoWordList { get; private set; } = new Dictionary<ServerTourInfo, TourData>();


    // 오른쪽에 ContentTypeId를 적어놓았다
    [SerializeField] private GameObject m_tourSpotPref; // 관광지 12
    [SerializeField] private GameObject m_culturalSpotPref; // 문화시설 14
    [SerializeField] private GameObject m_eventSpotPref; // 축제/공연/행사 15
    [SerializeField] private GameObject m_travelCorseSpotPref; // 여행코스 25
    [SerializeField] private GameObject m_sportSpotPref; // 레저 / 스포츠 28
    [SerializeField] private GameObject m_accommodationSpotPref; // 숙박 32
    [SerializeField] private GameObject m_shoppingSpotPref; // 쇼핑 38
    [SerializeField] private GameObject m_foodSpotPref; // 음식 39

    private IEnumerator Start()
    {
        while (DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }

        List<ServerTourInfo> tourList = DataManager.instance.GetHometourPlacesList();

        for(int i = 0; i < tourList.Count; i++)
        {
            GameObject obj = null;
            switch (tourList[i].contenttypeid)
            {
                case "12": obj = Instantiate(m_tourSpotPref, transform); break;
                case "14": obj = Instantiate(m_culturalSpotPref, transform); break;
                case "15": obj = Instantiate(m_eventSpotPref, transform); break;
                case "25": obj = Instantiate(m_travelCorseSpotPref, transform); break;
                case "28": obj = Instantiate(m_sportSpotPref, transform); break;
                case "32": obj = Instantiate(m_accommodationSpotPref, transform); break;
                case "38": obj = Instantiate(m_shoppingSpotPref, transform); break;
                case "39": obj = Instantiate(m_foodSpotPref, transform); break;
            }

            if (obj == null)
                Debug.Log("컨텐츠 타입이 잘못되었습니다");

            float x = (float)MercatorProjection.lonToX(double.Parse(tourList[i].longitude));
            float y = (float)MercatorProjection.latToY(double.Parse(tourList[i].latitude));

            Vector3 objPosition = new Vector3(x, 0, y) - MapReader.Instance.boundsCenter;
            obj.transform.position = objPosition;

            TourInfoWordList.Add(tourList[i], obj.GetComponent<TourData>());
            obj.GetComponent<TourData>().StartSetting(tourList[i]);
        }
    }
}
