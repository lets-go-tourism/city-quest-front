using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public void DoAction(int no)
    {
        // Å½Çè/¹ÌÅ½Çè Áö¿ª ÆË¾÷Ã¢ ²ô±â
        if (no == 0)
        {
            Props_UI.instance.PropsUISetting(false);
            Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        }

        // »çÁø ÂïÀ¸·¯ °¡±â
        else if (no == 1)
        {
            Props_UI.instance.PropsUISetting(false);
            Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
            Props_UI.instance.props[0].gameObject.SetActive(false);
            Props_UI.instance.CanvasCamera.enabled = true;
        }

        // »çÁø Âï°í µ¹¾Æ°¡±â
        else if(no == 2)
        {
            Props_UI.instance.PropsUISetting(true);
            Props_UI.instance.props[0].gameObject.SetActive(true);
            Props_UI.instance.CanvasCamera.enabled = false;
        }
    }
}
