using UnityEngine;
using UnityEngine.UI;

public class Props_UI : MonoBehaviour
{
    // ����
    public Canvas canvasProp;       // [��]Ž�� �˾�â
    public Canvas canvasTour;       // �������� �˾�â

    public Image[] tags;            // ���ҽ�Ʈ �������� ��

    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }
}
