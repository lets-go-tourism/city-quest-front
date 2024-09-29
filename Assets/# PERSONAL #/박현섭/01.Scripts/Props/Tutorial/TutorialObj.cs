using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObj : MonoBehaviour
{
    public static TutorialObj instance;

    private void Awake()
    {
        instance = this;
    }

    public TutorialCloud Cloud { get; private set; }

    private void Start()
    {
        Cloud = transform.GetChild(0).GetComponent<TutorialCloud>();
    }
}
