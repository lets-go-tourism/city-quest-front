using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIController : MonoBehaviour
{
    public static MapUIController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // 프랍들 하단에 보이는 UI 관리 스크립트
    public NameTagContainer NameTagContainer { get { return nameTagContainer; } private set { NameTagContainer = value; } }
    [SerializeField] private NameTagContainer nameTagContainer;

    // 플레이어의 현재 위치가 어느 방향인지 알려주는 스크립트
    public MyPosArrowUI MyPosArrowUI { get { return myPosArrowUI; } private set { myPosArrowUI = value; } }
    [SerializeField] private MyPosArrowUI myPosArrowUI;

    public delegate void UIActiveUpdate(float distance);
    public UIActiveUpdate uiActiveUpdateDelegate;

    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime;

        if (time < 0.2f)
            return;

        time = 0;
        float dist = (Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height + Screen.height / 5, Camera.main.transform.position.y)) - new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z)).sqrMagnitude;

        if (uiActiveUpdateDelegate != null)
            uiActiveUpdateDelegate.Invoke(dist);
    }
}
