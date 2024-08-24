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

                // body�����͸� ����Ʈ�� ��ȯ
                byte[] jsonToSend = new UTF8Encoding().GetBytes(requester.body);

                request.uploadHandler.Dispose();
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

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
            print("��û ����");
            print(request.downloadHandler.text);
            print(request.error);
            StartCoroutine(KJY_ConnectionTMP.instance.successText());
        }
    }
}
