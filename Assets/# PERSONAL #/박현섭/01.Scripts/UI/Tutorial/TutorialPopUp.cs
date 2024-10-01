using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField] private Image _background;

    [SerializeField] private Image _picture;
    [SerializeField] private Image _screenshotImage;

    [SerializeField] private Image _dotOne;
    [SerializeField] private Image _dotTwo;
    [SerializeField] private Image _dotThree;
    [SerializeField] private Image _dotFour;
    [SerializeField] private Image _dotFive;

    [SerializeField] private TextMeshProUGUI _headlineText;
    [SerializeField] private TextMeshProUGUI _taillineText;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _endButton;

    [Header("��������Ʈ")]
    [SerializeField] private Sprite _activeDotSprite;
    [SerializeField] private Sprite _deActiveDotSprite;

    [SerializeField] private Sprite indicatorSprite;
    [SerializeField] private Sprite bottomSheetSprite;
    [SerializeField] private Sprite wantPosSprite;

    private int _count = 0;

    private void Start()
    {
        _dotOne.sprite = _activeDotSprite;
        _endButton.GetComponent<Image>().enabled = false;
        _background.GetComponent<Button>().onClick.AddListener(OnClickPopUp);

        _closeButton.onClick.AddListener(OnClickCloseBtn);
        _endButton.onClick.AddListener(OnClickCloseBtn);

        _screenshotImage = _picture.transform.GetChild(0).GetComponent<Image>();
    }

    private void OnClickPopUp()
    {
        if (_coolDown)
            return;

        switch (_count)
        {
            case 0:
                _count++;
                _dotOne.sprite = _deActiveDotSprite;
                _dotTwo.sprite = _activeDotSprite;

                _taillineText.text = "ȭ�鿡�� �� ��ġ�� ������ ���� ���� ���� �ִ� ������ ȭ��ǥ�� ǥ�õſ�. ȭ��ǥ�� ������\n���� ���� �� ��ġ�� �̵��� �� �־��.";

                _picture.rectTransform.sizeDelta = new Vector2(720, 420);

                _screenshotImage.sprite = indicatorSprite;
                _screenshotImage.rectTransform.anchoredPosition = new Vector2(-54, -9);

                _coolDown = true;
                break;
            case 1:
                _count++;
                _dotTwo.sprite = _deActiveDotSprite;
                _dotThree.sprite = _activeDotSprite;

                _taillineText.text = "���� ��Ʈ���� �� �ֺ��� ����Ʈ ��ҿ� ���������� ����� ������ ǥ�õſ�.";

                _picture.rectTransform.sizeDelta = new Vector2(720, 558);

                _screenshotImage.sprite = bottomSheetSprite;
                _screenshotImage.rectTransform.localScale = Vector3.one * 0.7f;
                _screenshotImage.rectTransform.anchoredPosition = new Vector2(-14, 379);

                _coolDown = true;
                break;
            case 2:
                _count++;
                _dotThree.sprite = _deActiveDotSprite;
                _dotFour.sprite = _activeDotSprite;

                _taillineText.text = "���� ��ư�� ������ ���� ���� ������ �׸� ��� �� �� �־��.";

                _screenshotImage.sprite = bottomSheetSprite;

                _screenshotImage.rectTransform.localScale = Vector3.one;
                _screenshotImage.rectTransform.anchoredPosition = new Vector2(154, 437);

                _coolDown = true;
                break;
            case 3:
                _count++;
                _dotFour.sprite = _deActiveDotSprite;
                _dotFive.sprite = _activeDotSprite;

                _endButton.GetComponent<Image>().enabled = true;

                _taillineText.text = "���� ��Ʈ���� ���ϴ� �׸��� ������ �������� �ش� ��ġ�� �� �� �־��.";

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
