using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

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
            string meter = ((int)GPS.Instance.GetDistToUserInRealWorld(ServerProp.latitude, ServerProp.longitude)).ToString();
            info[1].GetComponent<TextMeshProUGUI>().text = meter + 'm';
            yield return new WaitForSeconds(5);
        }
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
}
