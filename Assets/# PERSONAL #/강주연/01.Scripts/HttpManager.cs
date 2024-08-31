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
    public LoginData loginData = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
    }

    // Request
    public void SendRequest(HttpRequester requester)
    {
        //headerState = state;
        StartCoroutine(SendProcess(requester));
    }

    IEnumerator SendProcess(HttpRequester requester)
    {

        UnityWebRequest request = null;

        switch (requester.requestType)
        {
            case RequestType.GET:

                request = UnityWebRequest.Get(requester.url);

                //if (headerState == RequestHeader.image)
                //{
                //    request.SetRequestHeader("Content-Type", "multipart-form-data");
                //}

                //if (headerState == RequestHeader.login)
                //{
                    request.SetRequestHeader("Content-Type", "application/json");
                //}

                //if (headerState == RequestHeader.other)
                //{
                //    request.SetRequestHeader("Content-Type", "application/json");
                //   request.SetRequestHeader("Authorization", "Bearer " + loginData.accessToken);
                //    request.SetRequestHeader("RefreshToken", "Bearer " + loginData.refreshToken);
                //}
                break;

            case RequestType.POST:
                request = UnityWebRequest.PostWwwForm(requester.url, requester.body);

                // body데이터를 바이트로 변환
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

                //if (headerState == RequestHeader.image)
                //{
                //    request.SetRequestHeader("Content-Type", "multipart-form-data");
                //}

                //if (headerState == RequestHeader.login)
                //{
                    request.SetRequestHeader("Content-Type", "application/json");
                //}

                //if (headerState == RequestHeader.other)
                //{
                //    request.SetRequestHeader("Authorization", "Bearer " + loginData.accessToken);
                //    request.SetRequestHeader("RefreshToken", "Bearer " + loginData.refreshToken);
                //}
                break;
        }

        print("기다리는 중");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            print("요청 완료");
            print(request.downloadHandler.text);
            requester.Complete(request.downloadHandler);
        }
        else
        {
            print("요청 실패");
            print(request.downloadHandler.text);
            print(request.error);
            StartCoroutine(KJY_ConnectionTMP.instance.successText());
        }
    }
}
