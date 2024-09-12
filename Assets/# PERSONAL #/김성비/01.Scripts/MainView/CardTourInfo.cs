using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System;

public class CardTourInfo : MonoBehaviour
{
    public Transform[] info;

    public enum Type
    {
        TouristSpot,
        CulturalFacilities,
        Festival,
        TravelCource,
        LeisureSports,
        Lodgment,
        Shopping,
        Food
    }
    public Type type;

    public ServerTourInfo ServerTourInfo { get; private set; }
    bool selected;

    public IEnumerator Start2()
    {
        while (true)
        {
            string meter = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(double.Parse(ServerTourInfo.latitude), double.Parse(ServerTourInfo.longitude))).ToString();
            info[1].GetComponent<TextMeshProUGUI>().text = meter;
            yield return new WaitForSeconds(5);
        }
    }

    string ConvertDistance(double distance)
    {
        string result = string.Empty;

        double tmp = distance;
        double a = 1000;
        if (tmp > a)
        {
            double calcultate = Math.Round(tmp / a, 1);
            result = calcultate.ToString() + "km";
        }
        else
        {
            result = tmp.ToString() + "m";
        }

        return result;
    }

    // �̹��� ����
    public IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            //Texture2D my = new Texture2D(myTexture.width, myTexture.height, TextureFormat.RGB24, false);
            //my.SetPixels(myTexture.GetPixels());
            //my.Compress(false);
            //my.Apply(false, true);
            info[2].GetComponent<RawImage>().texture = myTexture;
        }
    }

    // Ÿ�� ����
    public void SettingTourType(string str)
    {
        int no = 0;
        no = int.Parse(str);

        if (no == 12) { type = Type.TouristSpot; }
        else if (no == 14) { type = Type.CulturalFacilities; }
        else if (no == 15) { type = Type.Festival; }
        else if (no == 25) { type = Type.TravelCource; }
        else if (no == 28) { type = Type.LeisureSports; }
        else if (no == 32) { type = Type.Lodgment; }
        else if (no == 38) { type = Type.Shopping; }
        else if (no == 39) { type = Type.Food; }
    }


    public void InputTourList(ServerTourInfo serverTourInfo)
    {
        this.ServerTourInfo = serverTourInfo;
    }

    public void SendTourInfo()
    {
        // �̼��� -> ����
        if (!selected)
        {
            Selected(true);

            for (int i = 0; i < BottomSheetManager.instance.contentTour.childCount; i++)
            {
                BottomSheetManager.instance.contentTour.GetChild(i).GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[0];
            }
            transform.GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[1];

            MapCameraController.Instance.StartCameraMoveToTarget(TourDataController.Instance.TourInfoWordList[ServerTourInfo].transform.position);
        }

        // 
        else
        {
            Selected(false);
        }
    }

    public void Selected(bool isSelected)
    {
        if (isSelected)
        {
            selected = true;
        }
        else
        {
            selected = false;
        }
    }
    //string TypeConvert(string str)
    //{
    //    string result = string.Empty;

    //    if(str == 12.ToString()) { result = "TouristSpot"; }
    //    else if(str == 14.ToString()) { result = "CulturalFacilities"; }
    //    else if(str == 15.ToString()) { result = "Festival"; }
    //    else if(str == 25.ToString()) { result = "TravelCource"; }
    //    else if(str == 28.ToString()) { result = "LeisureSports"; }
    //    else if(str == 32.ToString()) { result = "Lodgment"; }
    //    else if(str == 38.ToString()) { result = "Shopping"; }
    //    else if(str == 39.ToString()) { result = "Food"; }

    //    return result;
    //}

    ///[System.Serializable]
    ///public class HometourPlace
    ///{
    ///    public string distance;
    ///    public string latitude;
    ///    public string imageUrl;
    ///    public string addr;
    ///    public string title;
    ///    public string contenttypeid;
    ///    public string longitude;
    ///}

    /// contenttypeid
    /// ������ : 12
    /// ��ȭ�ü� : 14
    /// ����/����/��� : 15
    /// �����ڽ� : 25
    /// ������ : 28
    /// ���� : 32
    /// ���� : 38
    /// ���� : 39
}
