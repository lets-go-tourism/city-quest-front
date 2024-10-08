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
    [SerializeField] List<string> loadingText = new List<string>() { "지도 그리는 중...", "커피 타는 중...", "성벽 쌓는 중...", "츄러스로 당 충전 중...", "백발백 중..."};
    [SerializeField] private Vector3 firstTr;
    [SerializeField] TextMeshProUGUI text;

    public bool connectionFinish = false;
    private int count = 0;

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
        }

        image.sprite = loadingImage[count];
        text.text = TextChange();
        image_obj.transform.localPosition = firstTr;
        image.DOFade(0, 0);
        image_obj.transform.DOMoveY(295.56f, 1f).SetRelative().SetEase(Ease.OutBack);
        image.DOFade(1, 0.2f);
        yield return new WaitForSeconds(1f);
        if (!connectionFinish)
        {
            count++;
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

    public string TextChange()
    {
        return loadingText[count];
    }
}
