using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    Button btn;
    public Transform bs_Tour;
    public Transform content;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public void QuestDone()
    {
        // �𵨸� Ȱ��ȭ
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.propModeling.gameObject.SetActive(true);

        // ī�޶� UI ��Ȱ��ȭ
        Props_UI.instance.CanvasCamera.enabled = false;

        // �˾�â UI Ȱ��ȭ
        Props_UI.instance.canvasProp.enabled = true;

        // ����Ʈ ��������Ʈ �ٲٱ�
        content.GetChild(6).transform.GetChild(0).GetComponent<Image>().sprite = content.GetChild(6).GetComponent<SpritesHolder>().sprites[2];
        content.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;

        content.GetChild(6).transform.GetChild(0).GetComponent<Button>().enabled = false;
    }

    // �±� ��������Ʈ �� ���� �ٲٱ�
    public void ChangeBottomSheet(int num)
    {
        // ��� ��
        if (num == 0)
        {
            Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
            Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
            bs_Tour.gameObject.SetActive(false);
        }

        // �������� ��
        if (num == 1)
        {
            Props_UI.instance.tags[1].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[0];
            Props_UI.instance.tags[0].sprite = Props_UI.instance.tags[0].transform.GetComponent<SpritesHolder>().sprites[1];
            bs_Tour.gameObject.SetActive(true);
        }
    }
}
