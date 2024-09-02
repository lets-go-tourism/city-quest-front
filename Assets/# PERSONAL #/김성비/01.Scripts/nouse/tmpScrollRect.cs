using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class tmpScrollRect : MonoBehaviour
{
    public ScrollRect scrollRect;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            scrollRect.horizontalNormalizedPosition = 0;
        }
    }
}
