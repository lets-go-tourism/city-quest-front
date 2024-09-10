using System.Runtime.CompilerServices;
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

    public void QuestDone()
    {
        CameraFeed.Instance.CameraOff();

        // 버튼 터치 불가능하게 만들기
        content.GetChild(6).transform.GetChild(0).GetComponent<Button>().enabled = false;

        // 한 2초정도 연출 있으면 좋을듯??
        // 퀘스트 스프라이트 바꾸기
        content.GetChild(6).transform.GetChild(0).GetComponent<Image>().sprite = content.GetChild(6).GetComponent<SpritesHolder>().sprites[2];

        // 줄 긋기
        content.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;

        // 구름 걷히는 연출함수 호출
        PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;
        PropsController.Instance.PropDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;

        CloudContainer.Instance.RemoveTarget(PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture]]);

        // 바텀시트 비활성화
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);

        // 바텀시트 태그 수정하기
        BottomSheetManager.instance.placeGOList[KJY_ConnectionTMP.instance.questNoPicture - 1].GetComponent<CardPlaceInfo>().ChangeType();

        // 태그 초기화
        ChangeSprites.instance.ChangePlaceSprites(0);
        ChangeSprites.instance.ChangeTourSprites(0);
    }

    // 태그 스프라이트 및 내용 바꾸기
    public void ChangeBottomSheet(int num)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
        {
            // 장소 탭 보기
            if (num == 0)
            {
                Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
                Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
                BS_Place.gameObject.SetActive(true);
            }

            // 관광정보 탭 보기
            if (num == 1)
            {
                Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
                Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
                BS_Place.gameObject.SetActive(false);
            }

            // 스크롤 초기화
            MainView_UI.instance.tourScrollRect.horizontalNormalizedPosition = 0;
            MainView_UI.instance.placeScrollRect.horizontalNormalizedPosition = 0;
        }
    }
}
