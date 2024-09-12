using UnityEngine;
using UnityEngine.UI;

public class Props_UI : MonoBehaviour
{
    // º¯¼ö
    public Canvas canvasProp;       // [¹Ì]Å½Çè ÆË¾÷Ã¢
    public Canvas canvasTour;       // °ü±¤Á¤º¸ ÆË¾÷Ã¢

    //public Transform contentTour;   // °ü±¤ ÆË¾÷ ÄÁÅÙÆ®

    public Image[] tags;            // ¹ÙÅÒ½ÃÆ® °ü±¤Á¤º¸ ÅÇ

    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.M)) 
        {
            ContentReset();
        }
    }

    void ContentReset()
    {
        PopUpMovement.instance.rtTour.GetComponent<ScrollRect>().normalizedPosition = new Vector2(1, 1);
    }
}