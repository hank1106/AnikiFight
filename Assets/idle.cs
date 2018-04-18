using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SC;

public class idle : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		StatusCheck.AIgetHitType = 0;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StatusCheck.AIgetHitType = 0;
    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
   		StatusCheck.AIgetHitType = 0;
    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("On Attack IK ");
    }
}
