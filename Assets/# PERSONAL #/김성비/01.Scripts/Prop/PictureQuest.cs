using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureQuest : MonoBehaviour
{
    Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => TakingPicture());
    }

    void TakingPicture()
    {
        // ¸ðµ¨¸µ ²ô±â
        Props_UI.instance.propModeling.rotation = Quaternion.Euler(-5, -10, 0);
        Props_UI.instance.props[0].gameObject.SetActive(false);
        
        // UI ²ô±â
        Props_UI.instance.CanvasCamera.enabled = true;

        // 
    }
}
