using System.Collections;
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

    public IEnumerator QuestDone()
    {
        // ��ư ��ġ �Ұ����ϰ� �����
        content.GetChild(6).transform.GetChild(0).GetComponent<Button>().enabled = false;

        // �� �߱�
        content.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;

        // ����Ʈ ��������Ʈ �ٲٱ�
        yield return StartCoroutine(nameof(ChangeSprite));

        // �˾�â �ݱ�
        yield return PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN), true);

        // ���� ������ �����Լ� ȣ��
        PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;
        PropsController.Instance.PropDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;

        CloudContainer.Instance.RemoveTarget(PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture]]);

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
    }

    IEnumerator ChangeSprite()
    {
        float t = 0;
        float d = 1f;

        Image image = content.GetChild(6).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();

        while (t < d)
        {
            image.fillAmount = Mathf.Lerp(0, 1, t / d);
            t += Time.deltaTime;
            yield return null;
        }

        image.fillAmount = 1f;
    }

    // �±� ��������Ʈ �� ���� �ٲٱ�
    public void ChangeBottomSheet(int num)
    {
        if (BottomSheetMovement.instance.state == BottomSheetMovement.State.UP)
        {
            // ��� �� ����
            if (num == 0)
            {
                Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
                Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
                BS_Place.gameObject.SetActive(true);
            }

            // �������� �� ����
            if (num == 1)
            {
                Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
                Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
                BS_Place.gameObject.SetActive(false);
            }

            // ��ũ�� �ʱ�ȭ
            MainView_UI.instance.tourScrollRect.horizontalNormalizedPosition = 0;
            MainView_UI.instance.placeScrollRect.horizontalNormalizedPosition = 0;
        }
    }
}
