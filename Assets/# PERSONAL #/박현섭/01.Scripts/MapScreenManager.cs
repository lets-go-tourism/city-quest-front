using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HttpManager.instance.loginData = new LoginResponse();
        HttpManager.instance.loginData.data = new LoginData();
        HttpManager.instance.loginData.data.accessToken = "eyJhbGciOiJIUzM4NCJ9.eyJzdWIiOiI2IiwiaWF0IjoxNzI1Mjc5MjM1LCJleHAiOjE4MTE2NzkyMzV9.taaVL_qnRmhE5nBQQvieeOy0KAO-vguGbsocbJq8OglzZzgjYlUiC9oLG3BT0SKM";
        KJY_ConnectionTMP.instance.OnClickHomeConnection();
    }
}
