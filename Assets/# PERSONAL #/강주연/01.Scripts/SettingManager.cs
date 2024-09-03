using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject bgSoundBtn;
    [SerializeField] private Button effectSoundBtn;
    [SerializeField] private GameObject logOutBtn;
    [SerializeField] private GameObject logoutPopUp;
    [SerializeField] private GameObject deletePopUp;

    [SerializeField] private Sprite onBtn;
    [SerializeField] private Sprite offBtn;
    [SerializeField] private Sprite logOutButtonClick;


    public AudioSource audioSource;

    private bool isSound;

    private void Start()
    {
        isSound = true;
        bgSoundBtn.GetComponent<Image>().sprite = onBtn;
    }

    public void LogOutButton()
    {
        DataManager.instance.SetLoginData(null);
        DataManager.instance.isLogout = true;
        if (File.Exists(DataManager.instance.GetPathData()))
        {
            File.Delete(DataManager.instance.GetPathData());
        }
        logOutBtn.GetComponent<Image>().sprite = logOutButtonClick;
        logoutPopUp.SetActive(true);
    }

    public void DeleteButton()
    {
        if (File.Exists(DataManager.instance.GetPathData()))
        {
            File.Delete(DataManager.instance.GetPathData());
        }
        KJY_ConnectionTMP.instance.OnConnectionDelete();
    }

    public void DeletePopUp()
    {
        DataManager.instance.isLogout = true;
        DataManager.instance.SetLoginData(null);
        deletePopUp.SetActive(true);
    }

    public void MoveFirstScene()
    {
        SceneManager.instance.ChangeScene(0);
    }

    public void BackGroundSoundOnOff()
    {
        if (isSound)
        {
            bgSoundBtn.GetComponent<Image>().sprite = offBtn;
            audioSource.Stop();
            isSound = false;
        }
        else
        {
            bgSoundBtn.GetComponent<Image>().sprite = onBtn;
            audioSource.Play();
            isSound = true;
        }
    }
}
