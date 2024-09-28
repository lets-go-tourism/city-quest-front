using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using static PopUpMovement;

public enum authState
{
    None,
    GPS,
    Camera,
    Alarm
}

public class KJY_LoginManager : MonoBehaviour
{
    public static KJY_LoginManager instance;

    [Header("splash_onBoard")]
    [SerializeField] private GameObject splash_onBoardObject;
    [SerializeField] private RectTransform logoFirstPosition;
    [SerializeField] private RectTransform logoSecondPosition;
    [SerializeField] private GameObject logo;
    [SerializeField] private Image logoImage;
    [SerializeField] private TextMeshProUGUI explain;

    [Header("OnBoard")]
    [SerializeField] private GameObject kakaoBtn;


    [Header("Term&Confirm")]
    [SerializeField] private GameObject confrimObject;
    [SerializeField] private Scrollbar confirmScrollView;
    [SerializeField] private ScrollRect confirmScrollViewtest;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Image confirmBtnSprite;
    private bool isConfirmView = false;

    [Header("Authorization")]
    [SerializeField] private GameObject authorizationObject;
    [SerializeField] private GameObject authorizationStartBtn;
    [SerializeField] private GameObject Popup;

    [Header("LoginSuccess")]
    [SerializeField] private TextMeshProUGUI loginText;
    [SerializeField] private GameObject CustomerLoginUI;
    [SerializeField] private GameObject loginImageUI;
    [SerializeField] private Image loginImage;
    [SerializeField] private RectTransform spotPosition;
    [SerializeField] private GameObject CustomerLoginBtn;

    [Header("Editor")]
    [SerializeField] private GameObject editorButton;
    [SerializeField] private List<string> loginEditorData;

    public authState state = authState.None;
    private bool isLogin = false;
    private int count = 0;
    //private PermissionCallbacks callbacks;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //explain.DOFade(0f, 0f);
        
        kakaoBtn.SetActive(false);
        logo.transform.position = logoFirstPosition.position;
        confirmBtn.interactable = false;
        StartCoroutine(Splash());
        //logiunUIRect.DOAnchorPosY(-1800, 0.38f);
    }

    private void Update()
    {
        if ((confirmScrollView.value == 0 && isConfirmView == true) || (confirmScrollViewtest.verticalNormalizedPosition <= 0.01f && isConfirmView == true))
        {
            confirmBtn.interactable = true;
        }
    }

    public void StartSplash()
    {
        StartCoroutine(Splash());
    }

    #region splash1-1-2
    private IEnumerator Splash()
    {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(onLogo());
        //explain.DOFade(1f, 1f);
        //explain.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        //여기서 로그인되있는지 안되있는지에 토큰 정보에 따라서 바로 이동시켜줄지 아니면 온보딩으로 이동할지 결정
        if (DataManager.instance.GetLoginData() == null || DataManager.instance.isLogout == true) // 로그인안됐는경우
        {
            isConfirmView = true;
            kakaoBtn.SetActive(true);
        }
        else
        {
            LoginCheck();
        }
    }

    IEnumerator onLogo()
    {
        logo.transform.DOMove(logoSecondPosition.position, 0.8f);
        logoImage.DOFade(1, 0.8f);
        yield return new WaitForSeconds(1f);
    }

    public void LoginCheck()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation) == false || Permission.HasUserAuthorizedPermission(Permission.Camera) == false || Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS") == false)
        {
            splash_onBoardObject.SetActive(false);
            kakaoBtn.SetActive(false);
            authorizationObject.SetActive(true);
            isLogin = true;
        }
        else
        {
            DataManager.instance.JsonSave();
            splash_onBoardObject.SetActive(false); //TMP
            kakaoBtn.SetActive(false); //TMP
            SceneMove();
        }
    }

    //public void ontestButton()
    //{
    //    testBtn.SetActive(true);
    //}

    //public void testButton()
    //{
    //    LoginResponse data = DataManager.instance.GetLoginData();
    //    text1.text = data.data.accessToken;
    //    text2.text = data.data.agreed.ToString();
    //    if (data.data.agreed == true)
    //    {
    //        ShownLoginSccuess();
    //        testBtn.SetActive(false);
    //    }
    //    else
    //    {
    //        ShowConfirmScrollView();
    //        testBtn.SetActive(false);
    //    }
    //}
    #endregion

    #region newCustomer

    public void SplashUIOff()
    {
        splash_onBoardObject.SetActive(false);
        kakaoBtn.SetActive(false);
    }

    //회원가입일시
    public void ShowConfirmScrollView()
    {
        //여기서 로그인했는지 안했는지에 따라 달라짐
        confrimObject.SetActive(true);
        isConfirmView = true;
    }

    public void OnClickConfirmButton()
    {
        confrimObject.SetActive(false);
        authorizationObject.SetActive(true);
        isConfirmView = false;
    }

    public void OnClickAuthorizationButton()
    {
        AuthorizationGPS();
    }

    public void AuthorizationGPS() //GPS 권한 허락받는 함수
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            state = authState.GPS;
            AuthorizationCamera();
        }
        else
        {
            PermissionCallbacks callbacks = new();
            callbacks.PermissionGranted += PermissionGrantedCallback;
            callbacks.PermissionDenied += PermissionDeniedCallback;

            Permission.RequestUserPermission(Permission.FineLocation, callbacks);
        }
    }

    public void AuthorizationCamera() //Camera 권한 허락받는 함수
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            state = authState.Camera;
            AuthorizationAlram();
        }
        else
        {
            PermissionCallbacks callbacks = new();
            callbacks.PermissionGranted += PermissionGrantedCallback;
            callbacks.PermissionDenied += PermissionDeniedCallback;

            Permission.RequestUserPermission(Permission.Camera, callbacks);
        }
    }

    public void AuthorizationAlram()//Alram보내는거 권한 허락받는 함수
    {
#if UNITY_ANDROID
        if (AndroidVersionCheck() == true)
        {
            if (Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                if (isLogin == false)
                {
                    DataManager.instance.JsonSave();
                    authorizationObject.SetActive(false);
                    loginText.text = "회원가입 성공!\n환영해요.";
                    StartCoroutine(nameof(PopupCoroutine));
                    //CustomerLoginUI.SetActive(true);
                }
                else
                {
                    DataManager.instance.JsonSave();
                    authorizationObject.SetActive(false);
                    loginText.text = "로그인 성공!\n환영해요.";
                    StartCoroutine(nameof(PopupCoroutine));
                    //CustomerLoginUI.SetActive(true);
                }
            }
            else
            {
                PermissionCallbacks callbacks = new();
                callbacks.PermissionGranted += PermissionGrantedCallback;
                callbacks.PermissionDenied += PermissionDeniedCallback;

                Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS", callbacks);
            }
        }
        else
        {
            if (isLogin == false)
            {
                DataManager.instance.JsonSave();
                authorizationObject.SetActive(false);
                loginText.text = "회원가입 성공!\n환영해요.";
                StartCoroutine(nameof(PopupCoroutine));
                //CustomerLoginUI.SetActive(true);
            }
            else
            {
                DataManager.instance.JsonSave();
                authorizationObject.SetActive(false);
                loginText.text = "로그인 성공!\n환영해요.";
                StartCoroutine(nameof(PopupCoroutine));
                //CustomerLoginUI.SetActive(true);
            }
        }
#endif
    }

    private bool AndroidVersionCheck()
    {
        return Application.platform == RuntimePlatform.Android &&
               new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT") >= 33;
    }

    private void PermissionGrantedCallback(string permissionName)
    {
        if (state == authState.None)
        {
            count = 0;
            DataManager.instance.JsonSave();
            state = authState.GPS;
            AuthorizationCamera();
        }
        else if (state == authState.GPS)
        {
            count = 0;
            state = authState.Camera;
            AuthorizationAlram();
        }
        else if (state == authState.Camera)
        {
            count = 0;
            if (isLogin == false)
            {
                DataManager.instance.JsonSave();
                authorizationObject.SetActive(false);
                loginText.text = "회원가입 성공!\n환영해요.";
                //CustomerLoginUI.SetActive(true);
                StartCoroutine(nameof(PopupCoroutine));
            }
            else
            {
                DataManager.instance.JsonSave();
                authorizationObject.SetActive(false);
                loginText.text = "로그인 성공!\n환영해요.";
                //CustomerLoginUI.SetActive(true);
                StartCoroutine(nameof(PopupCoroutine));
            }
        }
    }

    private void PermissionDeniedCallback(string permissionName)
    {
        count++;
        OnPopUp();
    }

    public void OnPopUp()
    {
        Popup.SetActive(true);
    }

    public void ClickPopUp()
    {
        Popup.SetActive(false);

        if (count >= 2)
        {
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                string packageName = currentActivityObject.Call<string>("getPackageName");

                using (var uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null))
                using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject))
                {
                    intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                    intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
                    currentActivityObject.Call("startActivity", intentObject);
                }
            }
        }
        else
        {
            if (state == authState.None)
            {
                AuthorizationGPS();
            }
            else if (state == authState.GPS)
            {
                AuthorizationCamera();
            }
            else if (state == authState.Camera)
            {
                AuthorizationAlram();
            }
        }

    }
    #endregion

    #region existCustomer
    public void ShownLoginSccuess()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation) == false || Permission.HasUserAuthorizedPermission(Permission.Camera) == false || Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS") == false)
        {
            authorizationObject.SetActive(true);
            isLogin = true;
        }
        else
        {
            DataManager.instance.JsonSave();
            loginText.text = "로그인 성공!\n환영해요.";

            //CustomerLoginUI.SetActive(true);
            StartCoroutine(nameof(PopupCoroutine));
        }
    }
    #endregion

    private IEnumerator PopupCoroutine()
    {
        CustomerLoginUI.SetActive(true);


        loginImageUI.transform.DOMove(spotPosition.position, 0.8f);
        
        loginImage.DOFade(1, 0.5f);

        yield return new WaitForSeconds(0.5f);
        //rtTour.DOAnchorPosY(0, 0.38f).SetEase(Ease.OutBack);

        //yield return new WaitForSeconds(0.5f);

        //tourState = TourState.UP;
    }

    public void SceneMove() // 씬이동하는 함수 Manager로만들까... -> 공통으로 필요
    {
        SceneManager.instance.ChangeScene(1);
    }

    public void ClickLoginButton()
    {
        SceneMove();
    }

    public void EditorButtonOn()
    {
        SplashUIOff();
        editorButton.SetActive(true);
    }

    public void OnClickEditorButton()
    {
        LoginResponse loginData = new LoginResponse();
        loginData.data = new LoginData();

        loginData.timeStamp = DateTime.Now;
        loginData.status = loginEditorData[0];
        loginData.data.accessToken = loginEditorData[1];
        loginData.data.refreshToken = loginEditorData[2];
        loginData.data.tokenType = loginEditorData[3];
        loginData.data.agreed = bool.Parse(loginEditorData[4]);

        DataManager.instance.SetLoginData(loginData);
        if (loginEditorData[4] == "false")
        {
            SplashUIOff();
            ShowConfirmScrollView();
        }
        else
        {
            SplashUIOff();
            ShownLoginSccuess();
        }
    }

    public void TestPopup()
    {
        StartCoroutine(PopupCoroutine());
    }
}
