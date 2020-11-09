using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    private Animator myAnimator;
	// Use this for initialization
	void Start () {
        myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        	
	}
    public void Open()
    {
        myAnimator.SetTrigger("Open"); //오픈 애니메이션
    }
}
