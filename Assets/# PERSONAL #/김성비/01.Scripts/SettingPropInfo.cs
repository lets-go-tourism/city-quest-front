using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class SettingPropInfo : MonoBehaviour
{
    public static SettingPropInfo instance;

    private void Awake()
    {
        instance = this;
    }

    public void PropInfoSetting(Transform trs)
    {
        // UNREVEAL
        if (trs.GetComponent<tmpPropReveal>().state == tmpPropReveal.State.UNREVEAL)
        {
            SettingNO(trs);
        }

        // ADVENTURING , REVEAL
        else
        {
            SettingYES(trs);
        }
    }

   public InfoHolder holderN;
   public InfoHolder holderY;

    void SettingNO(Transform tr)
    {
        // ������ ����
        Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = tr.GetComponent<MeshRenderer>().material;
        holderN.infos[1].GetComponent<TextMeshProUGUI>().text = tr.name;
        holderN.infos[2].GetComponent<TextMeshProUGUI>().text = "99.99" + "km";
        //holder.infos[3].GetComponent<Image>().sprite = ;
        holderN.infos[4].GetComponent<TextMeshProUGUI>().text = "��� 00�� 00�� 00�� 00";
        holderN.GoURL("ADRESS");
        //holder.infos[5].GetComponent<Image>().sprite = ;
        holderN.infos[6].GetComponent<TextMeshProUGUI>().text = "� ����Ʈ�ϱ��";

        // UI Ȱ��ȭ
        Props_UI.instance.PropsUISetting(true, 2);
    }

    void SettingYES(Transform tr)
    {
        // ������ ����
        Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = tr.GetComponent<MeshRenderer>().material;
        holderY.infos[1].GetComponent<TextMeshProUGUI>().text = tr.name;
        holderY.infos[2].GetComponent<TextMeshProUGUI>().text = "0�� 0�� �湮";
        //holderY.infos[3].GetComponent<Image>().sprite = ;
        //holderY.infos[4].GetComponent<Image>().sprite = ;
        holderY.infos[5].GetComponent<TextMeshProUGUI>().text = "��� 00�� 00�� 00�� 00";
        // UI Ȱ��ȭ
        Props_UI.instance.PropsUISetting(true, 1);
    }


    //Props_UI.instance.propModeling.GetComponent<MeshRenderer>().material = trs.GetComponent<MeshRenderer>().material;   // �𵨸�
    //Props_UI.instance.contents[0].GetChild(0).GetComponent<TextMeshProUGUI>().text = trs.name;                          // �̸�
    //Props_UI.instance.contents[1].GetChild(0).GetComponent<TextMeshProUGUI>().text = "o o o";                           // ���̵�
    //// Props_UI.instance.contents[7].                                                                                   // ��� ����
    //Props_UI.instance.contents[8].GetChild(0).GetComponent<TextMeshProUGUI>().text = "Address";                         // ��� ����
    ////Props_UI.instance.contents[8].GetChild(1)                                                                         // ��� ��ũ

    //Props_UI.instance.contents[2].gameObject.SetActive(false);  // �湮����
    //Props_UI.instance.contents[3].gameObject.SetActive(false);  // ����
    //Props_UI.instance.contents[4].gameObject.SetActive(true);   // �������� �Ÿ�
    //Props_UI.instance.contents[5].gameObject.SetActive(false);  // ���� ���� 1
    //Props_UI.instance.contents[6].gameObject.SetActive(false);  // ���� ���� 2
    //Props_UI.instance.contents[9].GetChild(0).GetComponent<TextMeshProUGUI>().text = "Quest";               // ���м�
    //Props_UI.instance.contents[10].GetChild(1).GetComponent<TextMeshProUGUI>().text = "Take a Picture !";    // ����Ʈ

    //Props_UI.instance.contents[2].gameObject.SetActive(true);  // �湮����
    //Props_UI.instance.contents[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = "2024.08.25";
    //Props_UI.instance.contents[4].gameObject.SetActive(false);  // �������� �Ÿ�

    //// ******************** ����Ʈ �޼����ο� ���� ���� 
    //Props_UI.instance.contents[5].gameObject.SetActive(true);   // ���� ���� 1
    //Props_UI.instance.contents[6].gameObject.SetActive(false);  // ���� ���� 2
    //Props_UI.instance.contents[9].GetChild(0).GetComponent<TextMeshProUGUI>().text = "PlusQuest";
    //Props_UI.instance.contents[10].GetChild(1).GetComponent<TextMeshProUGUI>().text = "Let's Look Arond !";

    // REVEAL
    //if (trs.GetComponent<tmpPropReveal>().state == tmpPropReveal.State.REVEAL)
    //{
    //    //Props_UI.instance.contents[3].gameObject.SetActive(true);
    //    //// Props_UI.instance.contents[3].GetComponent<>().          // � ��������

    //    ////if ( �߰� ����Ʈ ��� �Ϸ� ������ )
    //    ////{
    //    ////    Props_UI.instance.contents[3].GetChild(0).GetComponent<Image>().color = Color.green;
    //    ////}

    //    //// �װ� �ƴ϶��
    //    //Props_UI.instance.contents[3].GetChild(0).GetComponent<Image>().color = Color.red;

    //}
}
