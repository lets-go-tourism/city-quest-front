using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private Image _backgroundDark;
    [SerializeField] private GameObject _tutorialSelectMsg;
    [SerializeField] private GameObject tutorialEndMsg;
    [SerializeField] private Image _invisibleNonTouch;

    [SerializeField] private Image _tutorialBtn;

    [SerializeField] private GameObject _masking1_1;
    [SerializeField] private GameObject _masking1_2;
    [SerializeField] private GameObject _masking1_3;
    [SerializeField] private GameObject _masking1_4;
    [SerializeField] private GameObject _masking2_1;
    [SerializeField] private GameObject _masking2_2;
    [SerializeField] private GameObject _masking2_3;
    [SerializeField] private GameObject _masking2_4;

    private Canvas canvas;

    //[SerializeField] private GameObject _roundMasking;

    private void Start()
    {
        _backgroundDark.enabled = true;

        _tutorialSelectMsg.SetActive(false);
        tutorialEndMsg.SetActive(false);
        _invisibleNonTouch.enabled = false;
        _tutorialBtn.enabled = false;

        canvas = GetComponent<Canvas>();

        prevHeight = Screen.height;
        prevWidth = Screen.width;

        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(prevWidth, prevHeight);
    }

    private float prevWidth = 0;
    private float prevHeight = 0;

    private void Update()
    {
        // 해상도가 바뀔경우 자식 0번 1번의 width height도 바꾼다
        if (prevHeight != Screen.height || prevWidth != Screen.width)
        {
            prevWidth = Screen.width;
            prevHeight = Screen.height;

            transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(prevWidth, prevHeight);
        }
    }

    public void OnBackgroundDark()
    {
        _backgroundDark.enabled = true;
    }

    public void OffBackgroundDark()
    {
        _backgroundDark.enabled = false;
    }

    public void OnNonTouch()
    {
        _invisibleNonTouch.enabled = true;
    }

    public void OffNonTouch()
    {
        _invisibleNonTouch.enabled = false;
    }

    public void OnTutorialYesOrNoMsg()
    {
        _tutorialSelectMsg.SetActive(true);
        _tutorialSelectMsg.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OnClickYes);
        _tutorialSelectMsg.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(OnClickNo);
    }

    private void OnClickYes()
    {
        _backgroundDark.enabled = false;
        _tutorialSelectMsg.SetActive(false);

        //KJY 추가
        SettingManager.instance.EffectSound_ButtonTouch();

        CloudContainer.Instance.AddTarget(PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[3]]);

        // 튜토리얼 시작
        StartCoroutine(nameof(Tutorial1_1));
    }

    private void OnClickNo()
    {
        _backgroundDark.enabled = false;
        _tutorialSelectMsg.SetActive(false);

        //KJY 추가
        SettingManager.instance.EffectSound_ButtonTouch();

        CameraFeed.Instance.isTutorial = false;
        DataManager.instance.SaveTutorialInfo();
    }

    private IEnumerator Tutorial1_1()
    {
        //KJY 추가
        CameraFeed.Instance.isTutorial = true;

        // 터치 막아버리고 정지영 커피 로스터즈로 카메라 이동
        OnNonTouch();
        Prop prop = PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[3]];
        MapCameraController.Instance.StartCameraMoveToTarget(prop.PropObj.transform.TransformPoint(prop.GetBoundsCenter()));
        PropsController.Instance.TintProp = prop;

        while (true)
        {
            if (MapCameraController.Instance.m_Moving == false)
                break;

            yield return null;
        }

        // 첫번째 마스킹 키고
        _masking1_1.SetActive(true);
        _masking1_1.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(prop.PropObj.transform.TransformPoint(prop.GetBoundsCenter())) - new Vector3(0, 20, 0);
        //_roundMasking.SetActive(true);

        // 말풍선 늦게 키기
        yield return new WaitForSeconds(0.5f);
        _masking1_1.transform.GetChild(0).GetComponent<Image>().enabled = true;

        // 보이는 부분에 안보이는 버튼 갓다 넣어두기
        _tutorialBtn.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(prop.PropObj.transform.TransformPoint(prop.GetBoundsCenter()));
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(400, 400);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial1_2);
    }

    private void StartTutorial1_2()
    {
        SettingManager.instance.EffectSound_PopUpTouch();
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        // 버튼 비활성화하고
        _tutorialBtn.enabled = false;
        _masking1_1.SetActive(false);
        //_roundMasking.SetActive(false);

        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
        HttpManager.instance.successDelegate += () => {SettingPropInfo.instance.PropInfoSetting(); StartCoroutine(nameof(Tutorial1_2)); };
        HttpManager.instance.errorDelegate += () => { MainView_UI.instance.BackgroundDarkDisable(); };

        // 통신
        KJY_ConnectionTMP.instance.OnConnectionQuest(3);

        // 뒤에 암전 키고
        MainView_UI.instance.BackgroundDarkEnable();

    }

    private IEnumerator Tutorial1_2() 
    {
        // 통신을 기다린다.
        yield return new WaitForSeconds(3.5f);

        // 두번째 마스킹 킨다
        _masking1_2.SetActive(true);
        //_roundMasking.SetActive(true);

        // 말풍선 늦게 키기
        yield return new WaitForSeconds(0.5f);
        _masking1_2.transform.GetChild(0).GetComponent<Image>().enabled = true;

        _tutorialBtn.rectTransform.anchoredPosition = new Vector3(540, 1730, 0);
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(700, 600);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial1_3);
    }

    private void StartTutorial1_3()
    {
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(nameof(Tutorial1_3));
    }

    private IEnumerator Tutorial1_3()
    {
        // 버튼 비활성화하고
        _tutorialBtn.enabled = false;
        _masking1_2.SetActive(false);

        // 세번째 마스킹 킨다
        _masking1_3.SetActive(true);


        // 말풍선 늦게 키기
        yield return new WaitForSeconds(0.5f);
        _masking1_3.transform.GetChild(0).GetComponent<Image>().enabled = true;

        // 연속 터치 방지
        yield return new WaitForSeconds(0.1f);

        _tutorialBtn.rectTransform.anchoredPosition = new Vector3(540, 1058, 0);
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(1000, 900);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial1_4);
    }

    private void StartTutorial1_4()
    {
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(nameof(Tutorial1_4));
    }

    private IEnumerator Tutorial1_4()
    {
        // 버튼 비활성화하고
        _tutorialBtn.enabled = false;
        _masking1_3.SetActive(false);

        // 네번째 마스킹 킨다
        _masking1_4.SetActive(true);

        // 말풍선 늦게 키기
        yield return new WaitForSeconds(0.5f);
        _masking1_4.transform.GetChild(0).GetComponent<Image>().enabled = true;

        // 연속 터치 방지
        yield return new WaitForSeconds(0.1f);

        _tutorialBtn.rectTransform.anchoredPosition = new Vector3(540, 436, 0);
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(1000, 400);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(CameraFeed.Instance.SetWebCam);
    }

    public void EndTutorial1()
    {
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        _masking1_4.SetActive(false);
    }

    public void StartTutorial2_1()
    {
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(nameof(Tutorial2_1));
    }

    private IEnumerator Tutorial2_1() 
    {
        //canvas.enabled = true;
        Prop prop = PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[3]];

        // 첫번째 마스킹 키고
        _masking2_1.SetActive(true);
        _masking2_1.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(prop.PropObj.transform.TransformPoint(prop.GetBoundsCenter())) - new Vector3(0, 20, 0);
        //_roundMasking.SetActive(true);

        // 말풍선 늦게 키기
        yield return new WaitForSeconds(0.5f);
        _masking2_1.transform.GetChild(0).GetComponent<Image>().enabled = true;

        // 보이는 부분에 안보이는 버튼 갓다 넣어두기
        _tutorialBtn.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(prop.PropObj.transform.TransformPoint(prop.GetBoundsCenter()));
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(400, 400);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial2_2);
    }

    private void StartTutorial2_2()
    {
        SettingManager.instance.EffectSound_PopUpTouch();
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();

        // 버튼 비활성화하고
        _tutorialBtn.enabled = false;
        _masking2_1.SetActive(false);

        // 통신
        KJY_ConnectionTMP.instance.OnConnectionQuest(3);

        // 뒤에 암전 키고
        MainView_UI.instance.BackgroundDarkEnable();

        PopUpMovement.instance.adventured = true;
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveUP), true);
        HttpManager.instance.successDelegate += () => { SettingPropInfo.instance.PropInfoSetting(); StartCoroutine(nameof(Tutorial2_2)); };
        HttpManager.instance.errorDelegate += () => { MainView_UI.instance.BackgroundDarkDisable(); };
    }

    private IEnumerator Tutorial2_2()
    {
        // 팝업이 올라오는걸 기다린다
        yield return new WaitForSeconds(3.5f);

        // 두번째 마스킹 킨다
        _masking2_2.SetActive(true);
        //_roundMasking.SetActive(true);

        // 말풍선 늦게 키기
        yield return new WaitForSeconds(0.5f);
        _masking2_2.transform.GetChild(0).GetComponent<Image>().enabled = true;

        _tutorialBtn.rectTransform.anchoredPosition = new Vector3(540, 1700, 0);
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(700, 750);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial2_3);
    }

    private void StartTutorial2_3()
    {
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(nameof(Tutorial2_3));
    }

    private IEnumerator Tutorial2_3() 
    {
        // 버튼 비활성화하고
        _tutorialBtn.enabled = false;
        _masking2_2.SetActive(false);

        // 세번째 마스킹 킨다
        _masking2_3.SetActive(true);

        // 말풍선 늦게 키기
        yield return new WaitForSeconds(0.5f);
        _masking2_3.transform.GetChild(0).GetComponent<Image>().enabled = true;

        _tutorialBtn.rectTransform.anchoredPosition = new Vector3(540, 832, 0);
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(1000, 1000);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial2_4);
    }

    private void StartTutorial2_4()
    {
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(nameof(Tutorial2_4));
    }

    private IEnumerator Tutorial2_4()
    {
        // 버튼 비활성화하고
        _tutorialBtn.enabled = false;
        _masking2_3.SetActive(false);

        // 네번째 마스킹 킨다
        _masking2_4.SetActive(true);

        // 말풍선 늦게 키기
        yield return new WaitForSeconds(0.5f);
        _masking2_4.transform.GetChild(0).GetComponent<Image>().enabled = true;

        _tutorialBtn.rectTransform.anchoredPosition = new Vector3(913, 1958, 0);
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(200, 200);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(EndTutorial2);
    }

    private void EndTutorial2()
    {
        SettingManager.instance.EffectSound_PopDown();
        // 팝업 창 끄기
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);
        Props_UI.instance.ResetScollView();
        _masking2_4.SetActive(false);
        _tutorialBtn.enabled = false;
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();

        StartCoroutine(nameof(TutorialEnd));
    }

    private IEnumerator TutorialEnd()
    {
        yield return new WaitForSeconds(1.5f);
        OffNonTouch();
        OnBackgroundDark();
        tutorialEndMsg.SetActive(true);
        tutorialEndMsg.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(RealEndTutorial);
    }

    private void RealEndTutorial()
    {
        SettingManager.instance.EffectSound_ButtonTouch();
        OffBackgroundDark();
        tutorialEndMsg.SetActive(false);
        DataManager.instance.SaveTutorialInfo();
    }
}
