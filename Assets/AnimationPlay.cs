using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlay : MonoBehaviour
{
    public Animation _animation;
    public string animationClipName;
    void Start()
    {
        _animation.Play(animationClipName);
    }

 
}
