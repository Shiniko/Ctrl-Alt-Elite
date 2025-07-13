using UnityEngine;
using UnityEngine.Assertions;

public class Dog_RoamingState : StateMachineBehaviour
{
    DogContext _dogContext;
    Transform _transform;
    Rigidbody _rigidbody;
    bool _newTargetSet = false;
    Vector3 _targetPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _dogContext = animator.GetComponent<DogContext>();
        _transform = _dogContext.transform;
        _rigidbody = _dogContext.GetRigidbody();

        _targetPosition = new(Random.Range(_dogContext.GetMinRoamRange(), _dogContext.GetMaxRoamRange()), _transform.position.y, _transform.position.z);
        _newTargetSet = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Approximately(_transform.position.x, _targetPosition.x))
        {
            _newTargetSet = false;
        }

        if(_newTargetSet == false)
        {
            //Pick a random location to go to
            _targetPosition = new(Random.Range(_dogContext.GetMinRoamRange(), _dogContext.GetMaxRoamRange()), _transform.position.y, _transform.position.z);
        }
        //Time.deltaTime is not used here because it causes weird movement behavior
        Vector3 nextStep = Vector3.MoveTowards(_transform.position, _targetPosition, _dogContext.GetSpeed() * Time.fixedDeltaTime);
        //Move towards the new locations
        _rigidbody.MovePosition(nextStep);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
