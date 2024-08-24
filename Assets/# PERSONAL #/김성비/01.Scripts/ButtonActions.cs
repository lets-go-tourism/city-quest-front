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
}
