using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TourNameTagUI : MonoBehaviour
{
    private const int _offsetY = -17;

    public int value = 500;

    public TourData TargetTour { get; private set; }
    public RectTransform RectTransform { get; private set; }

    private Image myImage;
    private TextMeshProUGUI myText;

    [SerializeField] private Color originColor;
    [SerializeField] private Color tintColor;

    public bool Visible { get { return visible; } set
        {
            if (visible == value)
                return;

            visible = value;
            myText.DOPause();
            myImage.DOPause();
            if (value)
            {
                myText.DOFade(1, 0.2f);
                myImage.DOFade(1, 0.2f);
            }
            else
            {
                myText.DOFade(0, 0.2f);
                myImage.DOFade(0, 0.2f);
            }
        }
    }
    private bool visible = true;

    private void Awake()
    {
        myImage = GetComponent<Image>();
        myText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        RectTransform = GetComponent<RectTransform>();
        this.enabled = false;
    }

    public void Init(TourData target)
    {
        enabled = true;
        this.TargetTour = target;
        Visible = true;
        CustomeUpdate();
        SettingTextWidth(RectTransform, myText, TargetTour.ServerTourInfo.title);
    }

    private void SettingTextWidth(RectTransform rectTrf, TextMeshProUGUI tmpText, string textValue)
    {
        tmpText.overflowMode = TextOverflowModes.Overflow;
        tmpText.text = textValue;

        Vector2 textRectSize = tmpText.rectTransform.sizeDelta;

        if (tmpText.preferredWidth > value)
        {
            textRectSize.x = value;
            tmpText.rectTransform.sizeDelta = textRectSize;
        }
        else
        {
            textRectSize.x = tmpText.preferredWidth;
            tmpText.rectTransform.sizeDelta = textRectSize;
        }

        Vector2 rectSize = rectTrf.sizeDelta;
        rectSize.x = textRectSize.x;
        rectTrf.sizeDelta = rectSize + new Vector2(48, 0);

        tmpText.overflowMode = TextOverflowModes.Ellipsis;
    }

    private void Update()
    {
        if (Visible)
        {
            if (PropsController.Instance.TintTourData != null && TargetTour == PropsController.Instance.TintTourData)
                myImage.color = tintColor;
            else
                myImage.color = originColor;
        }

        RectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(TargetTour.transform.position + new Vector3(0, 0, _offsetY * Camera.main.transform.position.y / 500));
    }

    public void CustomeUpdate()
    {
        RectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(TargetTour.transform.position + new Vector3(0, 0, _offsetY * Camera.main.transform.position.y / 500));
    }

    private void OnDisable()
    {
        TargetTour = null;
        Visible = false;
    }
}
