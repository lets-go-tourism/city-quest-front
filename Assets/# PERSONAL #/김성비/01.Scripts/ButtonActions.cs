using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    public static ButtonActions Instance;

    private void Awake()
    {
        Instance = this;
    }

    Button btn;
    public Transform BS_Place;
    public Transform content;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public IEnumerator QuestDone()
    {
        // ��ư ��ġ �Ұ����ϰ� �����
        content.GetChild(6).transform.GetChild(0).GetComponent<Button>().enabled = false;

        // �� �߱�
        content.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        content.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0.592f, 0.522f, 0.400f, 1f);

        // ����Ʈ ��������Ʈ �ٲٱ�
        yield return StartCoroutine(nameof(ChangeSprite));

        // �˾�â �ݱ�
        yield return PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);

        // ���� ������ �����Լ� ȣ��
        PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;
        PropsController.Instance.PropDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;

        Prop targetProp = PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture]];

        CloudContainer.Instance.RemoveTarget(targetProp);
        TutorialObj.instance.Cloud.StartSetting(targetProp);

        // ���ҽ�Ʈ �±� �����ϱ�
        for (int i = 0; i < BottomSheetManager.instance.contentPlace.childCount; i++)
        {
            if (BottomSheetManager.instance.contentPlace.GetChild(i).GetComponent<CardPlaceInfo>().ServerProp.propNo == KJY_ConnectionTMP.instance.questNoPicture)
            {
                BottomSheetManager.instance.contentPlace.GetChild(i).GetComponent<CardPlaceInfo>().ChangeType();
            }
        }

        // ���ҽ�Ʈ �ʱ�ȭ
        ChangeSprites.instance.place[0].GetComponent<Image>().sprite = ChangeSprites.instance.place[0].GetComponent<SpritesHolder>().sprites[1];
        ChangeSprites.instance.place[1].GetComponent<Image>().sprite = ChangeSprites.instance.place[0].GetComponent<SpritesHolder>().sprites[0];
        ChangeSprites.instance.place[2].GetComponent<Image>().sprite = ChangeSprites.instance.place[0].GetComponent<SpritesHolder>().sprites[0];

        BottomSheetManager.instance.SortingAll(true);
        MainView_UI.instance.placeScrollRect.horizontalNormalizedPosition = 0;

        yield return null;
        yield return new WaitForSeconds(TutorialObj.instance.Cloud.GetAnimTime());

        if (CameraFeed.Instance.isTutorial)
        {
            yield return new WaitForSeconds(1.5f);
            CameraFeed.Instance.isTutorial = false;
            TutorialUI.Instance.StartTutorial2_1();
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            TutorialUI.Instance.OffNonTouch();
        }
    }

    IEnumerator ChangeSprite()
    {
        float t = 0;
        float d = 0.7f;

        Image image = content.GetChild(6).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();

        while (t < d)
        {
            image.fillAmount = Mathf.Lerp(0, 1, t / d);
            t += Time.deltaTime;
            yield return null;
        }

        image.fillAmount = 1f;

        yield return new WaitForSeconds(0.3f);
    }

    // �±� ��������Ʈ �� ���� �ٲٱ�
    public void ChangeBottomSheet(int num)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
        {
            //KJY�߰�
            SettingManager.instance.EffectSound_ButtonTouch();

            // ��ƮƮ������
            RectTransform rtPlace = Props_UI.instance.tags[0].GetComponent<RectTransform>();
            RectTransform rtTour = Props_UI.instance.tags[1].GetComponent<RectTransform>();

            // ��� �� ����
            if (num == 0)
            {
                // ��ġ
                rtPlace.anchoredPosition = new Vector2(267, 3);
                rtTour.anchoredPosition = new Vector2(-270, 0);

                // ������
                rtPlace.sizeDelta = new Vector2(549, 126);
                rtTour.sizeDelta = new Vector2(537, 120);

                // ��������Ʈ
                Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
                Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];

                // Ȱ��ȭ
                BS_Place.gameObject.SetActive(true);
            }

            // �������� �� ����
            if (num == 1)
            {
                // ��ġ
                rtPlace.anchoredPosition = new Vector2(270, 0);
                rtTour.anchoredPosition = new Vector2(-267, 3);

                // ������
                rtPlace.sizeDelta = new Vector2(537, 120);
                rtTour.sizeDelta = new Vector2(549, 126);

                // ��������Ʈ
                Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
                Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];

                // ��Ȱ��ȭ
                BS_Place.gameObject.SetActive(false);
            }

            // ��ũ�� �ʱ�ȭ
            MainView_UI.instance.tourScrollRect.horizontalNormalizedPosition = 0;
            MainView_UI.instance.placeScrollRect.horizontalNormalizedPosition = 0;
        }
    }

    public void ChangeCancel(bool place)
    {
        HttpManager.instance.AbortRequest();
        PopUpMovement.instance.skeleton = false;

        CancelConnection(place);
    }

    public void CancelConnection(bool place)
    {
        if (place)
        {
            //SettingPropInfo.instance.StopCoroutine(nameof(SettingPropInfo.instance.GetTexture));

            PopUpMovement.instance.StopCoroutine(nameof(PopUpMovement.instance.MoveUP));
            PopUpMovement.instance.rtPlace.anchoredPosition = new Vector2(0, -2600);

            if (PopUpMovement.instance.adventured)
            {
                PopUpMovement.instance.placeADcancel = true;
                PopUpMovement.instance.skPlaceAD.DOAnchorPosY(-2600, 0.38f);
            }
            else
            {
                PopUpMovement.instance.placeUNCancel = true;
                PopUpMovement.instance.skPlaceUN.DOAnchorPosY(-2600, 0.38f);
            }
        }
        else
        {
            //SettingTourInfo.instance.StopCoroutine(nameof(SettingTourInfo.instance.GetTexture));
            PopUpMovement.instance.tourCancel = true;
            PopUpMovement.instance.StopCoroutine(nameof(PopUpMovement.instance.MoveUP));

            PopUpMovement.instance.rtTour.anchoredPosition = new Vector2(0, -2500);

            PopUpMovement.instance.skTour.DOAnchorPosY(-2500, 0.39f);
        }

        MainView_UI.instance.BackgroundDarkDisable();
    }
}