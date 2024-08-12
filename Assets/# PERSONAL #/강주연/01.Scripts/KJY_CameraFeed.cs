using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
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

    private void Start()
    {
        SetWebCam();
    }

    private void SetWebCam()
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

    private void CreateWebCamTexture(string permissionName = null)
    {
        if (webCamTexture)
        {
            Destroy(webCamTexture);
            webCamTexture = null;
        }

        WebCamDevice[] webCamDevices = WebCamTexture.devices;
        if (webCamDevices.Length == 0) return;

        int backCamIndex = -1;
        for (int i = 0, l = webCamDevices.Length; i < l; ++i)
        {
            if (!webCamDevices[i].isFrontFacing)
            {
                backCamIndex = i;
                break;
            }
        }

        if (backCamIndex != -1)
        {
            int requestWidth = Screen.width;
            int reqeustHeight = Screen.height;
            for (int i = 0, l = webCamDevices[backCamIndex].availableResolutions.Length; i < l; ++i)
            {
                Resolution resolution = webCamDevices[backCamIndex].availableResolutions[i];
                if (GetAspectRatio((int)requestedRatio.x, (int)requestedRatio.y).Equals(GetAspectRatio(resolution.width, resolution.height)))
                {
                    requestWidth = resolution.width;
                    reqeustHeight = resolution.height;

                    break;
                }
            }
            webCamTexture = new WebCamTexture(webCamDevices[backCamIndex].name, requestWidth, reqeustHeight, requestedFPS);
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

        int viedeoRotAngle = webCamTexture.videoRotationAngle;
        webCamRawImage.transform.localEulerAngles = new Vector3(0, 0, -viedeoRotAngle);

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

        if (Mathf.Abs(viedeoRotAngle) % 180 != 0f)
        {
            Swap(ref width, ref height);
        }
        webCamRawImage.rectTransform.sizeDelta = new Vector2(width, height);    
    }
}
