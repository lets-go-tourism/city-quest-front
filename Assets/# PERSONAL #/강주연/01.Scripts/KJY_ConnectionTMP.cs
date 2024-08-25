using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class ImageReqeuest
{
    public int questNo;
    public byte[] data;
}

[System.Serializable]
public class ImageSetting
{
    public int questNo;
    public string url; // Make sure this is used correctly
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
        //this.questNo = setting.questNo;
        this.url = setting.url; // Use the URL from the setting
        this.data = setting.data;

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

public class KJY_ConnectionTMP : MonoBehaviour
{
    public static KJY_ConnectionTMP instance;

    [SerializeField] private GameObject text;

    private void Awake()
    {
        instance = this;
    }

    public void OnClickTest(Texture2D texture, int questNo)
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

    private Texture2D ConvertToUncompressed(Texture2D original)
    {
        // 압축되지 않은 형식의 새 텍스처 생성
        Texture2D newTexture = new Texture2D(original.width, original.height, TextureFormat.RGBA32, false);

        // 원본 텍스처의 픽셀 데이터를 읽어와 새 텍스처에 복사
        newTexture.SetPixels(original.GetPixels());
        newTexture.Apply();

        return newTexture;
    }

    public IEnumerator successText()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(1);
        text.SetActive(false);
    }
}
