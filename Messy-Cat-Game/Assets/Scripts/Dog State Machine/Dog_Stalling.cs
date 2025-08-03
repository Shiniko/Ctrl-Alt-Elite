using System.Collections;
using UnityEngine;

public class Dog_Stalling : StateMachineBehaviour
{
    DogContext _dogContext;
    private bool _stalling;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _dogContext = animator.GetComponent<DogContext>();
        _stalling = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_stalling == false)
        {
            _dogContext.StartCoroutine(Stall());
        }
    }

    IEnumerator Stall()
    {
        _stalling = true;
        yield return new WaitForSeconds(_dogContext.GetStallTime());
        _dogContext.GetComponent<Animator>().SetBool(DogContext.stallingHash, false);
    }
}
