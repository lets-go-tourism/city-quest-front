using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainView_UI : MonoBehaviour
{
    /// <summary>
    /// 버튼 기능
    /// - Settings : 세팅
    /// - Achieves : 업적 
    /// - Quests   : 진행중인 퀘스트
    /// </summary>
    //[SerializeField]
    // 메인화면 버튼 : 설정(, 퀘스트, 업적)
    //public Button[] buttons;

    // 바텀시트
    public RectTransform bottomSheet;

    // 바텀시트  스크롤뷰
    public ScrollRect categoryScrollRect;

    // 바텀시트 카드 스크롤뷰
    public ScrollRect placeScrollRect;
    public ScrollRect tourScrollRect;

    // 바텀시트 안내 UI
    public TextMeshProUGUI BSGuideUI;

    // 바텀시트 스켈레톤
    public Transform skeleton;
    public Image[] skeletons;
    
    // 종료 확인 UI
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

    // 박현섭
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
