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
        HttpManager.instance.loginData.data.accessToken = "eyJhbGciOiJIUzM4NCJ9.eyJzdWIiOiI0IiwiaWF0IjoxNzI1MzUxMjc1LCJleHAiOjE4MTE3NTEyNzV9.Z79jBdCygqYa7d0eJzgOt1ZGBVVF90tLB0axxRmyRDWZbg89B7rRzrVtknQ_8Qgn";

        DataManager.instance.SetLoginData(HttpManager.instance.loginData);
#endif

        DataManager.instance.requestSuccess = false;
        KJY_ConnectionTMP.instance.OnClickHomeConnection();
    }

    private IEnumerator Start()
    {
        MainView_UI.instance.BackgroundDarkEnable();
        while(DataManager.instance.requestSuccess == false)
        {
            yield return null;
        }
        MainView_UI.instance.BackgroundDarkDisable();
    }
}
