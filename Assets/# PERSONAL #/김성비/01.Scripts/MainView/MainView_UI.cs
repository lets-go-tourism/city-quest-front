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

    public ScrollRect placeScrollRect;
    public ScrollRect tourScrollRect;

    public Transform quitUI;

    public static MainView_UI instance;
    private void Awake()
    {
        instance = this;

        buttons[0].onClick.AddListener(() => BTN_Settings());
        buttons[1].onClick.AddListener(() => BTN_Achieves());
        buttons[2].onClick.AddListener(() => BTN_Quests());
        m_BackgroundDarkImage.enabled = false;
        quitUI.gameObject.SetActive(false);
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

    // ������
    public Image m_BackgroundDarkImage;

    public void BackgroundDarkEnable()
    {
        m_BackgroundDarkImage.enabled = true;
    }

    public void BackgroundDarkDisable()
    {
        m_BackgroundDarkImage.enabled = false;
    }
}
