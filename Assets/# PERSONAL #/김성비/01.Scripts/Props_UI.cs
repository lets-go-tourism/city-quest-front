using UnityEngine;
using UnityEngine.UI;

public class Props_UI : MonoBehaviour
{
    // 변수
    public Canvas canvasS;          // 뒷배경
    public Canvas canvasM;          // 메인 캔버스
    public Canvas CanvasCamera;     // 캔버스 카메라

    public Transform[] props;       // 지도 위 프랍들

    public Transform propModeling;  // 팝업창 프랍 모델링
    public RectTransform content;   // 팝업창 컨텐트
    public Transform[] contents;    // 팝업창 컨텐트 내부 사항

    public Image[] tags;          // 바텀시트 관광정보 탭


    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }

    // UI On/Off 설정
    public void PropsUISetting(bool isOpen)
    {
        // 캔버스
        canvasM.enabled = isOpen;
        canvasS.enabled = isOpen;
        // 3D 모델링
        propModeling.gameObject.SetActive(isOpen);

        content.anchoredPosition = Vector3.zero;
    }
}
