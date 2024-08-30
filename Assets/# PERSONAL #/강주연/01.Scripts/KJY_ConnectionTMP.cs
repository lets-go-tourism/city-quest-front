using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#region ImageConnection
[System.Serializable]
public class ImageSetting
{
    public string url;
    public int questNo;
    public byte[] data;
}

public class ImageResponse
{
    // Define your response fields here if needed
}

public class TryImageConnection : MonoBehaviour
{
    private int questNo;
    private string url;
    private byte[] data;

    public void Initialize(ImageSetting setting)
    {
        this.url = setting.url;
        this.data = setting.data;
        this.questNo = setting.questNo;

        Debug.Log(url);
        SendImage();
    }

    private void SendImage()
    {
        StartCoroutine(UploadImage());
    }

    private IEnumerator UploadImage()
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", data, "image.jpg", "image/jpg");
        form.AddField("questNo", questNo.ToString());

         
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            //www.SetRequestHeader("Content-Type", "multipart/form-data");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
            }
            else
            {
                Debug.Log("Image upload complete!");
                Complete(www.downloadHandler);
            }
        }
    }

    private void Complete(DownloadHandler result)
    {
        ImageResponse response = JsonUtility.FromJson<ImageResponse>(result.text);
    }
}
#endregion

#region HomeConnection
[System.Serializable]
public class HomeSetting
{
    public string url;
    public double latitude;
    public double longitude;
}

[System.Serializable]
public class HomeRequest
{
    public double latitude;
    public double longitude;
}

[System.Serializable]
public class HomeResponse
{
    public DateTime timeStamp;
    public string status;
    public HomeData data;
}

[System.Serializable]
public class HomeData
{
    public List<HomeProps> props;
    public List<HomeAdventurePlace> adventurePlace;
    public List<HometourPlace> tourPlace;
}

[System.Serializable]
public class HomeProps
{
    public long propNo;
    public string name;
    public double longtitude;
    public double latitude;
    public bool status;
}

[System.Serializable]
public class HomeAdventurePlace
{
    public long adventureNo;
    public string name;
    public string difficulty;
    public string imageUrl;
    public double distance;
    public bool status;
}

[System.Serializable]
public class HometourPlace
{
    public string distance;
    public string latitude;
    public string imageUrl;
    public string addr;
    public string title;
    public string contenttypeid;
    public string longitude;
}

[System.Serializable]
public class TryHomeConnection : ConnectionStratage
{
    private string url;
    private double latitude;
    private double longitude;

    public TryHomeConnection(HomeSetting setting)
    {
        this.url = setting.url;
        this.latitude = setting.latitude;
        this.longitude = setting.longitude;

        CreateJson();
    }

    private void CreateJson()
    {
        HomeRequest request = new HomeRequest();
        request.latitude = latitude;
        request.longitude = longitude;

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }

    private void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        Debug.Log(this.url);
        request.Setting(RequestType.GET, this.url);
        request.body = jsonData;
        request.complete = Complete;

        HttpManager.instance.SendRequest(request);
    }

    private void Complete(DownloadHandler result)
    {
        HomeResponse response = new HomeResponse();
        response = JsonUtility.FromJson<HomeResponse>(result.text);

        if (response.status == "OK")
        {
            DataManager.instance.SetHomePropsList(response.data.props);
            DataManager.instance.SetHomeAdventurePlaceList(response.data.adventurePlace);
            DataManager.instance.SetHometourPlaceList(response.data.tourPlace);
        }
        else
        {
            Debug.Log(result.error);
        }
    }

}
#endregion

#region
public class QuestSetting
{
    public string url;
}

public class QuestRequest
{
    private string url;
}

public class QuestResponse
{
    public DateTime timeStamp;
    public string status;
    public QuestData data;
}

public class QuestData
{
    private string locationName;
    private string addr;
    private string kakaoMapUrl;
    private string imageUrl;
    private long propNo;
    private bool status;
    private string difficulty;
    private string questDesc;
    private double distance;
    private DateTime date;
    private string questImage;
}

public class TryQuestConnection:ConnectionStratage
{
    private string url;

    public TryQuestConnection(QuestSetting setting)
    {
        this.url = setting.url;
        CreateJson();
    }

    private void CreateJson()
    {
        QuestRequest request = new QuestRequest();

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }

    private void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        Debug.Log(this.url);
        request.Setting(RequestType.GET, this.url);
        request.body = jsonData;
        request.complete = Complete;

        HttpManager.instance.SendRequest(request);
    }

    private void Complete(DownloadHandler result)
    {
        QuestResponse response = new QuestResponse();
        response = JsonUtility.FromJson<QuestResponse>(result.text);

        if (response.status == "OK")
        {
            DataManager.instance.SetQuestInfo(response.data);
        }
    }
}
#endregion

public class KJY_ConnectionTMP : MonoBehaviour
{
    public static KJY_ConnectionTMP instance;

    [SerializeField] private GameObject text;

    private void Awake()
    {
        instance = this;
    }

    public void OnClickTest(Texture2D texture, int questNo)//카메라 통신하는 정보
    {
        if (texture == null)
        {
            StartCoroutine(successText());
            return;
        }

        ImageSetting setting = new ImageSetting
        {
            questNo = questNo,
            data = texture.EncodeToJPG(),
            url = "http://43.203.101.31:8080/api/v1/quest/image3"
        };

        TryImageConnection tryImageConnection = gameObject.AddComponent<TryImageConnection>();
        tryImageConnection.Initialize(setting);
    }

    public IEnumerator successText()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(1);
        text.SetActive(false);
    }

    public void OnClickHomeConnection() //홈정보 통신하는 함수
    {
        HomeSetting setting = new HomeSetting();
        LocationInfo info = DataManager.instance.GetGPSInfo();
        setting.latitude = info.latitude;
        setting.longitude = info.longitude;
        setting.url = "http://43.203.101.31:8080/api/v1/home?" + "lon=" + setting.longitude + "&lat=" + setting.latitude;

        TryHomeConnection homeConnection = new TryHomeConnection(setting);
    }

    public void OnConnectionQuest(int questNo) //퀘스트 팝업 통신하는 함수
    {
        QuestSetting setting = new QuestSetting();
        LocationInfo info = DataManager.instance.GetGPSInfo();
        setting.url = "http://43.203.101.31:8080/api/v1/quest?questNo=" + questNo + "&lon=" + info.longitude + "&lat=" + info.latitude;

        TryQuestConnection questConnection = new TryQuestConnection(setting);
    }
}
