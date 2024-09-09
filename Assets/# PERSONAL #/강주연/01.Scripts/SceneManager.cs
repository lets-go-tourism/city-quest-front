using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    public void Awake()
    {
        Application.targetFrameRate = 120;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }

    public void ChangeScene(int num)
    {
        // Load the scene by its name
        UnityEngine.SceneManagement.SceneManager.LoadScene(num);
        if (num == 1)
        {
            //KJY_ConnectionTMP.instance.OnClickHomeConnection();
        }
    }

    public void SceneLoad()
    {

    }
}
