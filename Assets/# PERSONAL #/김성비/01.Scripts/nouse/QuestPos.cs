using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ��ǥ : ����Ʈ ��ư�� ���ҽ�Ʈ�� ������ �������� ��� ��ġ������ �� �ֵ���
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
