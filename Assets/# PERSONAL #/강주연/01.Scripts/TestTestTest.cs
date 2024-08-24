using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestTestTest : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return SendData();
    }

    private IEnumerator SendData()
    {
        string path = @"C:\Users\juyeon0514\Pictures\test.png";
        string url = "http://43.203.101.31:8080/api/v1/quest/image";
        byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        string fileName = System.IO.Path.GetFileName(path);

        WWWForm form = new WWWForm();
        form.AddBinaryData("image", byteTexture, "test.jpg", "image/jpg");
        form.AddField("questNo", 0);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {

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
