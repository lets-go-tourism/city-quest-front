using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_ScreenResolution : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Awake()
    {
        Rect viewport = cam.rect;

        float screenAspectRatio = (float)(Screen.width / Screen.height);
        float targetAspectRation = 9f / 19.5f;

        if (screenAspectRatio < targetAspectRation)
        {
            viewport.height = screenAspectRatio / targetAspectRation;
            viewport.y = (1f - viewport.height) / 2f;
        }
        else
        {
            viewport.width = targetAspectRation / screenAspectRatio;
            viewport.x = (1f - viewport.width) / 2f;
        }

        cam.rect = viewport;
    }
}
