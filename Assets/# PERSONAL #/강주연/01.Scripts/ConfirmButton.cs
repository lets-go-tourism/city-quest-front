using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    public void ClickConfirm()
    {
        KJY_ConnectionTMP.instance.OnConnectionConfirm();
    }
}
