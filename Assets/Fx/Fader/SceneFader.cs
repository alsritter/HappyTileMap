using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    private Animator anim;
    private int fadeId;

    void Start()
    {
        anim = GetComponent<Animator>();
        fadeId = Animator.StringToHash("Fade");
        //GameManager.RegisterSceneFader(this);
    }

    /// <summary>
    /// 激活死亡动画
    /// </summary>
    public void FadeOut()
    {
        anim.SetTrigger(fadeId);
    }
}
