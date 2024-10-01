using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCloud : MonoBehaviour
{
    public Prop TargetProp { get; private set; }
    private MeshRenderer meshRenderer;
    private Animator animator;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        meshRenderer.enabled = false;
        enabled = false;
    }

    public void StartSetting(Prop target)
    {
        this.TargetProp = target;
        meshRenderer.enabled = true;
        animator.enabled = true;
        transform.position = TargetProp.PropObj.transform.TransformPoint(TargetProp.GetBoundsCenter());
        enabled = true;
    }

    private float time = 0;
    private float waitTime = 4;

    public float GetAnimTime()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time > waitTime)
        {
            time = 0;
            enabled = false;
        }
        if (TargetProp == null)
            return;
        transform.position = TargetProp.PropObj.transform.TransformPoint(TargetProp.GetBoundsCenter());
    }

    private void OnDisable()
    {
        TargetProp = null;
        meshRenderer.enabled = false;
        animator.enabled = false;
    }
}
