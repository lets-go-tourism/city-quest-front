using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField] private Image _background;

    [SerializeField] private Image _picture;

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

    private int _count = 0;

    private void Start()
    {
        _dotOne.sprite = _activeDotSprite;
        _endButton.GetComponent<Image>().enabled = false;
        _background.GetComponent<Button>().onClick.AddListener(OnClickPopUp);

        _closeButton.onClick.AddListener(OnClickCloseBtn);
        _endButton.onClick.AddListener(OnClickCloseBtn);
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

                _taillineText.text = "ȭ�鿡�� �� ��ġ�� ������ ���� ���� ���� �ִ� ������ ȭ��ǥ�� ǥ�õſ�. ȭ��ǥ�� ������ ���� ���� �� ��ġ�� �̵��� �� �־��.";

                _picture.rectTransform.sizeDelta = new Vector2(720, 420);

                _coolDown = true;
                break;
            case 1:
                _count++;
                _dotTwo.sprite = _deActiveDotSprite;
                _dotThree.sprite = _activeDotSprite;

                _taillineText.text = "���� ��Ʈ���� �� �ֺ��� ����Ʈ ��ҿ� ���������� ����� ������ ǥ�õſ�.";

                _picture.rectTransform.sizeDelta = new Vector2(720, 558);

                _coolDown = true;
                break;
            case 2:
                _count++;
                _dotThree.sprite = _deActiveDotSprite;
                _dotFour.sprite = _activeDotSprite;

                _taillineText.text = "���� ��ư�� ������ ���� ���� ������ �׸� ��� �� �� �־��.";

                _coolDown = true;
                break;
            case 3:
                _count++;
                _dotFour.sprite = _deActiveDotSprite;
                _dotFive.sprite = _activeDotSprite;

                _endButton.GetComponent<Image>().enabled = true;

                _taillineText.text = "���� ��Ʈ���� ���ϴ� �׸��� ������ �������� �ش� ��ġ�� �� �� �־��.";

                _background.rectTransform.sizeDelta = new Vector2(852, 1266);
                break;
        }
    }

    private void OnClickCloseBtn()
    {
        _background.gameObject.SetActive(false);
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
