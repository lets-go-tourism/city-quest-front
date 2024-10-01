using System;
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
    public LoginResponse loginData;

    // 박현섭
    //public bool RequestSuccess { get; private set; } = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
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
                    if (DataManager.instance.GetLoginData() != null)
                    {
                        loginData = DataManager.instance.GetLoginData();
                    }
                    request.SetRequestHeader("Content-Type", "application/json");
                    request.SetRequestHeader("Authorization", loginData.data.accessToken);
                }
                break;

            case RequestType.POST:
                request = UnityWebRequest.PostWwwForm(requester.url, requester.body);

                // body데이터를 바이트로 변환
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
                    if (DataManager.instance.GetLoginData() != null)
                    {
                        loginData = DataManager.instance.GetLoginData();
                    }
                    request.SetRequestHeader("Content-Type", "application/json");
                    request.SetRequestHeader("Authorization", loginData.data.accessToken);
                }
                break;
        }

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            requester.Complete(request.downloadHandler);

            if(successDelegate != null)
                successDelegate.Invoke();

            successDelegate = null;
        }
        else
        {
            if (headerState == RequestHeader.other && KJY_ConnectionTMP.instance.isQuest == true)
            {
                SendRequest(KJY_ConnectionTMP.instance.requestHttp, KJY_ConnectionTMP.instance.requestHeaderHttp);
            }
            else
            {
                requester.Complete(request.downloadHandler);
                //StartCoroutine(KJY_ConnectionTMP.instance.successText());

                if (errorDelegate != null)
                    errorDelegate.Invoke();

                errorDelegate = null;
            }
        }
    }


    public delegate void RequestSuccessDelegate();
    public delegate void RequestErrorDelegate();

    public RequestSuccessDelegate successDelegate;
    public RequestErrorDelegate errorDelegate;

}
