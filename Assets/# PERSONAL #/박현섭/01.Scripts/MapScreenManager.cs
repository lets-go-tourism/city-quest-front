using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
# if UNITY_EDITOR
        HttpManager.instance.loginData = new LoginResponse();
        HttpManager.instance.loginData.data = new LoginData();
        HttpManager.instance.loginData.data.accessToken = "eyJhbGciOiJIUzM4NCJ9.eyJzdWIiOiI0IiwiaWF0IjoxNzI1MzUxMjc1LCJleHAiOjE4MTE3NTEyNzV9.Z79jBdCygqYa7d0eJzgOt1ZGBVVF90tLB0axxRmyRDWZbg89B7rRzrVtknQ_8Qgn";

        DataManager.instance.SetLoginData(HttpManager.instance.loginData);

#endif
        KJY_ConnectionTMP.instance.OnClickHomeConnection();
    }
}
