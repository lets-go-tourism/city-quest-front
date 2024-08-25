using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public void DoAction(int no)
    {
        // Ž��/��Ž�� ���� �˾�â ����
        if (no == 0)
        {
            Props_UI.instance.PropsUISetting(false);
            Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);

            // ������ ��ġ�� �� �ֵ���!!
            for (int i = 0; i < Props_UI.instance.props.Length; i++)
                Props_UI.instance.props[i].GetComponent<BoxCollider>().enabled = true;
        }

        // ���� ������ ����
        else if (no == 1)
        {
            Props_UI.instance.PropsUISetting(false);
            Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
            Props_UI.instance.props[0].gameObject.SetActive(false);
            Props_UI.instance.CanvasCamera.enabled = true;
        }

        // ���� ��� ���ư���
        else if(no == 2)
        {
            Props_UI.instance.PropsUISetting(true);
            Props_UI.instance.props[0].gameObject.SetActive(true);
            Props_UI.instance.CanvasCamera.enabled = false;
        }
    }

    public void ChangeBottomSheet(bool open)
    {
        // true  : ������
        // false : �����
        Props_UI.instance.tour.gameObject.SetActive(open);

        // �������� �� �� �� : �������� �˾� ����
        if (open)
        {
            // �������� �� �� �� : �������� �˾� ����
        }

        // ������� �� �� �� : ��� �˾� ����
        if (!open)
        {
            // ������� �� �� �� : ��� �˾� ����
        }
    }
}
