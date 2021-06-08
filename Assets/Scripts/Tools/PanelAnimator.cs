using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimator : MonoBehaviour {
    //动画
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Hide()
    {
        if (animator)
        {
            animator.SetTrigger("Hide");
        }
    }
    public void Close()
    {
        gameObject.SetActive(false); 
    }
}
