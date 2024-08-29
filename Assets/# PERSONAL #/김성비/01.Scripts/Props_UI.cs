using UnityEngine;
using UnityEngine.UI;

public class Props_UI : MonoBehaviour
{
    // ����
    public Canvas canvasS;          // �޹��
    public Canvas canvasM;          // ���� ĵ����
    public Canvas CanvasCamera;     // ĵ���� ī�޶�

    public Transform[] props;       // ���� �� ������

    public Transform propModeling;  // �˾�â ���� �𵨸�
    public RectTransform content;   // �˾�â ����Ʈ
    public Transform[] contents;    // �˾�â ����Ʈ ���� ����

    public Image[] tags;          // ���ҽ�Ʈ �������� ��


    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }

    // UI On/Off ����
    public void PropsUISetting(bool isOpen)
    {
        // ĵ����
        canvasM.enabled = isOpen;
        canvasS.enabled = isOpen;
        // 3D �𵨸�
        propModeling.gameObject.SetActive(isOpen);

        content.anchoredPosition = Vector3.zero;
    }
}
