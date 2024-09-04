using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public enum authState
{
    None,
    GPS,
    Camera,
    Alarm
}

public class KJY_UIManager : MonoBehaviour
{
    public static KJY_UIManager instance;

    [Header("splash_onBoard")]
    [SerializeField] private GameObject splash_onBoardObject;
    [SerializeField] private RectTransform logoFirstPosition;
    [SerializeField] private RectTransform logoSecondPosition;
    [SerializeField] private GameObject logo;
    [SerializeField] private TextMeshProUGUI explain;

    [Header("OnBoard")]
    [SerializeField] private GameObject kakaoBtn;


    [Header("Term&Confirm")]
    [SerializeField] private GameObject confrimObject;
    [SerializeField] private Scrollbar confirmScrollView;
    [SerializeField] private ScrollRect confirmScrollViewtest;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Image confirmBtnSprite;
    [SerializeField] private Sprite confimrBtnClickSpirte;
    private bool isConfirmView = false;

    [Header("Authorization")]
    [SerializeField] private GameObject authorizationObject;
    [SerializeField] private GameObject authorizationStartBtn;
    [SerializeField] private Sprite authorizationStartBtnClickSprite;
    [SerializeField] private GameObject Popup;

    [Header("LoginSuccess")]
    [SerializeField] private TextMeshProUGUI loginText;
    [SerializeField] private GameObject CustomerLoginUI;
    [SerializeField] private GameObject CustomerLoginBtn;
    [SerializeField] private Sprite CustomerLoginBtnClickSprite;

    [SerializeField] private GameObject testBtn;
    [SerializeField] private TextMeshProUGUI text1;

    public authState state = authState.None;
    private bool isLogin = false;


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
        KJY_UIManager.instance.StartSplash();
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
        yield return logo.transform.DOMove(logoSecondPosition.position, 1f);
        //explain.DOFade(1f, 1f);
        //explain.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        //���⼭ �α��ε��ִ��� �ȵ��ִ����� ��ū ������ ���� �ٷ� �̵��������� �ƴϸ� �º������� �̵����� ����
        if (DataManager.instance.GetLoginData() == null || DataManager.instance.isLogout == true) // �α��ξȵƴ°��
        {
            isConfirmView = true;
            kakaoBtn.SetActive(true);
        }
        else
        {
            LoginCheck();
        }
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
    //ȸ�������Ͻ�
    public void ShowConfirmScrollView()
    {
        splash_onBoardObject.SetActive(false);
        kakaoBtn.SetActive(false);
        //���⼭ �α����ߴ��� ���ߴ����� ���� �޶���
        confrimObject.SetActive(true);
        isConfirmView = true;
    }

    public void OnClickConfirmButton()
    {
        confirmBtn.GetComponent<Image>().sprite = confimrBtnClickSpirte; 
        confrimObject.SetActive(false);
        authorizationObject.SetActive(true);
        isConfirmView = false;
    }

    public void OnClickAuthorizationButton()
    {
        authorizationStartBtn.GetComponent<Image>().sprite = authorizationStartBtnClickSprite;
        AuthorizationGPS();
    }

    public void AuthorizationGPS() //GPS ���� ����޴� �Լ�
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

    public void AuthorizationCamera() //Camera ���� ����޴� �Լ�
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

    public void AuthorizationAlram()//Alram�����°� ���� ����޴� �Լ�
    {
#if UNITY_ANDROID
        if (Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            if (isLogin == false)
            {
                DataManager.instance.JsonSave();
                authorizationObject.SetActive(false);
                loginText.text = "ȸ������ ����!\nȯ���ؿ�.";
                CustomerLoginUI.SetActive(true);
            }
            else
            {
                DataManager.instance.JsonSave();
                authorizationObject.SetActive(false);
                loginText.text = "�α��� ����!\nȯ���ؿ�.";
                CustomerLoginUI.SetActive(true);
            }
        }
        else
        {
            PermissionCallbacks callbacks = new();
            callbacks.PermissionGranted += PermissionGrantedCallback;
            callbacks.PermissionDenied += PermissionDeniedCallback;

            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS", callbacks);
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
            DataManager.instance.JsonSave();
            state = authState.GPS;
            AuthorizationCamera();
        }
        else if (state == authState.GPS)
        {
            state = authState.Camera;
            AuthorizationAlram();
        }
        else if (state == authState.Camera)
        {
            if (isLogin == false)
            {
                DataManager.instance.JsonSave();
                authorizationObject.SetActive(false);
                loginText.text = "ȸ������ ����!\nȯ���ؿ�.";
                CustomerLoginUI.SetActive(true);
            }
            else
            {
                DataManager.instance.JsonSave();
                authorizationObject.SetActive(false);
                loginText.text = "�α��� ����!\nȯ���ؿ�.";
                CustomerLoginUI.SetActive(true);
            }
        }
    }

    private void PermissionDeniedCallback(string permissionName)
    {
        OnPopUp();
    }

    public void OnPopUp()
    {
        Popup.SetActive(true);
    }

    public void ClickPopUp()
    {
        Popup.SetActive(false);
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
    #endregion

    #region existCustomer
    public void ShownLoginSccuess()
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
            splash_onBoardObject.SetActive(false);
            kakaoBtn.SetActive(false);
            loginText.text = "�α��� ����!\nȯ���ؿ�.";
            CustomerLoginUI.SetActive(true);
        }
    }
    #endregion

    public void SceneMove() // ���̵��ϴ� �Լ� Manager�θ����... -> �������� �ʿ�
    {
        SceneManager.instance.ChangeScene(1);
    }

    public void ClickLoginButton()
    {
        CustomerLoginBtn.GetComponent<Image>().sprite = CustomerLoginBtnClickSprite;
        SceneMove();
    }

}
