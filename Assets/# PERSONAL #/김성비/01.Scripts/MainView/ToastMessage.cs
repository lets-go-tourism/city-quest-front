using UnityEngine;
using UnityEngine.Android;
public class ToastMessage : MonoBehaviour
{
    public static void ShowToast(string text)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        curActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaObject toast = new AndroidJavaObject("android.widget.Toast", curActivity);
            toast.CallStatic<AndroidJavaObject>("makeText", curActivity, text, 2).Call("show");
        }));
    }
}