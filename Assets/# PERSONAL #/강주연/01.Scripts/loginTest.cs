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
    public bool agreed;
}

public class loginTest : MonoBehaviour
{
    public List<string> extractedValues = new List<string>();
    public LoginResponse loginData;
    public TextMeshProUGUI text;
    private int count;

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
         new List<string>(),  // ºó ½ºÅ´ ¸®½ºÆ® Àü´Þ
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
            case GpmWebViewCallback.CallbackType.Open:
                {
                    count = 0;
                }
                break;

            case GpmWebViewCallback.CallbackType.PageLoad:
                {
                    Debug.Log("WebView Load Finished");

                    count++;


                   // if (count == 1 || count == 2 || count == 3)
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
                            GpmWebView.ExecuteJavaScript(script);
                    //}
                }
                break;
            case GpmWebViewCallback.CallbackType.ExecuteJavascript:
                {
                    if (data == null)
                        return;

                    string cleanedString = data.Replace("\\\\\\", "");
                    extractedValues = ExtractStringsAndBooleans(cleanedString);

                    print(extractedValues.Count + " °¹¼ö´Â");

                    for (int i = 0; i < extractedValues.Count; i++)
                    {
                        print(i + "¹øÂ° " + extractedValues[i]);
                    }

                    if (extractedValues[4] == "OK" && extractedValues.Count > 13)
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
                            KJY_UIManager.instance.LoginCheck();
                        }
                        GpmWebView.Close();
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
}
