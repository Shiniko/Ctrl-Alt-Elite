using System.Collections;
using UnityEngine;

public class Dog_Sniffing : StateMachineBehaviour
{
    DogContext dogContext;
    Animator animator;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dogContext = animator.GetComponent<DogContext>();
        this.animator = animator;

        dogContext.StartCoroutine(InvestigateTimeOut());
    }

    private IEnumerator InvestigateTimeOut()
    {
        yield return new WaitForSeconds(dogContext.GetInvestigationTime());
        animator.SetBool("Distracted", false);
    }
}
