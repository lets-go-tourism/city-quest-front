using UnityEngine;
using UnityEngine.UI;

public class Props_UI : MonoBehaviour
{
    // 변수
    public Canvas canvasPopUpNO;    // 팝업 미탐험
    public Canvas canvasPopUpYES;   // 팝업 탐험
    public Canvas CanvasCamera;     // 캔버스 카메라

    public Transform[] props;       // 지도 위 프랍들
    public Transform propModeling;  // 팝업창 프랍 모델링

    public Image[] tags;            // 바텀시트 관광정보 탭

    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }

    // UI On/Off 
    // 0 : 전부 끄기 , 1 : 탐험장소 , 2 : 미탐험장소
    public void PropsUISetting(bool isOpen, int state)   
    {
        if (state == 1)
        {
            // 캔버스 활성화
            canvasPopUpYES.enabled = isOpen;  // 탐험장소 팝업창
            canvasPopUpNO.enabled = !isOpen;

            // 세팅
        }
        else if (state == 2)
        {
            // 캔버스 활성화
            canvasPopUpNO.enabled = isOpen;   // 미탐험장소 팝업창
            canvasPopUpYES.enabled = !isOpen;

            // 세팅
        }
        else
        {
            canvasPopUpNO.enabled = isOpen;
            canvasPopUpYES.enabled = isOpen;
        }

        // 3D 모델링        
        propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        propModeling.gameObject.SetActive(isOpen);
    }
}
