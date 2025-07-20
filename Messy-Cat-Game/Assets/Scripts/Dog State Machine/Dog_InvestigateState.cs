using UnityEngine;

//Moves towards a suspicious event and investigates it
public class Dog_InvestigateState : StateMachineBehaviour
{
    // The suspicious Event the dog will investigate
    SuspiciousEvent suspiciousEvent;

    Rigidbody _rigidbody;
    Transform _transform;
    DogContext _dogContext;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidbody = animator.GetComponent<Rigidbody>();
        _transform = animator.transform;
        _dogContext = animator.GetComponent<DogContext>();
        suspiciousEvent = _dogContext.currentSuspiciousEvent;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Time.deltaTime is not used here because it causes weird movement behavior
        _rigidbody.MovePosition(Vector3.MoveTowards(_transform.position, suspiciousEvent.origin, _dogContext.GetWalkSpeed() * Time.fixedDeltaTime));

        //If the dog has reached the destination, reset the goTo variable
        if (Vector3.Distance(_transform.position, suspiciousEvent.origin) < 0.1f)
        {
            animator.SetBool("Distracted",true);
            //Play sniffing animation
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}