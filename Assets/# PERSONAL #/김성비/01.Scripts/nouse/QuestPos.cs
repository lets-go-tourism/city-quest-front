using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 목표 : 퀘스트 버튼이 바텀시트와 일정한 간격으로 계속 위치해있을 수 있도록
public class QuestPos : MonoBehaviour
{
    public RectTransform bottomSheet;
    Vector3 distance;
    private void Start()
    {
        distance = GetComponent<RectTransform>().localPosition - bottomSheet.localPosition;
    }

    void Update()
    {
        transform.localPosition = bottomSheet.localPosition + distance;
    }
}
