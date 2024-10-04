using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUp : MonoBehaviour
{
    public static TutorialPopUp instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Image _background;

    [SerializeField] private Image _picture;
    private Image _screenshotImage;

    [SerializeField] private Image _dotOne;
    [SerializeField] private Image _dotTwo;
    [SerializeField] private Image _dotThree;
    [SerializeField] private Image _dotFour;
    [SerializeField] private Image _dotFive;

    [SerializeField] private TextMeshProUGUI _headlineText;
    [SerializeField] private TextMeshProUGUI _taillineText;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _endButton;

    [Header("스프라이트")]
    [SerializeField] private Sprite _activeDotSprite;
    [SerializeField] private Sprite _deActiveDotSprite;

    [SerializeField] private Sprite myPositionSprite;
    [SerializeField] private Sprite indicatorSprite;
    [SerializeField] private Sprite bottomSheetSprite;
    [SerializeField] private Sprite wantPosSprite;

    private int _count = 0;

    private void Start()
    {
        _dotOne.sprite = _activeDotSprite;
        _endButton.GetComponent<Image>().enabled = false;
        //_background.GetComponent<Button>().onClick.AddListener(OnClickPopUp);

        _closeButton.onClick.AddListener(OnClickCloseBtn);
        _endButton.onClick.AddListener(OnClickCloseBtn);

        _screenshotImage = _picture.transform.GetChild(0).GetComponent<Image>();
    }

    public void OnClickPopUp(int countPlus)
    {
        if (_coolDown)
            return;

        _count += countPlus;
        if (_count < 0) _count = 0;

        switch (_count)
        {
            case 0:
                _dotOne.sprite = _activeDotSprite;
                _dotTwo.sprite = _deActiveDotSprite;

                _taillineText.text = "지도 상의 내 위치는 이렇게 표시돼요.";

                _picture.rectTransform.sizeDelta = new Vector2(720, 639);

                _screenshotImage.sprite = myPositionSprite;
                _screenshotImage.rectTransform.anchoredPosition = new Vector2(-42, -50);

                _coolDown = true;
                break;
            case 1:
                _dotOne.sprite = _deActiveDotSprite;
                _dotTwo.sprite = _activeDotSprite;
                _dotThree.sprite = _deActiveDotSprite;

                _taillineText.text = "화면에서 내 위치가 보이지 않을 때는\n내가 있는 방향이 화살표로 표시돼요.\n화살표를 누르면\n지도 상의 내 위치로 이동할 수 있어요.";

                _picture.rectTransform.sizeDelta = new Vector2(720, 420);

                _screenshotImage.sprite = indicatorSprite;
                _screenshotImage.rectTransform.localScale = Vector3.one;
                _screenshotImage.rectTransform.anchoredPosition = new Vector2(-54, -9);

                _coolDown = true;
                break;
            case 2:
                _dotTwo.sprite = _deActiveDotSprite;
                _dotThree.sprite = _activeDotSprite;
                _dotFour.sprite = _deActiveDotSprite;

                _taillineText.text = "바텀 시트에는 내 주변의 퀘스트 장소와\n관광정보가 가까운 순으로 표시돼요.";

                _picture.rectTransform.sizeDelta = new Vector2(720, 558);

                _screenshotImage.sprite = bottomSheetSprite;
                _screenshotImage.rectTransform.localScale = Vector3.one * 0.7f;
                _screenshotImage.rectTransform.anchoredPosition = new Vector2(-14, 379);

                _coolDown = true;
                break;
            case 3:
                _dotThree.sprite = _deActiveDotSprite;
                _dotFour.sprite = _activeDotSprite;
                _dotFive.sprite = _deActiveDotSprite;

                _endButton.GetComponent<Image>().enabled = false;

                _taillineText.text = "바텀 시트에서 원하는 항목을 누르면\n지도에서 해당 위치를 볼 수 있어요.";

                _background.rectTransform.sizeDelta = new Vector2(852, 1035);

                _screenshotImage.sprite = bottomSheetSprite;

                _screenshotImage.rectTransform.localScale = Vector3.one;
                _screenshotImage.rectTransform.anchoredPosition = new Vector2(154, 437);

                _coolDown = true;
                break;
            case 4:
                _dotFour.sprite = _deActiveDotSprite;
                _dotFive.sprite = _activeDotSprite;

                _endButton.GetComponent<Image>().enabled = true;

                _taillineText.text = "필터 버튼을 누르면 보고 싶은 종류의\n항목만 골라 볼 수 있어요."; 

                _background.rectTransform.sizeDelta = new Vector2(852, 1266);

                _screenshotImage.sprite = wantPosSprite;
                _screenshotImage.rectTransform.anchoredPosition = new Vector2(3, -268);
                break;
        }
    }

    private void OnClickCloseBtn()
    {
        _background.gameObject.SetActive(false);
        StartCoroutine(nameof(OnClickClose));
    }

    private IEnumerator OnClickClose()
    {
        TutorialUI.Instance.OnNonTouch();
        TutorialUI.Instance.OffBackgroundDark();
        yield return new WaitForSeconds(1);
        TutorialUI.Instance.OffNonTouch();
        TutorialUI.Instance.OnBackgroundDark();
        TutorialUI.Instance.OnTutorialYesOrNoMsg();
    }

    private float _coolTime = 0;
    private bool _coolDown = false;

    private void Update()
    {
        if (_coolDown)
        {
            _coolTime += Time.deltaTime;

            if (_coolTime > 0.2f)
            {
                _coolTime = 0;
                _coolDown = false;
            }
        }
    }
}
