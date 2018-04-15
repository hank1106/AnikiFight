using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurt : StateMachineBehaviour {

   public GameObject particle;
    public float radius;
    protected GameObject clone;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        clone = Instantiate(particle, animator.rootPosition, Quaternion.identity) as GameObject;
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(clone);
    }
}
