using UnityEngine;

//Moves towards a suspicious event and investigates it
public class Dog_InvestigateState : StateMachineBehaviour
{
    // The suspicious Event the dog will investigate
    Vector3 _suspiciousEvent;

    Rigidbody _rigidbody;
    Transform _transform;
    DogContext _dogContext;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.transform;
        _dogContext = animator.GetComponent<DogContext>();
        _suspiciousEvent = _dogContext.currentSuspiciousEvent;
        _rigidbody = _dogContext.rb;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Time.deltaTime is not used here because it causes weird movement behavior
        _rigidbody.MovePosition(Vector3.MoveTowards(_transform.position, _suspiciousEvent, _dogContext.GetWalkSpeed() * Time.fixedDeltaTime));
        _rigidbody.transform.LookAt(new Vector3(_suspiciousEvent.x, _transform.position.y, _suspiciousEvent.z));
        //If the dog has reached the destination, reset the goTo variable
        if (Vector3.Distance(_transform.position, _suspiciousEvent) < 0.1f)
        {
            animator.SetBool(DogContext.distractedHash,true);
        }
    }
}