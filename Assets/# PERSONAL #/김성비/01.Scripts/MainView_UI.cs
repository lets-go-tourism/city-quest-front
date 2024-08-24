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
