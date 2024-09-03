using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public enum RequestType
{
    GET,
    POST,
    PUT,
    DELETE
}

public enum RequestHeader
{
    login,
    image,
    other
}

public class HttpManager : MonoBehaviour
{
    public static HttpManager instance;
    private RequestHeader headerState = RequestHeader.login;
    public LoginResponse loginData = new LoginResponse();
    public string test;

    // ������
    public bool RequestSuccess { get; private set; } = false;

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

    // Request
    public void SendRequest(HttpRequester requester, RequestHeader state)
    {
        headerState = state;
        StartCoroutine(SendProcess(requester));
    }

    IEnumerator SendProcess(HttpRequester requester)
    {

        UnityWebRequest request = null;

        switch (requester.requestType)
        {
            case RequestType.GET:

                request = UnityWebRequest.Get(requester.url);

                if (headerState == RequestHeader.image)
                {
                    request.SetRequestHeader("Content-Type", "multipart-form-data");
                }

                if (headerState == RequestHeader.login)
                {
                  request.SetRequestHeader("Content-Type", "application/json");
                }

                if (headerState == RequestHeader.other)
                {
                    request.SetRequestHeader("Content-Type", "application/json");
                    request.SetRequestHeader("Authorization", loginData.data.accessToken);
                }
                break;

            case RequestType.POST:
                request = UnityWebRequest.PostWwwForm(requester.url, requester.body);

                // body�����͸� ����Ʈ�� ��ȯ
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

                if (headerState == RequestHeader.image)
                {
                    request.SetRequestHeader("Content-Type", "multipart-form-data");
                }

                if (headerState == RequestHeader.login)
                {
                  request.SetRequestHeader("Content-Type", "application/json");
                }

                if (headerState == RequestHeader.other)
                {
                    request.SetRequestHeader("Content-Type", "application/json");
                    request.SetRequestHeader("Authorization", loginData.data.accessToken);
                    Debug.Log(loginData.data.accessToken);
                    //request.SetRequestHeader("Authorization", test);
                }
                break;
        }

        print("��ٸ��� ��");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            print("��û �Ϸ�");
            print(request.downloadHandler.text);
            requester.Complete(request.downloadHandler);
        }
        else
        {
            //SendRequest(KJY_ConnectionTMP.instance.requestHttp, KJY_ConnectionTMP.instance.requestHeaderHttp);
            print("��û ����");
            print(request.downloadHandler.text);
            print(request.error);
            //StartCoroutine(KJY_ConnectionTMP.instance.successText());
        }
    }
}
