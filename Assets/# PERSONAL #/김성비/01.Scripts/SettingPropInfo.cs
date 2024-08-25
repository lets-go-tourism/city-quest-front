using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPropInfo : MonoBehaviour
{
    public static SettingPropInfo instance;

    private void Awake()
    {
        instance = this;
    }

    public void PropInfoSetting(Transform trs)
    {
        Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = trs.GetComponent<MeshRenderer>().material;   // �𵨸�
        Props_UI.instance.contents[0].GetChild(0).GetComponent<TextMeshProUGUI>().text = trs.name;                          // �̸�
        Props_UI.instance.contents[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = "o o o";                           // ���̵�
        // Props_UI.instance.contents[7].                                                                                   // ��� ����
        Props_UI.instance.contents[8].GetChild(0).GetComponent<TextMeshProUGUI>().text = "Address";                         // ��� ����
        //Props_UI.instance.contents[8].GetChild(1)                                                                         // ��� ��ũ

        // UNREVEAL
        if (trs.GetComponent<tmpPropReveal>().state == tmpPropReveal.State.UNREVEAL)
        {
            Props_UI.instance.contents[2].gameObject.SetActive(false);  // �湮����
            Props_UI.instance.contents[3].gameObject.SetActive(false);  // ����
            Props_UI.instance.contents[4].gameObject.SetActive(true);   // �������� �Ÿ�
            Props_UI.instance.contents[5].gameObject.SetActive(false);  // ���� ���� 1
            Props_UI.instance.contents[6].gameObject.SetActive(false);  // ���� ���� 2
            Props_UI.instance.contents[9].GetChild(0).GetComponent<TextMeshProUGUI>().text = "Quest";               // ���м�
            Props_UI.instance.contents[10].GetChild(1).GetComponent<TextMeshProUGUI>().text = "Take a Picture !";    // ����Ʈ
        }

        // ADVENTURING
        else
        {
            Props_UI.instance.contents[2].gameObject.SetActive(true);  // �湮����
            Props_UI.instance.contents[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = "2024.08.25";
            Props_UI.instance.contents[4].gameObject.SetActive(false);  // �������� �Ÿ�

            // ******************** ����Ʈ �޼����ο� ���� ���� 
            Props_UI.instance.contents[5].gameObject.SetActive(true);   // ���� ���� 1
            Props_UI.instance.contents[6].gameObject.SetActive(false);  // ���� ���� 2
            Props_UI.instance.contents[9].GetChild(0).GetComponent<TextMeshProUGUI>().text = "PlusQuest";
            Props_UI.instance.contents[10].GetChild(1).GetComponent<TextMeshProUGUI>().text = "Let's Look Arond !";

            // REVEAL
            if (trs.GetComponent<tmpPropReveal>().state == tmpPropReveal.State.REVEAL)
            {
                Props_UI.instance.contents[3].gameObject.SetActive(true);
                // Props_UI.instance.contents[3].GetComponent<>().          // � ��������

                //if ( �߰� ����Ʈ ��� �Ϸ� ������ )
                //{
                //    Props_UI.instance.contents[3].GetChild(0).GetComponent<Image>().color = Color.green;
                //}

                // �װ� �ƴ϶��
                Props_UI.instance.contents[3].GetChild(0).GetComponent<Image>().color = Color.red;
            }
        }

        Props_UI.instance.PropsUISetting(true);
    }
}
