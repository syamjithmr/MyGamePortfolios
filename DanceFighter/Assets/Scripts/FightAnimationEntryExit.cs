using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightAnimationEntryExit : StateMachineBehaviour
{
    public string parameter;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var item in animator.parameters)
        {
            if (item.name.Contains("Dance") && item.type==AnimatorControllerParameterType.Bool)
            {
                if (animator.GetBool(item.name))
                    animator.SetBool(item.name, false);
            }
        }
        animator.GetComponent<EthanController>().IsDancing = false;
        animator.GetComponent<EthanController>().IsFighting = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(parameter, false);
        animator.GetComponent<EthanController>().IsFighting = false;
        animator.GetComponent<Rigidbody>().constraints ^= RigidbodyConstraints.FreezePositionY;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
