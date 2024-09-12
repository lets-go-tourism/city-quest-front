using UnityEngine;
using UnityEngine.UI;

public class Props_UI : MonoBehaviour
{
    // ����
    public Canvas canvasProp;       // [��]Ž�� �˾�â
    public Canvas canvasTour;       // �������� �˾�â

    //public Transform contentTour;   // ���� �˾� ����Ʈ

    public Image[] tags;            // ���ҽ�Ʈ �������� ��

    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.M)) 
        {
            ContentReset();
        }
    }

    void ContentReset()
    {
        PopUpMovement.instance.rtTour.GetComponent<ScrollRect>().normalizedPosition = new Vector2(1, 1);
    }
}