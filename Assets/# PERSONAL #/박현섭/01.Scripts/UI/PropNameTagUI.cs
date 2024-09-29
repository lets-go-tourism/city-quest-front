using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropNameTagUI : MonoBehaviour
{
    public int value = 500;

    public Prop TargetProp { get; private set; }
    private RectTransform rectTransform;

    private Image myImage;
    private TextMeshProUGUI myText;

    [SerializeField] private Color originColor;
    [SerializeField] private Color tintColor;

    private void Awake()
    {
        myImage = GetComponent<Image>();
        myText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        this.enabled = false;
    }

    public void Init(Prop target)
    {
        this.TargetProp = target;
        this.TargetProp.NameTag = this;

        enabled = true;
    }

    public void TintColor()
    {
        myImage.color = tintColor;
    }

    public void OriginColor()
    {
        myImage.color = originColor;
    }

    private void OnEnable()
    {
        myImage.enabled = true;
        myText.enabled = true;

        SettingTextWidth(rectTransform, myText, TargetProp.HomeAdventurePlaceData.name);
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
        rectTrf.sizeDelta = rectSize + new Vector2(32, 0);

        tmpText.overflowMode = TextOverflowModes.Ellipsis;
    }

    private void Update()
    {
        if (PropsController.Instance.TintProp != null && TargetProp == PropsController.Instance.TintProp)
            myImage.color = tintColor;
        else
            myImage.color = originColor;

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(TargetProp.transform.position + new Vector3(0, 0, TargetProp.OffsetY));
        rectTransform.anchoredPosition = screenPoint;
    }

    private void OnDisable()
    {
        TargetProp = null;

        myImage.enabled = false;
        myText.enabled = false;
    }
}
