using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CameraFeed : MonoBehaviour
{
    [Header("Setting")]
    public Vector2 requestedRatio;
    public int requestedFPS;

    [Header("Component")]
    public RawImage webCamRawImage;

    [Header("Data")]
    public WebCamTexture webCamTexture;
    private bool isVertical = true;

    private bool useFrontCamera = true; 

    private void Start()
    {
        SetWebCam();
    }

    public void SetWebCam()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            CreateWebCamTexture();
        }
        else
        {
            PermissionCallbacks permissionCallbacks = new();
            permissionCallbacks.PermissionGranted += CreateWebCamTexture;
            Permission.RequestUserPermission(Permission.Camera, permissionCallbacks);
        }
    }

    public void SwitchCamera()
    {
        useFrontCamera = !useFrontCamera;
        SetWebCam();
    }

    private void CreateWebCamTexture(string permissionName = null)
    {
        if (webCamTexture)
        {
            Destroy(webCamTexture);
            webCamTexture = null;
        }

        WebCamDevice[] webCamDevices = WebCamTexture.devices;
        if (webCamDevices.Length == 0) return;

        int cameraIndex = -1;
        for (int i = 0, l = webCamDevices.Length; i < l; ++i)
        {
            if (webCamDevices[i].isFrontFacing == useFrontCamera)  
            {
                cameraIndex = i;
                break;
            }
        }

        if (cameraIndex != -1)
        {
            int requestWidth = Screen.width;
            int requestHeight = Screen.height;
            for (int i = 0, l = webCamDevices[cameraIndex].availableResolutions.Length; i < l; ++i)
            {
                Resolution resolution = webCamDevices[cameraIndex].availableResolutions[i];
                if (GetAspectRatio((int)requestedRatio.x, (int)requestedRatio.y).Equals(GetAspectRatio(resolution.width, resolution.height)))
                {
                    requestWidth = resolution.width;
                    requestHeight = resolution.height;

                    break;
                }
            }
            webCamTexture = new WebCamTexture(webCamDevices[cameraIndex].name, requestWidth, requestHeight, requestedFPS);
            webCamTexture.filterMode = FilterMode.Trilinear;
            webCamTexture.Play();

            webCamRawImage.texture = webCamTexture;
        }
    }

    private string GetAspectRatio(int width, int height, bool allowPrtrait = false)
    {
        if (!allowPrtrait && width < height)
        {
            Swap(ref width, ref height);
        }
        float r = (float)width / height;
        return r.ToString("F2");
    }

    private void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    private void Update()
    {
        UpdateWebCamRawImage();
    }

    private void UpdateWebCamRawImage()
    {
        if (!webCamTexture) return;

        int videoRotAngle = webCamTexture.videoRotationAngle;
        webCamRawImage.transform.localEulerAngles = new Vector3(0, 0, -videoRotAngle);

        int width, height;
        if (Screen.orientation == ScreenOrientation.Portrait ||
            Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            width = Screen.width;
            height = Screen.width * webCamTexture.width / webCamTexture.height;
        }
        else
        {
            height = Screen.height;
            width = Screen.width * webCamTexture.width / webCamTexture.height;
        }

        if (Mathf.Abs(videoRotAngle) % 180 != 0f)
        {
            Swap(ref width, ref height);
        }
        webCamRawImage.rectTransform.sizeDelta = new Vector2(width, height);
    }

    public void CapturePhoto()
    {
        StartCoroutine(CapturePhotoCoroutine());
    }

    private IEnumerator CapturePhotoCoroutine()
    {
        yield return new WaitForEndOfFrame(); 

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        Texture2D rotatedPhoto = RotateTexture(photo, false);
        webCamRawImage.texture = rotatedPhoto;

        webCamTexture.Stop();
    }

    private Texture2D RotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Texture2D rotatedTexture = new Texture2D(originalTexture.height, originalTexture.width);
        Color32[] originalPixels = originalTexture.GetPixels32();
        Color32[] rotatedPixels = new Color32[originalPixels.Length];

        int w = originalTexture.width;
        int h = originalTexture.height;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (clockwise)
                {
                    rotatedPixels[j + i * h] = originalPixels[(h - j - 1) * w + i];
                }
                else
                {
                    rotatedPixels[j + i * h] = originalPixels[j * w + (w - i - 1)];
                }
            }
        }

        rotatedTexture.SetPixels32(rotatedPixels);
        rotatedTexture.Apply();
        return rotatedTexture;
    }



    public void RotatePhoto()
    {
        Texture2D currentPhoto = webCamRawImage.texture as Texture2D;

        if (currentPhoto != null)
        {
            Texture2D rotatedPhoto = RotateTexture(currentPhoto, true); // 시계 방향 90도 회전
            webCamRawImage.texture = rotatedPhoto;
        }
    }

    //public Texture2D ConvertRenderTextureToTexture2D(Texture texture)
    //{
    //    if (texture is Texture2D texture2D)
    //    {
    //        return texture2D;
    //    }

    //    // Texture가 Texture2D가 아닌 경우
    //    RenderTexture renderTexture = texture as RenderTexture;
    //    if (renderTexture != null)
    //    {
    //        // RenderTexture를 Texture2D로 변환
    //        Texture2D texture2DFromRenderTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
    //        RenderTexture.active = renderTexture;
    //        texture2DFromRenderTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
    //        texture2DFromRenderTexture.Apply();
    //        RenderTexture.active = null;
    //        return texture2DFromRenderTexture;
    //    }

    //    // Texture가 RenderTexture도 아니면 예외 처리
    //    Debug.LogError("Texture is neither Texture2D nor RenderTexture");
    //    return null;
    //}

    private Texture2D ConvertTextureToTexture2D(Texture texture)
    {
        if (texture is Texture2D texture2D)
        {
            return texture2D;
        }
        else if (texture is RenderTexture renderTexture)
        {
            return ConvertRenderTextureToTexture2D(renderTexture);
        }
        else
        {
            Debug.LogError("Unsupported texture format. The texture must be either Texture2D or RenderTexture.");
            return null;
        }
    }

    private Texture2D ConvertRenderTextureToTexture2D(RenderTexture renderTexture)
    {
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;

        return texture2D;
    }

    public void UploadImage()
    {
        //// RawImage의 texture가 Texture2D인지 확인
        //Texture2D texture2D = webCamRawImage.texture as Texture2D;

        //if (texture2D == null)
        //{
        //    // 만약 Texture가 RenderTexture일 경우
        //    RenderTexture renderTexture = webCamRawImage.texture as RenderTexture;
        //    if (renderTexture != null)
        //    {
        //        texture2D = ConvertRenderTextureToTexture2D(renderTexture);
        //    }
        //    else
        //    {
        //        Debug.LogError("The texture is neither a Texture2D nor a RenderTexture.");
        //    }
        //}
        int width = webCamRawImage.texture.width;
        int height = webCamRawImage.texture.height;

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture copiedRenderTexture = new RenderTexture(width, height, 0);

        Graphics.Blit(webCamRawImage.texture, copiedRenderTexture);

        RenderTexture.active = copiedRenderTexture;


        Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);

        texture2D.ReadPixels(new UnityEngine.Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRenderTexture;

        KJY_ConnectionTMP.instance.OnClickTest(texture2D, 1);
        //KJY_ConnectionTMP.instance.OnClickTest(webCamRawImage.texture, 0);
    }

    public void Test()
    {
        //KJY_ConnectionTMP.instance.Test(webCamRawImage.texture, 0);
    }
}
