using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameTagUI : MonoBehaviour
{
    public enum State
    {
        Prop,
        TourData
    }

    public State state;

    public Prop TargetProp { get; private set; }
    public TourData TourData { get; private set; }
    private RectTransform rectTransform;

    private Image myImage;
    private TextMeshProUGUI myText;

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
        state = State.Prop;
        enabled = true;
    }

    public void Init(TourData target)
    {
        this.TourData = target;
        state = State.TourData;
        enabled = true;
    }

    private void OnEnable()
    {
        myImage.enabled = true;
        myText.enabled = true;

        SettingTextWidth(rectTransform, myText, state == State.Prop ? TargetProp.HomeAdventurePlaceData.name : TourData.ServerTourInfo.title);
    }

    private void SettingTextWidth(RectTransform rectTrf, TextMeshProUGUI tmpText, string textValue)
    {
        tmpText.text = textValue;

        Vector2 rectSize = rectTrf.sizeDelta;
        rectSize.x = tmpText.preferredWidth;
        rectTrf.sizeDelta = rectSize + new Vector2(48, 0);
    }

    private void Update()
    {
        Vector3 screenPoint = state == State.Prop ? Camera.main.WorldToScreenPoint(TargetProp.transform.position + new Vector3(0, 0, TargetProp.OffsetY)) : Camera.main.WorldToScreenPoint(TourData.transform.position + new Vector3(0, 0, -15f));
        rectTransform.anchoredPosition = screenPoint;
    }

    private void OnDisable()
    {
        TargetProp = null;

        myImage.enabled = false;
        myText.enabled = false;
    }
}
