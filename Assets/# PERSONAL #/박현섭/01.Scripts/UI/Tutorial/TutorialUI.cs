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
    [SerializeField] private Image _invisibleNonTouch;

    [SerializeField] private Image _tutorialBtn;

    [SerializeField] private GameObject _masking1;
    [SerializeField] private GameObject _masking2;
    [SerializeField] private GameObject _masking3;
    [SerializeField] private GameObject _masking4;
    [SerializeField] private GameObject _masking5;
    [SerializeField] private GameObject _masking6;

    [SerializeField] private GameObject _roundMasking;

    public delegate void TutorialStart();
    public delegate void TutorialEnd();

    public TutorialStart _tutorialStartDelegate;
    public TutorialEnd _tutorialEndDelegate;

    private void Start()
    {
        _backgroundDark.enabled = true;
        _tutorialSelectMsg.SetActive(false);
        _invisibleNonTouch.enabled = false;
        _tutorialBtn.enabled = false;
    }

    public void OnBackgroundDark()
    {
        _backgroundDark.enabled = true;
    }

    public void OffBackgroundDark()
    {
        _backgroundDark.enabled = false;
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

        // Ʃ�丮�� ����
        StartCoroutine(nameof(Tutorial));
    }

    private void OnClickNo()
    {
        _backgroundDark.enabled = false;
        _tutorialSelectMsg.SetActive(false);
    }

    private IEnumerator Tutorial()
    {
        if (_tutorialStartDelegate != null)
            _tutorialStartDelegate.Invoke();

        // ��ġ ���ƹ����� ������ Ŀ�� �ν������ ī�޶� �̵�
        _invisibleNonTouch.enabled = true;
        Prop prop = PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[5]];
        MapCameraController.Instance.StartCameraMoveToTarget(prop.PropObj.transform.TransformPoint(prop.GetBoundsCenter()));

        while (true)
        {
            if (MapCameraController.Instance.m_Moving == false)
                break;

            yield return null;
        }

        // ��ġ �ٽ� Ű��
        _invisibleNonTouch.enabled = false;

        // ù��° ����ŷ Ű��
        _masking1.SetActive(true);
        _roundMasking.SetActive(true);

        // ��ǳ�� �ʰ� Ű��
        yield return new WaitForSeconds(0.5f);
        _masking1.transform.GetChild(0).GetComponent<Image>().enabled = true;

        // ���̴� �κп� �Ⱥ��̴� ��ư ���� �־�α�
        _tutorialBtn.rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(prop.PropObj.transform.TransformPoint(prop.GetBoundsCenter()));
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(400, 400);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial2);
    }

    private void StartTutorial2()
    {
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(nameof(Tutorial2));
    }

    private IEnumerator Tutorial2() 
    {
        // ��ư ��Ȱ��ȭ�ϰ�
        _tutorialBtn.enabled = false;
        _masking1.SetActive(false);
        _roundMasking.SetActive(false);

        // ���
        KJY_ConnectionTMP.instance.OnConnectionQuest(5);

        // �ڿ� ���� Ű��
        MainView_UI.instance.BackgroundDarkEnable();

        HttpManager.instance.successDelegate += () => { SettingPropInfo.instance.PropInfoSetting(); };
        HttpManager.instance.errorDelegate += () => { MainView_UI.instance.BackgroundDarkDisable(); };

        // ����� ��ٸ���.
        yield return new WaitForSeconds(5);

        // �ι�° ����ŷ Ų��
        _masking2.SetActive(true);
        _roundMasking.SetActive(true);

        // ��ǳ�� �ʰ� Ű��
        yield return new WaitForSeconds(0.5f);
        _masking2.transform.GetChild(0).GetComponent<Image>().enabled = true;

        _tutorialBtn.rectTransform.anchoredPosition = new Vector3(540, 1730, 0);
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(700, 600);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial3);
    }

    private void StartTutorial3()
    {
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(nameof(Tutorial3));
    }

    private IEnumerator Tutorial3()
    {
        // ��ư ��Ȱ��ȭ�ϰ�
        _tutorialBtn.enabled = false;
        _masking2.SetActive(false);

        // ����° ����ŷ Ų��
        _masking3.SetActive(true);


        // ��ǳ�� �ʰ� Ű��
        yield return new WaitForSeconds(0.5f);
        _masking3.transform.GetChild(0).GetComponent<Image>().enabled = true;

        // ���� ��ġ ����
        yield return new WaitForSeconds(0.1f);

        _tutorialBtn.rectTransform.anchoredPosition = new Vector3(540, 1058, 0);
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(1000, 900);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial4);
    }

    private void StartTutorial4()
    {
        _tutorialBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(nameof(Tutorial4));
    }

    private IEnumerator Tutorial4()
    {
        // ��ư ��Ȱ��ȭ�ϰ�
        _tutorialBtn.enabled = false;
        _masking3.SetActive(false);

        // �׹�° ����ŷ Ų��
        _masking4.SetActive(true);

        // ��ǳ�� �ʰ� Ű��
        yield return new WaitForSeconds(0.5f);
        _masking4.transform.GetChild(0).GetComponent<Image>().enabled = true;

        // ���� ��ġ ����
        yield return new WaitForSeconds(0.1f);

        _tutorialBtn.rectTransform.anchoredPosition = new Vector3(540, 436, 0);
        _tutorialBtn.rectTransform.sizeDelta = new Vector2(1000, 400);
        _tutorialBtn.enabled = true;
        _tutorialBtn.GetComponent<Button>().onClick.AddListener(StartTutorial5);
    }

    private void StartTutorial5()
    {
        gameObject.SetActive(false);
        CameraFeed.Instance.TutorialStart();
    }
}
