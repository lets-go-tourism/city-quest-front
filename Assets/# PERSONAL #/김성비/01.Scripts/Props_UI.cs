using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Props_UI : MonoBehaviour
{
    // ����
    public Canvas canvasS;
    public Canvas canvasM;
    public Transform prop;
    public ScrollView scrollView;
    
    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }

    // UI On/Off ����
    public void PropsUISetting(bool isOpen)
    {
        // ĵ����
        canvasM.enabled = isOpen;
        canvasS.enabled = isOpen;
        // 3D �𵨸�
        prop.gameObject.SetActive(isOpen);
        
    }
}
