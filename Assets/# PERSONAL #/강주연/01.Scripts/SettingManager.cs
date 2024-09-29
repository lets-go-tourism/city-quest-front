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
    [SerializeField] private AudioSource bgSource;

    [Header("LogOutUI_DeleteUI")]
    [SerializeField] private GameObject logOutBtn;
    [SerializeField] private GameObject logoutPopUp;
    [SerializeField] private GameObject deletePopUp;


    [Header("BackGroundAudio")]
    public AudioSource audioSource;
    private bool isSound;

    [Header("EffectSoundAudio")]
    public AudioSource effectSource;
    public AudioClip buttonTouchClip;
    public AudioClip popUpTouchClip;
    public AudioClip[] mapMoveTouch_ShortClip;
    public AudioClip[] mapMoveTouch_LongClip;
    private int shortClipCount;
    private int LongClipCount;

    private bool isEffectSound;

    private void Start()
    {
        isSound = true;
        isEffectSound = true;
        effectSoundBtn.GetComponent<Image>().sprite = onBtn;
        bgSoundBtn.GetComponent<Image>().sprite = onBtn;
        shortClipCount = 0;
        LongClipCount = 0;
    }

    public void LogOutButton()
    {
        DataManager.instance.SetLoginData(null);
        DataManager.instance.isLogout = true;
        if (File.Exists(DataManager.instance.GetPathData()))
        {
            File.Delete(DataManager.instance.GetPathData());
        }
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

    public void BackGroundSound_Original()
    {
        bgSource.volume = 0.8f;
    }

    public void BackGroundSound_InProp()
    {
        bgSource.volume = 0.6f;
    }

    public void BackGroundSound_InSetting()
    {
        bgSource.volume = 0.3f;
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

    public void EffectSound_ButtonTouch()
    {
        if (!isEffectSound)
            return;
         effectSource.PlayOneShot(buttonTouchClip);
         effectSource.volume = 1f;
    }

    public void EffectSound_PopUpTouch()
    {
        if (!isEffectSound)
            return;
        effectSource.PlayOneShot(popUpTouchClip);
        effectSource.volume = 1f;
    }

    public void EffectSound_MapShortTouch()
    {
        if (!isEffectSound)
            return;
        if (shortClipCount == mapMoveTouch_ShortClip.Length)
        {
            shortClipCount = 0;
        }
        effectSource.PlayOneShot(mapMoveTouch_ShortClip[shortClipCount]);
        effectSource.volume = 1f;
    }

    public void EffectSound_MapLongTouch()
    {
        if (!isEffectSound)
            return;
        if (LongClipCount == mapMoveTouch_LongClip.Length)
        {
            LongClipCount = 0;
        }
        effectSource.PlayOneShot(mapMoveTouch_LongClip[LongClipCount]);
        effectSource.volume = 1f;
    }
}
