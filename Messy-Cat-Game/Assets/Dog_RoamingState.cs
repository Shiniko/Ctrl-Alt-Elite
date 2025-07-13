using UnityEngine;
using UnityEngine.Assertions;

public class Dog_RoamingState : StateMachineBehaviour
{
    //References
    DogContext _dogContext;
    Transform _transform;
    Rigidbody _rigidbody;

    Vector3 _goTo;
    bool _goToSet = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _dogContext = animator.GetComponent<DogContext>();
        _transform = _dogContext.transform;
        _rigidbody = _dogContext.GetRigidbody();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_goToSet == false)
        {
            //Get a new location to roam to
            _goTo = _dogContext.GetNewRoamLocation();
            _goToSet = true;
        }
        //Time.deltaTime is not used here because it causes weird movement behavior
        _rigidbody.MovePosition(Vector3.MoveTowards(_transform.position, _goTo, _dogContext.GetSpeed() * Time.fixedDeltaTime));

        //If the dog has reached the destination, reset the goTo variable
        if(Vector3.Distance(_transform.position, _goTo) < 0.1f)
        {
            bool done = false;
            Timer.StartTimer(ref done,2);
            while(done == false)
            {

            }
            _goToSet = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
