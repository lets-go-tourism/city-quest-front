using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpConvert : MonoBehaviour
{
    public char[] chars0;
    public char[] chars1;

    void Start()
    {
        // string -> char
        chars0 = "������� ������".ToCharArray();
        chars1 = "ȭ����ñ���".ToCharArray();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
