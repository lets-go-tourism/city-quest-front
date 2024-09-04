using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScreenManager : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_EDITOR
        HttpManager.instance.loginData = new LoginResponse();
        HttpManager.instance.loginData.data = new LoginData();
        HttpManager.instance.loginData.data.accessToken = "eyJhbGciOiJIUzM4NCJ9.eyJzdWIiOiIzOCIsImlhdCI6MTcyNTM4MzMxOSwiZXhwIjoxODExNzgzMzE5fQ.dHrpyncjuC75zWZjzpItdq4KaA3r3WYOwnz82C3vT1P_JHvXY7BiY_-0jQ2L6m3p\",\"refreshToken\":\"eyJhbGciOiJIUzM4NCJ9.eyJpYXQiOjE3MjUzODMzMTksImV4cCI6MTgxMTc4MzMxOX0.8bTp2sb_yRLu8GixBdTgTTJfypry1QobBktlWNDfaPVw8Dp-2ogdyQynV3QCPXV3";

        DataManager.instance.SetLoginData(HttpManager.instance.loginData);
#endif

        DataManager.instance.requestSuccess = false;
    }

    private IEnumerator Start()
    {
        KJY_ConnectionTMP.instance.OnClickHomeConnection();
        MainView_UI.instance.BackgroundDarkEnable();
        while(DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }
        MainView_UI.instance.BackgroundDarkDisable();
    }
}
