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
        // 버튼 터치 불가능하게 만들기
        content.GetChild(6).transform.GetChild(0).GetComponent<Button>().enabled = false;

        // 줄 긋기
        content.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        content.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0.592f, 0.522f, 0.400f, 1f);

        // 퀘스트 스프라이트 바꾸기
        yield return StartCoroutine(nameof(ChangeSprite));

        // 팝업창 닫기
        yield return PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);

        // 구름 걷히는 연출함수 호출
        PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;
        PropsController.Instance.PropDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;

        Prop targetProp = PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture]];

        CloudContainer.Instance.RemoveTarget(targetProp);
        TutorialObj.instance.Cloud.StartSetting(targetProp);

        // 바텀시트 태그 수정하기
        for (int i = 0; i < BottomSheetManager.instance.contentPlace.childCount; i++)
        {
            if (BottomSheetManager.instance.contentPlace.GetChild(i).GetComponent<CardPlaceInfo>().ServerProp.propNo == KJY_ConnectionTMP.instance.questNoPicture)
            {
                BottomSheetManager.instance.contentPlace.GetChild(i).GetComponent<CardPlaceInfo>().ChangeType();
            }
        }

        // 바텀시트 초기화
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

    // 태그 스프라이트 및 내용 바꾸기
    public void ChangeBottomSheet(int num)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
        {
            //KJY추가
            SettingManager.instance.EffectSound_ButtonTouch();

            // 렉트트랜스폼
            RectTransform rtPlace = Props_UI.instance.tags[0].GetComponent<RectTransform>();
            RectTransform rtTour = Props_UI.instance.tags[1].GetComponent<RectTransform>();

            // 장소 탭 보기
            if (num == 0)
            {
                // 위치
                rtPlace.anchoredPosition = new Vector2(267, 3);
                rtTour.anchoredPosition = new Vector2(-270, 0);

                // 사이즈
                rtPlace.sizeDelta = new Vector2(549, 126);
                rtTour.sizeDelta = new Vector2(537, 120);

                // 스프라이트
                Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
                Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];

                // 활성화
                BS_Place.gameObject.SetActive(true);
            }

            // 관광정보 탭 보기
            if (num == 1)
            {
                // 위치
                rtPlace.anchoredPosition = new Vector2(270, 0);
                rtTour.anchoredPosition = new Vector2(-267, 3);

                // 사이즈
                rtPlace.sizeDelta = new Vector2(537, 120);
                rtTour.sizeDelta = new Vector2(549, 126);

                // 스프라이트
                Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
                Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];

                // 비활성화
                BS_Place.gameObject.SetActive(false);
            }

            // 스크롤 초기화
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