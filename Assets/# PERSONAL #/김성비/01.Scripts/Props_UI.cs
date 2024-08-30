using UnityEngine;
using UnityEngine.UI;

public class Props_UI : MonoBehaviour
{
    // ����
    public Canvas canvasPopUpNO;    // �˾� ��Ž��
    public Canvas canvasPopUpYES;   // �˾� Ž��
    public Canvas CanvasCamera;     // ĵ���� ī�޶�

    public Transform[] props;       // ���� �� ������
    public Transform propModeling;  // �˾�â ���� �𵨸�

    public Image[] tags;            // ���ҽ�Ʈ �������� ��

    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }

    // UI On/Off 
    // 0 : ���� ���� , 1 : Ž����� , 2 : ��Ž�����
    public void PropsUISetting(bool isOpen, int state)   
    {
        if (state == 1)
        {
            // ĵ���� Ȱ��ȭ
            canvasPopUpYES.enabled = isOpen;  // Ž����� �˾�â
            canvasPopUpNO.enabled = !isOpen;

            // ����
        }
        else if (state == 2)
        {
            // ĵ���� Ȱ��ȭ
            canvasPopUpNO.enabled = isOpen;   // ��Ž����� �˾�â
            canvasPopUpYES.enabled = !isOpen;

            // ����
        }
        else
        {
            canvasPopUpNO.enabled = isOpen;
            canvasPopUpYES.enabled = isOpen;
        }

        // 3D �𵨸�        
        propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        propModeling.gameObject.SetActive(isOpen);
    }
}
