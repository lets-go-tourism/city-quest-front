using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLocationUI : MonoBehaviour
{
    private RectTransform m_RectTransform;

    private void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        Vector3 userPos = GPS.Instance.GetCurrentGPSPos();
        m_RectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(userPos);
    }
}
