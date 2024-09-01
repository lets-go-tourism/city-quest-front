using Gpm.WebView;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.Application;

public class LoginResponse
{
    public DateTime timeStamp;
    public string status;
    public LoginData data;
}

public class LoginData
{
    public string accessToken;
    public string refreshToken;
    public string tokenType;
}

public class loginTest : MonoBehaviour
{
    public List<string> extractedValues;
    public LoginResponse loginData;

    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    public void ShowUrl()
    {
        GpmWebView.ShowUrl(
            "https://kauth.kakao.com/oauth/authorize?response_type=code&client_id=4d8289f86a3c20f5fdbb250e628d2c75&redirect_uri=https://letsgotour.store/oauth2/kakao",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.FULLSCREEN,
                isClearCookie = false,
                isClearCache = false,
                isNavigationBarVisible = true,
                title = "Login",
                isBackButtonVisible = true,
                isForwardButtonVisible = true,
                isCloseButtonVisible = true,
                contentMode = GpmWebViewContentMode.MOBILE
            },
             OnCallback,
         new List<string>(),  // 빈 스킴 리스트 전달
         new List<string>()); ;
    }

    private void OnCallback(GpmWebViewCallback.CallbackType callbackType, string data, GpmWebViewError error)
    {
        if (error != null)
        {
            Debug.LogError("WebView Error: " + error);
            return;
        }

        switch (callbackType)
        {
            case GpmWebViewCallback.CallbackType.PageStarted:
                break;

            case GpmWebViewCallback.CallbackType.PageLoad:
                {
                    Debug.Log("WebView Load Finished");
                   
                    // JavaScript를 실행하여 JSON 데이터를 가져옵니다.
                    string script = @"
                (function() {
                    function getTextContent(element) {
                        return element ? element.textContent.trim() : '';
                    }
            
                    function getAllTextContent() {
                        let elements = document.querySelectorAll('*');
                        let texts = [];
                        elements.forEach(function(element) {
                            let textContent = getTextContent(element);
                            if (textContent) {
                                texts.push(textContent);
                            }
                        });
                        return texts;
                    }

                    return JSON.stringify(getAllTextContent());
                })();";
                    GpmWebView.ExecuteJavaScript(script);
                }
                break;
            case GpmWebViewCallback.CallbackType.ExecuteJavascript:
                {
                    string cleanedString = data.Replace("\\\\\\", "");
                    extractedValues = ExtractStrings(cleanedString);

                    loginData = new LoginResponse();
                    loginData.data = new LoginData();

                    loginData.timeStamp = DateTime.Now;
                    loginData.status = extractedValues[4];
                    loginData.data.accessToken = extractedValues[7];
                    loginData.data.refreshToken = extractedValues[9];
                    loginData.data.tokenType = extractedValues[11];

                    DataManager.instance.SetLoginData(loginData);
                }
                break;
            default:
                Debug.Log("Callback type: " + callbackType);
                break;
        }
    }

    public static List<string> ExtractStrings(string input)
    {
        List<string> extractedStrings = new List<string>();
        bool insideQuotes = false;
        string currentString = "";

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '"')
            {
                if (insideQuotes)
                {
                    extractedStrings.Add(currentString);
                    currentString = ""; // Reset the current string
                }

                insideQuotes = !insideQuotes;
            }
            else if (insideQuotes)
            {
                currentString += input[i];
            }
        }

        return extractedStrings;
    }


    public void TestLoginData()
    {
        //DataManager.instance.testConnectin = extractedValues;

        //KJY_ConnectionTMP.instance.test = extractedValues;

        //loginData = new LoginResponse();
        //loginData.data = new LoginData();

        //loginData.timeStamp = DateTime.Now;
        //loginData.status = extractedValues[4];
        //loginData.data.accessToken = extractedValues[7];
        //loginData.data.refreshToken = extractedValues[9];
        //loginData.data.tokenType = extractedValues[11];

        LoginResponse test = DataManager.instance.GetLoginData();

        text1.text = test.data.accessToken;
        text2.text = test.status;

        Canvas.ForceUpdateCanvases();

    }
}
