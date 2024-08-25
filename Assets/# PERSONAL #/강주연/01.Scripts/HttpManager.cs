using System.Collections;
using System.Collections.Generic;
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

public class HttpManager : MonoBehaviour
{
    public static HttpManager instance;

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
        StartCoroutine(SendProcess(requester));
    }

    IEnumerator SendProcess(HttpRequester requester)
    {

        UnityWebRequest request = null;

        switch (requester.requestType)
        {
            case RequestType.GET:

                request = UnityWebRequest.Get(requester.url);
                request.SetRequestHeader("Content-Type", "multipart-form-data");

                break;

            case RequestType.POST:
                request.SetRequestHeader("Content-Type", "multipart-form-data");
                request = UnityWebRequest.Post(requester.url, requester.form);

                // body데이터를 바이트로 변환
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

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
