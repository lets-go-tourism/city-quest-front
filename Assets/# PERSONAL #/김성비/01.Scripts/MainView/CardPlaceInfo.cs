using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System;

public class CardPlaceInfo : MonoBehaviour
{
    public Transform[] info;

    public enum Type
    {
        REVEAL,
        UNREVEAL
    }
    public Type type;

    public ServerAdventurePlace ServerAdventurePlace { get; private set; }
    public ServerProp ServerProp { get; private set; }

    public IEnumerator Start2()
    {
        while (true)
        {
            string meter = ConvertDistance(GPS.Instance.GetDistToUserInRealWorld(ServerProp.latitude, ServerProp.longitude)).ToString();
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
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            //texture.Compress(false);
            //texture.Apply(false, true);
            info[2].GetComponent<RawImage>().texture = texture;
        }
    }

    public void SettingPlaceType(bool reveal)
    {
        if (reveal) { type = Type.REVEAL; }
        else if (!reveal) { type = Type.UNREVEAL; }
    }

    public void SetServerProp(ServerAdventurePlace prop)
    {
        this.ServerAdventurePlace = prop;
    }

    public void setPlaceProp(ServerProp serverProp)
    {
        this.ServerProp = serverProp;
    }

    public void SendPlaceInfo()
    {
        for (int i = 0; i < BottomSheetManager.instance.contentPlace.childCount; i++)
        {
            BottomSheetManager.instance.contentPlace.GetChild(i).GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[0];
        }
        transform.GetChild(0).GetComponent<Image>().sprite = GetComponent<SpritesHolder>().sprites[1];

        MapCameraController.Instance.StartCameraMoveToTarget(PropsController.Instance.ServerAdventurePlaceWorldDic[this.ServerAdventurePlace].transform.position);
    }

    ///public class HomeAdventurePlace
    ///{
    ///    public long adventureNo;
    ///    public string name;
    ///    public string difficulty;
    ///    public string imageUrl;
    ///    public double distance;
    ///    public bool status;
    ///}
    ///
    public void ChangeType()
    {
        type = Type.REVEAL;
    }
}
