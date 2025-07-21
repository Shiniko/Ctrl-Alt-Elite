using UnityEngine;
//This script is for when the dog has seen the cat for a brief moment
//Gets triggered when the cat has entered the dog's field of view
public class Dog_SuspiciousState : StateMachineBehaviour
{
    DogVision dogVision;
    DogContext dogContext;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dogVision = animator.GetComponent<DogContext>().dogVision;
        dogContext = animator.GetComponent<DogContext>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dogVision.suspicious = false;
    }
}
