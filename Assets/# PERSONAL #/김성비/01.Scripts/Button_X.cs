using UnityEngine;
using UnityEngine.UI;

public class Button_X : MonoBehaviour
{
    Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public void Exit(int no)
    {
        // Ž��/��Ž�� ���� �˾�â ����
        if (no == 0)
        {
            Props_UI.instance.PropsUISetting(false);
            Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        }
    }
}
