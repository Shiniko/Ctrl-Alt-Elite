using UnityEngine;

public class Dog_RoamingState : StateMachineBehaviour
{
    //References
    DogContext _dogContext;
    Transform _transform;
    Rigidbody _rigidbody;
    Animator _animator;
    Vector3 _goTo;
    bool _goToSet;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _dogContext = animator.GetComponent<DogContext>();
        _transform = animator.transform;
        _rigidbody = _dogContext.rb;
        this._animator = animator;
        _goToSet = false;

        Ticker.OnTickAction += UpdateMovement;
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ticker.OnTickAction -= UpdateMovement;
    }

    public void UpdateMovement()
    {
        if (_goToSet == false)
        {
            _goToSet = true;
            //Get a new location to roam to
            _goTo = _dogContext.GetNewRoamLocation();

        }

        //Time.deltaTime is not used here because it causes weird movement behavior
        _rigidbody.MovePosition(Vector3.MoveTowards(_transform.position, _goTo, _dogContext.GetWalkSpeed() * Time.fixedDeltaTime));
        _rigidbody.transform.LookAt(new Vector3(_goTo.x, _transform.position.y, _goTo.z));
        //If the dog has reached the destination, reset the goTo variable
        if (Vector3.Distance(_transform.position, _goTo) < 0.1f)
        {
            _animator.SetBool(DogContext.stallingHash, true);
        }
    }
}
