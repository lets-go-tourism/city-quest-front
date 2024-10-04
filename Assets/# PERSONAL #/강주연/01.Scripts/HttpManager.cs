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
    private UnityWebRequest currentRequest;

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
        if (currentRequest != null)
        {
            AbortRequest();  
        }

        headerState = state;
        StartCoroutine(SendProcess(requester));
    }

    IEnumerator SendProcess(HttpRequester requester)
    {
        currentRequest = null;
        
        switch (requester.requestType)
        {
            case RequestType.GET:

                currentRequest = UnityWebRequest.Get(requester.url);

                if (headerState == RequestHeader.image)
                {
                    currentRequest.SetRequestHeader("Content-Type", "multipart-form-data");
                }

                if (headerState == RequestHeader.login)
                {
                  currentRequest.SetRequestHeader("Content-Type", "application/json");
                }

                if (headerState == RequestHeader.other)
                {
                    if (DataManager.instance.GetLoginData() != null)
                    {
                        loginData = DataManager.instance.GetLoginData();
                    }
                    currentRequest.SetRequestHeader("Content-Type", "application/json");
                    currentRequest.SetRequestHeader("Authorization", loginData.data.accessToken);
                }
                break;

            case RequestType.POST:
                currentRequest = UnityWebRequest.PostWwwForm(requester.url, requester.body);

                // body데이터를 바이트로 변환
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                currentRequest.uploadHandler.Dispose();
                currentRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);

                if (headerState == RequestHeader.image)
                {
                    currentRequest.SetRequestHeader("Content-Type", "multipart-form-data");
                }

                if (headerState == RequestHeader.login)
                {
                    currentRequest.SetRequestHeader("Content-Type", "application/json");
                }

                if (headerState == RequestHeader.other)
                {
                    if (DataManager.instance.GetLoginData() != null)
                    {
                        loginData = DataManager.instance.GetLoginData();
                    }
                    currentRequest.SetRequestHeader("Content-Type", "application/json");
                    currentRequest.SetRequestHeader("Authorization", loginData.data.accessToken);
                }
                break;
        }


        yield return currentRequest.SendWebRequest();

        if (currentRequest.result == UnityWebRequest.Result.Success)
        {
            requester.Complete(currentRequest.downloadHandler);

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
                requester.Complete(currentRequest.downloadHandler);
                //StartCoroutine(KJY_ConnectionTMP.instance.successText());

                if (errorDelegate != null)
                    errorDelegate.Invoke();

                errorDelegate = null;
            }
        }
    }

    public void AbortRequest()
    {
        if (currentRequest != null)
        {
            currentRequest.Abort();
            currentRequest.Dispose();
            currentRequest = null;
            DataManager.instance.SetQuestInfo(null);
        }
    }


    public delegate void RequestSuccessDelegate();
    public delegate void RequestErrorDelegate();

    public RequestSuccessDelegate successDelegate;
    public RequestErrorDelegate errorDelegate;

}
