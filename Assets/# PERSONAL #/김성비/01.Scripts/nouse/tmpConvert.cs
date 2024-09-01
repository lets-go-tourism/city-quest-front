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
        chars0 = "¼ö¿øÇà±Ã ¼º°û±æ".ToCharArray();
        chars1 = "È­¼ºÇà±Ã±¤Àå".ToCharArray();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
