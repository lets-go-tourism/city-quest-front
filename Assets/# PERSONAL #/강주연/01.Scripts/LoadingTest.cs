using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TriangleNet;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTest : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject image_obj;
    [SerializeField] Image image;
    [SerializeField] List<Sprite> loadingImage = new List<Sprite>();
    [SerializeField] private Vector3 firstTr;
    [SerializeField] TextMeshProUGUI text;

    public bool connectionFinish = false;
    private int count = 0;
    private int textCount = 1;

    private void Start()
    {
        LoadCanvasOn();
        StartCoroutine(loadingStart());
    }

    IEnumerator loadingStart()
    {
        if (count == loadingImage.Count)
        {
            count = 0;
            if (textCount % 4 == 0)
            {
                TextChange();
            }
        }

        image.sprite = loadingImage[count];
        image_obj.transform.localPosition = firstTr;
        image.DOFade(0, 0);
        image_obj.transform.DOMoveY(295.56f, 1f).SetRelative().SetEase(Ease.OutBack);
        image.DOFade(1, 0.2f);
        yield return new WaitForSeconds(1f);
        if (!connectionFinish)
        {
            count++;
            textCount++;
            StartCoroutine(loadingStart());
        }
        else
        {
            LoadingCanvasOff();
            yield return null;

            MapUIController.Instance.NameTagContainer.CollisionUpdate();

            yield break;
        }
    }

    private void LoadingCanvasOff()
    {
        canvas.enabled = false;
    }

    public void LoadCanvasOn()
    {
        canvas.enabled = true;
    }

    public void TextChange()
    {
        if (text.text.Contains("지"))
        {
           text.text = "미니어쳐 놓는 중...";
        }
        else
        {
            text.text = "지도 펼치는 중...";
        }
    }
}
