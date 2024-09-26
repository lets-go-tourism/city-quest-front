using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Image _backgroundDark;
    [SerializeField] private GameObject _tutorialSelectMsg;
    [SerializeField] private Image _invisibleNonTouch;

    [SerializeField] private Image _unmask;
    [SerializeField] private Image _screen;
    [SerializeField] private Image _tutorialBtn;

    public delegate void TutorialStart();
    public delegate void TutorialEnd();

    public TutorialStart _tutorialStartDelegate;
    public TutorialEnd _tutorialEndDelegate;

    private void Start()
    {
        _backgroundDark.enabled = true;
        _tutorialSelectMsg.SetActive(true);
        _invisibleNonTouch.enabled = false;
        _tutorialSelectMsg.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OnClickYes);
        _tutorialSelectMsg.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(OnClickNo);
        _unmask.enabled = false;
        _screen.enabled = false;
        _tutorialBtn.enabled = false;
    }

    private void OnClickYes()
    {
        _backgroundDark.enabled = false;
        _tutorialSelectMsg.SetActive(false);

        // 튜토리얼 시작
        StartCoroutine(nameof(StartTutorial));
    }

    private void OnClickNo()
    {
        _backgroundDark.enabled = false;
        _tutorialSelectMsg.SetActive(false);
    }

    private IEnumerator StartTutorial()
    {
        if (_tutorialStartDelegate != null)
            _tutorialStartDelegate.Invoke();

        _invisibleNonTouch.enabled = true;
        Prop prop = PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[5]];
        MapCameraController.Instance.StartCameraMoveToTarget(prop.transform.position);

        while (true)
        {
            if (MapCameraController.Instance.m_Moving == false)
                break;

            yield return null;
        }


        _invisibleNonTouch.enabled = false;
        _screen.enabled = true;

        _unmask.enabled = true;
        _unmask.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(prop.PropObj.transform.TransformPoint(prop.GetBoundsCenter()));
        _unmask.rectTransform.sizeDelta = new Vector2(400, 400);

        _tutorialBtn.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(prop.PropObj.transform.TransformPoint(prop.GetBoundsCenter()));
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(400, 400);
        _tutorialBtn.enabled = true;

        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial2);
    }

    private void StartTutorial2()
    {
        StartCoroutine(nameof(Tutorial2));
    }

    private IEnumerator Tutorial2() 
    {
        _tutorialBtn.enabled = false;
        _unmask.enabled = false;
        _screen.enabled = false;

        KJY_ConnectionTMP.instance.OnConnectionQuest(5);

        MainView_UI.instance.BackgroundDarkEnable();

        HttpManager.instance.successDelegate += () => { SettingPropInfo.instance.PropInfoSetting(); };
        HttpManager.instance.errorDelegate += () => { MainView_UI.instance.BackgroundDarkDisable(); };
        yield return new WaitForSeconds(2);



    }

    private void StartTutorial3()
    {
        
    }
}
