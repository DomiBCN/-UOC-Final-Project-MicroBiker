using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScript : MonoBehaviour {

    public Animator animator;
    
    // Use this for initialization
    void Start () {
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
	}
	
}
