using Gpm.WebView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.Application;
using static UnityEngine.Rendering.DebugUI.MessageBox;

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
    public bool agreed;
}

public class loginTest : MonoBehaviour
{
    public List<string> extractedValues = new List<string>();
    public LoginResponse loginData;
    private bool test = false;
    private int count;
    [SerializeField]private Canvas canvas;
    private int width = Screen.width;
    private int height = Screen.height;

    public void ShowUrl()
    {
        GpmWebView.ShowUrl(
            "https://kauth.kakao.com/oauth/authorize?response_type=code&client_id=4d8289f86a3c20f5fdbb250e628d2c75&redirect_uri=https://letsgotour.store/oauth2/kakao",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                title = "Login",
                isBackButtonVisible = true,
                isForwardButtonVisible = true,
                isCloseButtonVisible = true,
                contentMode = GpmWebViewContentMode.MOBILE
            },
             OnCallback,
         new List<string>(),  // ºó ½ºÅ´ ¸®½ºÆ® Àü´Þ
         new List<string>()); 
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

            case GpmWebViewCallback.CallbackType.Open:
                {
                    GpmWebView.SetPosition(0, 0);
                    GpmWebView.SetSize(width, height);
                    count = 0;
                }
                break;

            case GpmWebViewCallback.CallbackType.PageLoad:
                {
                   // count++;

                   // if (count >= 3)
                    //{
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
                    canvas.enabled = true;
                    GpmWebView.ExecuteJavaScript(script);
                   // }
                }
                break;
            case GpmWebViewCallback.CallbackType.ExecuteJavascript:
                {
                    if (data == null)
                        return;

                    string cleanedString = data.Replace("\\\\\\", "");
                    extractedValues = ExtractStringsAndBooleans(cleanedString);

                    if (extractedValues.Count == 43)
                    {
                        canvas.enabled = true;
                        if (extractedValues[4] == "OK")
                        {
                            loginData = new LoginResponse();
                            loginData.data = new LoginData();

                            loginData.timeStamp = DateTime.Now;
                            loginData.status = extractedValues[4];
                            loginData.data.accessToken = extractedValues[7];
                            loginData.data.refreshToken = extractedValues[9];
                            loginData.data.tokenType = extractedValues[11];
                            loginData.data.agreed = bool.Parse(extractedValues[13]);

                            DataManager.instance.SetLoginData(loginData);
                            if (extractedValues[13] == "false")
                            {
                                KJY_UIManager.instance.ShowConfirmScrollView();
                            }
                            else
                            {
                                Debug.Log("inhere");
                                KJY_UIManager.instance.ShownLoginSccuess();
                            }
                            GpmWebView.Close();
                            canvas.enabled = false;
                        }
                    }
                    else
                    {
                        canvas.enabled = false;
                    }
                }
                break;
            default:
                Debug.Log("Callback type: " + callbackType);
                break;
        }
    }

    public static List<string> ExtractStringsAndBooleans(string input)
    {
        List<string> extractedValues = new List<string>();
        bool insideQuotes = false;
        string currentString = "";

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '"')
            {
                if (insideQuotes)
                {
                    extractedValues.Add(currentString);
                    currentString = ""; 
                }
                insideQuotes = !insideQuotes;
            }
            else if (insideQuotes)
            {
                currentString += input[i];
            }
            else if (!insideQuotes)
            {
                if (input.Substring(i).StartsWith("true"))
                {
                    extractedValues.Add("true");
                    i += 3;
                }
                else if (input.Substring(i).StartsWith("false"))
                {
                    extractedValues.Add("false");
                    i += 4;
                }
            }
        }

        return extractedValues;
    }

    public void TestLoginData()
    {
        loginData = new LoginResponse();
        loginData.data = new LoginData();

        loginData.timeStamp = DateTime.Now;
        loginData.status = extractedValues[4];
        loginData.data.accessToken = extractedValues[7];
        loginData.data.refreshToken = extractedValues[9];
        loginData.data.tokenType = extractedValues[11];
        loginData.data.agreed = bool.Parse(extractedValues[13]);

        DataManager.instance.SetLoginData(loginData);
    }

    public void CanvasOnOff(bool value)
    {
        canvas.enabled = value;
    }
}
