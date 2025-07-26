using UnityEngine;

public class Dog_Chasing : StateMachineBehaviour
{
    DogContext _dogContext;
    Rigidbody _rigidbody;
    Transform _playerTransform;

    //Memory Variables
    Vector3 _newPosition;
    Vector3 _newLookAt;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _dogContext = animator.GetComponent<DogContext>();
        _rigidbody = _dogContext.rb;
        _playerTransform = _dogContext.player;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _newPosition = Vector3.MoveTowards(_dogContext.transform.position, new(_playerTransform.position.x, _playerTransform.position.y, _playerTransform.position.y - 1f), _dogContext.GetCanterSpeed() * Time.fixedDeltaTime);
        _rigidbody.MovePosition(_newPosition);

        _newLookAt = new Vector3(_playerTransform.position.x, _dogContext.transform.position.y, _playerTransform.position.z);
        _rigidbody.transform.LookAt(_newLookAt);

        if (Vector3.Distance(_dogContext.transform.position, _playerTransform.position) <= _dogContext.GetBarkingRange())
        {
            //If the dog is close enough to the player, bark
            animator.SetTrigger(DogContext.barkingHash);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
