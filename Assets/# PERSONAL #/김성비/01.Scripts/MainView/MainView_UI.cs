using TMPro;
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
    // ����ȭ�� ��ư : ����(, ����Ʈ, ����)
    //public Button[] buttons;

    // ���ҽ�Ʈ
    public RectTransform bottomSheet;

    // ���ҽ�Ʈ  ��ũ�Ѻ�
    public ScrollRect categoryScrollRect;

    // ���ҽ�Ʈ ī�� ��ũ�Ѻ�
    public ScrollRect placeScrollRect;
    public ScrollRect tourScrollRect;

    // ���ҽ�Ʈ �ȳ� UI
    public TextMeshProUGUI BSGuideUI;

    // ���ҽ�Ʈ ���̷���
    public Transform skeleton;
    public Image[] skeletons;
    
    // ���� Ȯ�� UI
    //public Transform quitUI;

    public static MainView_UI instance;
    private void Awake()
    {
        instance = this;

        //buttons[0].onClick.AddListener(() => BTN_Settings());
        //buttons[1].onClick.AddListener(() => BTN_Achieves());
        //buttons[2].onClick.AddListener(() => BTN_Quests());
        m_BackgroundDarkImage.enabled = false;
        //quitUI.gameObject.SetActive(false);

        categoryScrollRect.normalizedPosition = Vector2.zero;
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
