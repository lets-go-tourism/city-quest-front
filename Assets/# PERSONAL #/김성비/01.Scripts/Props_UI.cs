using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Props_UI : MonoBehaviour
{
    // º¯¼ö
    public Canvas canvasS;
    public Canvas canvasM;
    public Transform propModeling;
    public ScrollView scrollView;
    public Transform[] props;

    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }

    // UI On/Off ¼³Á¤
    public void PropsUISetting(bool isOpen)
    {
        // Äµ¹ö½º
        canvasM.enabled = isOpen;
        canvasS.enabled = isOpen;
        // 3D ¸ðµ¨¸µ
        propModeling.gameObject.SetActive(isOpen);
        
    }
}
