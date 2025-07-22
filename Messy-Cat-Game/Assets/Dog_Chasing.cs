using UnityEngine;

public class Dog_Chasing : StateMachineBehaviour
{
    DogContext _dogContext;
    Rigidbody _rigidbody;
    Transform playerTransform;

    //Memory Variables
    Vector3 newPosition;
    Vector3 newLookAt;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _dogContext = animator.GetComponent<DogContext>();
        _rigidbody = _dogContext.GetRigidbody();
        playerTransform = _dogContext.player.transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        newPosition = Vector3.MoveTowards(_dogContext.transform.position, playerTransform.position, _dogContext.GetCanterSpeed() * Time.fixedDeltaTime);
        _rigidbody.MovePosition(newPosition);

        newLookAt = new Vector3(playerTransform.position.x, _dogContext.transform.position.y, playerTransform.position.z);
        _rigidbody.transform.LookAt(newLookAt);

        if (Vector3.Distance(_dogContext.transform.position, playerTransform.position) <= _dogContext.GetBarkingRange())
        {
            //If the dog is close enough to the player, bark
            animator.SetTrigger("Bark");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
