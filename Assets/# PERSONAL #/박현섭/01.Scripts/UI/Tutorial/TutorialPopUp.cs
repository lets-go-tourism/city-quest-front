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

    [Header("스프라이트")]
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

                _taillineText.text = "화면에서 내 위치가 보이지 않을 때는 내가 있는 방향이 화살표로 표시돼요. 화살표를 누르면 지도 상의 내 위치로 이동할 수 있어요.";

                _picture.rectTransform.sizeDelta = new Vector2(720, 420);

                _coolDown = true;
                break;
            case 1:
                _count++;
                _dotTwo.sprite = _deActiveDotSprite;
                _dotThree.sprite = _activeDotSprite;

                _taillineText.text = "바텀 시트에는 내 주변의 퀘스트 장소와 관광정보가 가까운 순으로 표시돼요.";

                _picture.rectTransform.sizeDelta = new Vector2(720, 558);

                _coolDown = true;
                break;
            case 2:
                _count++;
                _dotThree.sprite = _deActiveDotSprite;
                _dotFour.sprite = _activeDotSprite;

                _taillineText.text = "필터 버튼을 누르면 보고 싶은 종류의 항목만 골라 볼 수 있어요.";

                _coolDown = true;
                break;
            case 3:
                _count++;
                _dotFour.sprite = _deActiveDotSprite;
                _dotFive.sprite = _activeDotSprite;

                _endButton.GetComponent<Image>().enabled = true;

                _taillineText.text = "바텀 시트에서 원하는 항목을 누르면 지도에서 해당 위치를 볼 수 있어요.";

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
