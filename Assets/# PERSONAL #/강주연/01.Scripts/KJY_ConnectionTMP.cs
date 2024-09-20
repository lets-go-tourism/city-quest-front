using System;
using System.Collections;
using System.Collections.Generic;
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
    public DateTime timeStamp;
    public string status;
    public ImageData data;
}

public class ImageData
{
    public long completedQuestNo;
    public string questImageUrl;
}

public class TryImageConnection : MonoBehaviour
{
    private int questNo;
    private string url;
    private byte[] data;
    private LoginResponse login = new LoginResponse();

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

         
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            login = DataManager.instance.GetLoginData();

            www.SetRequestHeader("Authorization", login.data.accessToken);
            print(login.data.accessToken);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
                ToastMessage.ShowToast("이미지 업로드에 실패했어요");
                ButtonActions.Instance.QuestDone();
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
        ToastMessage.ShowToast("이미지를 업로드했어요");
        KJY_ConnectionTMP.instance.OnConnectionFailUI();

        ButtonActions.Instance.QuestDone();
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
    public List<ServerProp> props;
    public List<ServerAdventurePlace> adventurePlace;
    public List<ServerTourInfo> tourPlace;
}

[System.Serializable]
public class ServerProp
{
    public long propNo;
    public string name;
    public double longitude;
    public double latitude;
    public bool status;
    public Vector3 position;
}

[System.Serializable]
public class ServerAdventurePlace
{
    public long adventureNo;
    public string name;
    public string difficulty;
    public string imageUrl;
    public double distance;
    public bool status;
}

[System.Serializable]
public class ServerTourInfo
{
    public string distance;
    public string latitude;
    public string imageUrl;
    public string addr;
    public string title;
    public string contenttypeid;
    public string longitude;
    public Vector3 position;
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

        KJY_ConnectionTMP.instance.requestHttp = request;
        KJY_ConnectionTMP.instance.requestHeaderHttp = RequestHeader.other;

        HttpManager.instance.SendRequest(request, RequestHeader.other);
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
            DataManager.instance.requestSuccess = true;

            ToastMessage.ShowToast("불러오기에 성공했어요");
        }
        else
        {
            Debug.Log(result.error);
            ToastMessage.ShowToast("불러오기에 실패했어요 앱을 다시 시작해주세요");
            KJY_ConnectionTMP.instance.OnConnectionFailUI();
        }
    }

}
#endregion

#region QuestConnection

[System.Serializable]
public class QuestSetting
{
    public string url;
}

public class QuestRequest
{
    private string url;
}

[System.Serializable]
public class QuestResponse
{
    public DateTime timeStamp;
    public string status;
    public QuestData data;
}

[System.Serializable]
public class QuestData
{
    public string locationName;
    public string addr;
    public string kakaoMapUrl;
    public string imageUrl;
    public long propNo;
    public bool status;
    public string difficulty;
    public string questDesc;
    public double distance;
    public string date;
    public string questImage;
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

        KJY_ConnectionTMP.instance.requestHttp = request;
        KJY_ConnectionTMP.instance.requestHeaderHttp = RequestHeader.other;
        HttpManager.instance.SendRequest(request, RequestHeader.other);
    }

    private void Complete(DownloadHandler result)
    {
        QuestResponse response = new QuestResponse();
        response = JsonUtility.FromJson<QuestResponse>(result.text);

        if (response.status == "OK")
        {
            DataManager.instance.SetQuestInfo(response.data);
            // 성공하면 여기
        }
        else
        {
            ToastMessage.ShowToast("프랍 데이터를 불러오지 못했어요 다시 시도해 주세요");
            KJY_ConnectionTMP.instance.OnConnectionFailUI();

        }
    }
}
#endregion

#region LoginConnection_justUseClass

//[System.Serializable]
//public class LoginSetting
//{
//    public string url;
//}

//[System.Serializable]
//public class LoginRequest
//{
//    public string url;
//}

//[System.Serializable]
//public class LoginResponse
//{
//    public DateTime timeStamp;
//    public string status;
//    public LoginData data;
//}

//[System.Serializable]
//public class LoginData
//{
//    public string accessToken;
//    public string refreshToken;
//    public string tokenType;
//}

//public class TryLoginConnection : ConnectionStratage
//{
//    private string url;

//    public TryLoginConnection(LoginSetting setting)
//    {
//        this.url = setting.url;

//        CreateJson();
//    }

//    private void CreateJson()
//    {
//        LoginRequest request = new LoginRequest();

//        string jsonData = JsonUtility.ToJson(request);
//        OnGetRequest(jsonData);
//    }

//    private void OnGetRequest(string jsonData)
//    {
//        HttpRequester request = new HttpRequester();

//        Debug.Log(this.url);
//        request.Setting(RequestType.GET, this.url);
//        request.body = jsonData;
//        request.complete = Complete;

//        HttpManager.instance.SendRequest(request, );
//    }

//    private void Complete(DownloadHandler result)
//    {
//        LoginResponse response = new LoginResponse();
//        response = JsonUtility.FromJson<LoginResponse>(result.text);

//        if (response.status == "OK")
//        {
//            DataManager.instance.SetLoginData(response.data);
//            HttpManager.instance.loginData = response.data;
//        }
//    }
//}
#endregion

#region ConfirmConnection
[System.Serializable]
public class ConfirmSetting
{
    public string url;
}

public class ConfirmRequest
{
    private string url;
}

[System.Serializable]
public class ConfirmResponse
{
    public DateTime timeStamp;
    public string status;
    public string data;
}

public class TryConfirmConnection : ConnectionStratage
{
    private string url;

    public TryConfirmConnection(ConfirmSetting setting)
    {
        this.url = setting.url;

        CreateJson();
    }

    private void CreateJson()
    {
        ConfirmRequest request = new ConfirmRequest();

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }

    private void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();

        Debug.Log(this.url);
        request.Setting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        KJY_ConnectionTMP.instance.requestHttp = request;
        KJY_ConnectionTMP.instance.requestHeaderHttp = RequestHeader.other;
        HttpManager.instance.SendRequest(request, RequestHeader.other);
    }

    private void Complete(DownloadHandler result)
    {
        ConfirmResponse response = new ConfirmResponse();
        response = JsonUtility.FromJson<ConfirmResponse>(result.text);

        if (response.status == "OK")
        {
            //UI넘어가는거 호출해주기
            KJY_LoginManager.instance.OnClickConfirmButton();
        }
        else
        {
            ToastMessage.ShowToast("통신에 실패했어요");
            KJY_ConnectionTMP.instance.OnConnectionFailUI();

        }
    }
}
#endregion

#region DeleteAccountConnection

[System.Serializable]
public class DeleteSetting
{
    public string url;
}

public class DeleteRequest
{
    private string url;
}

[System.Serializable]
public class DeleteResponse
{
    public DateTime timeStamp;
    public string status;
    public string data;
}

public class TryDeleteConnection : ConnectionStratage
{
    private string url;

    public TryDeleteConnection(DeleteSetting setting)
    {
        this.url = setting.url;

        CreateJson();
    }

    private void CreateJson()
    {
        DeleteRequest request = new DeleteRequest();

        string jsonData = JsonUtility.ToJson(request);
        OnGetRequest(jsonData);
    }

    private void OnGetRequest(string jsonData)
    {
        HttpRequester request = new HttpRequester();
        

        Debug.Log(this.url);
        request.Setting(RequestType.POST, this.url);
        request.body = jsonData;
        request.complete = Complete;

        KJY_ConnectionTMP.instance.requestHttp = request;
        KJY_ConnectionTMP.instance.requestHeaderHttp = RequestHeader.other;
        HttpManager.instance.SendRequest(request, RequestHeader.other);
    }

    private void Complete(DownloadHandler result)
    {
        DeleteResponse response = new DeleteResponse();
        response = JsonUtility.FromJson<DeleteResponse>(result.text);

        if (response.status == "OK")
        {
            //첫씬으로돌아가고 토큰 파일도 없애버리고 DataManager도 완전 다시하게해야함
            SettingManager.instance.DeletePopUp();
        }
        else
        {
            ToastMessage.ShowToast("계정 삭제에 실패했어요 다시 시도해 주세요");
            KJY_ConnectionTMP.instance.OnConnectionFailUI();

        }
    }
}
#endregion

public class KJY_ConnectionTMP : MonoBehaviour
{
    public static KJY_ConnectionTMP instance;

    [SerializeField] private GameObject text;
    public HttpRequester requestHttp;
    public RequestHeader requestHeaderHttp;
    public int questNoPicture;
    public GameObject connectionFailUI;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (connectionFailUI == null)
        {
            connectionFailUI = GameObject.Find("ConnectionFail Canvas").transform.GetChild(0).gameObject;
        }
    }

    public void OnConnectionFailUI()
    {
        connectionFailUI.SetActive(true);
    }

    public void QuestNo(int questNo)
    {
        this.questNoPicture = questNo;
    }

    public void OnClickTest(Texture2D texture)//카메라 통신하는 정보
    {
        if (texture == null)
        {
            StartCoroutine(successText());
            return;
        }

        ImageSetting setting = new ImageSetting
        {
            questNo = questNoPicture,
            data = texture.EncodeToJPG(),
            url = "https://letsgotour.store/api/v1/quest/image/" + questNoPicture.ToString()
          
        };

        Debug.Log(setting.url);
        TryImageConnection tryImageConnection = gameObject.AddComponent<TryImageConnection>();
        tryImageConnection.Initialize(setting);
    }

    public IEnumerator successText()
    {
        yield return new WaitForSeconds(1);
    }

    public void OnClickHomeConnection() //홈정보 통신하는 함수
    {
        HomeSetting setting = new HomeSetting();
        //LocationInfo info = DataManager.instance.GetGPSInfo();
        //setting.latitude = info.latitude;
        //setting.longitude = info.longitude;
        setting.latitude = 37.282649;
        setting.longitude = 127.016415;
        setting.url = "https://letsgotour.store/api/v1/home?" + "lon=" + setting.longitude + "&lat=" + setting.latitude;

        TryHomeConnection homeConnection = new TryHomeConnection(setting);
    }

    public void OnConnectionQuest(int questNo) //퀘스트 팝업 통신하는 함수
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        QuestSetting setting = new QuestSetting();
        LocationInfo info = GPS.Instance.LocationInfo;
        setting.url = "https://letsgotour.store/api/v1/quest?questNo=" + questNo + "&lon=" + info.longitude + "&lat=" + info.latitude;
        TryQuestConnection questConnection = new TryQuestConnection(setting);
#elif UNITY_EDITOR
        QuestSetting setting = new QuestSetting();
        float latitude = (float)37.566826;
        float longitude = (float) 126.9786567;
        setting.url = "https://letsgotour.store/api/v1/quest?questNo=" + questNo + "&lon=" + longitude + "&lat=" + latitude;
        TryQuestConnection questConnection = new TryQuestConnection(setting);
#endif
    }

    public void OnConnectionConfirm()
    {
        ConfirmSetting setting = new ConfirmSetting();
        setting.url = "https://letsgotour.store/auth/terms";

        TryConfirmConnection confirmConnection = new TryConfirmConnection(setting);
    }

    public void OnConnectionDelete()
    {
        DeleteSetting setting = new DeleteSetting();
        setting.url = "https://letsgotour.store/auth/unlink";

        TryDeleteConnection confirmConnection = new TryDeleteConnection(setting);
    }

    //public void OnConnectionLogin()
    //{
    //    LoginSetting setting = new LoginSetting();
    //    setting.url = "https://letsgotour.store/oauth2/kakao?code=faFanf-f09RBQJs_TzU5Men7TExAKXiT3LL7MQ95k2idB7yTBa5Y9gAAAAQKPXNNAAABkam9CazgLMgnBn6ZSw";


    //    TryLoginConnection tryLoginConnection = new TryLoginConnection(setting);
    //}

    public void Test()
    {
        Application.OpenURL("https://kauth.kakao.com/oauth/authorize?response_type=code&client_id=4d8289f86a3c20f5fdbb250e628d2c75&redirect_uri=https://letsgotour.store/oauth2/kakao");
    }

}
