using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    Button btn;
    public Transform BS_Place;
    public Transform content;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public void QuestDone()
    {
        // ��ư ��ġ �Ұ����ϰ� �����
        content.GetChild(6).transform.GetChild(0).GetComponent<Button>().enabled = false;

        // �� 2������ ���� ������ ������??
        // ����Ʈ ��������Ʈ �ٲٱ�
        content.GetChild(6).transform.GetChild(0).GetComponent<Image>().sprite = content.GetChild(6).GetComponent<SpritesHolder>().sprites[2];

        // �� �߱�
        content.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;

        // ���� ������ �����Լ� ȣ��
        PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;
        PropsController.Instance.PropDic[KJY_ConnectionTMP.instance.questNoPicture].status = true;

        CloudContainer.Instance.RemoveTarget(PropsController.Instance.ServerAdventurePlaceWorldDic[PropsController.Instance.AdventurePlaceDic[KJY_ConnectionTMP.instance.questNoPicture]]);

        // ���ҽ�Ʈ ��Ȱ��ȭ
        PopUpMovement.instance.StartCoroutine(nameof(PopUpMovement.instance.MoveDOWN),true);
    }

    // �±� ��������Ʈ �� ���� �ٲٱ�
    public void ChangeBottomSheet(int num)
    {
        // ��� ��
        if (num == 0)
        {
            Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
            Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
            BS_Place.gameObject.SetActive(true);
        }

        // �������� ��
        if (num == 1)
        {
            Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
            Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
            BS_Place.gameObject.SetActive(false);
        }
    }
}
