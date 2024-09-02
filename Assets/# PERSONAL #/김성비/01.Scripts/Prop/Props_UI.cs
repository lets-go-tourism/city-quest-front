using UnityEngine;
using UnityEngine.UI;

public class Props_UI : MonoBehaviour
{
    // º¯¼ö
    public Canvas canvasProp;       // [¹Ì]Å½Çè ÆË¾÷Ã¢
    public Canvas canvasTour;       // °ü±¤Á¤º¸ ÆË¾÷Ã¢
    public Canvas CanvasCamera;     // Äµ¹ö½º Ä«¸Þ¶ó

    public Transform[] props;       // Áöµµ À§ ÇÁ¶øµé
    public Transform propModeling;  // ÆË¾÷Ã¢ ÇÁ¶ø ¸ðµ¨¸µ

    public Image[] tags;            // ¹ÙÅÒ½ÃÆ® °ü±¤Á¤º¸ ÅÇ

    public static Props_UI instance;
    void Awake()
    {
        instance = this;
    }
}
