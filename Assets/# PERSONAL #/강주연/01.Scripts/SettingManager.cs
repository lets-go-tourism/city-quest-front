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

    [Header("SoundUI")]
    [SerializeField] private GameObject bgSoundBtn;
    [SerializeField] private Button effectSoundBtn;
    [SerializeField] private Sprite onBtn;
    [SerializeField] private Sprite offBtn;

    [Header("LogOutUI_DeleteUI")]
    [SerializeField] private GameObject logOutBtn;
    [SerializeField] private GameObject logoutPopUp;
    [SerializeField] private GameObject deletePopUp;
    [SerializeField] private Sprite logOutButtonClick;


    [Header("BackGroundAudio")]
    public AudioSource audioSource;
    private bool isSound;

    [Header("EffectSoundAudio")]
    public AudioSource effectSource;
    public AudioClip[] effectClip;

    private bool isEffectSound;

    private void Start()
    {
        isSound = true;
        isEffectSound = true;
        effectSoundBtn.GetComponent<Image>().sprite = onBtn;
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
        DataManager.instance.SetLoginData(null);
        DataManager.instance.isLogout = true;
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

    public void EffectSoundOnOff()
    {
        if (isEffectSound)
        {
            effectSoundBtn.GetComponent<Image>().sprite = offBtn;
            effectSource.volume = 0;
            isEffectSound = false;
        }
        else
        {
            effectSoundBtn.GetComponent<Image>().sprite = onBtn;
            effectSource.volume = 0.5f;
            isEffectSound = true;
        }
    }

    public void EffectSoundManager(int num)
    {
        if (!isEffectSound)
            return;
        if (num == 0)
        {
            effectSource.PlayOneShot(effectClip[num]);
            effectSource.volume = 0.5f;
        }
        else if (num == 1)
        {
            
        }
        else if (num == 2)
        {

        }
    }
}
