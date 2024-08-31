using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameTagUI : MonoBehaviour
{
    public Prop TargetProp { get; private set; }
    private RectTransform rectTransform;

    private Image myImage;
    private Text myText;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        myImage = GetComponent<Image>();
        myText = transform.GetChild(0).GetComponent<Text>();
        this.enabled = false;
    }

    public void Init(Prop target)
    {
        this.TargetProp = target;
        enabled = true;
    }

    private void OnEnable()
    {
        myImage.enabled = true;
        myText.enabled = true;
    }

    private void Update()
    {
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
