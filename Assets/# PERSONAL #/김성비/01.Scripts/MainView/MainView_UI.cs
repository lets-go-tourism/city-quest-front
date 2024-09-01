using UnityEngine;
using UnityEngine.UI;

public class MainView_UI : MonoBehaviour
{
    /// <summary>
    /// ��ư ���
    /// - Settings : ����
    /// - Achieves : ���� 
    /// - Quests   : �������� ����Ʈ
    /// </summary>
    //[SerializeField]
    public Button[] buttons;

    private void Start()
    {
        buttons[0].onClick.AddListener(() => BTN_Settings());
        buttons[1].onClick.AddListener(() => BTN_Achieves());
        buttons[2].onClick.AddListener(() => BTN_Quests());
    }

    void BTN_Settings()
    {
        print("BTN_Settings");
    }

    void BTN_Achieves()
    {
        print("BTN_Achieves");
    }

    void BTN_Quests()
    {
        print("BTN_Quests");
    }
}
