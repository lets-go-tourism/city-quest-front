using UnityEngine;
using UnityEngine.UI;

public class Props_UI : MonoBehaviour
{
    // ����
    public Canvas canvasProp;       // [��]Ž�� �˾�â
    public Canvas canvasTour;       // �������� �˾�â
    public Canvas CanvasCamera;     // ĵ���� ī�޶�

    public Transform[] props;       // ���� �� ������
    public Transform propModeling;  // �˾�â ���� �𵨸�

    public Image[] tags;            // ���ҽ�Ʈ �������� ��

    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }
}
