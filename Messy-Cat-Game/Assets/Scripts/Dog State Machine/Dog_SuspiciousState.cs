using UnityEngine;
using UnityEngine.UI;
//This script is for when the dog has seen the cat for a brief moment
//Gets triggered when the cat has entered the dog's field of view
public class Dog_SuspiciousState : StateMachineBehaviour
{
    DogVision dogVision;
    DogContext dogContext;
    Vector3 suspiciousEvent;

    private Image agroMeter;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dogVision = animator.GetComponent<DogContext>().dogVision;
        dogContext = animator.GetComponent<DogContext>();

        suspiciousEvent = dogContext.currentSuspiciousEvent;
        agroMeter = dogContext.GetAgroMeter();
        agroMeter.fillAmount = 0;
        agroMeter.gameObject.SetActive(true);
        dogContext.transform.LookAt(suspiciousEvent);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dogVision.CanSee(dogContext.player.gameObject))
        {
            agroMeter.fillAmount += Time.fixedDeltaTime / dogContext.GetSeeCatTime();
        }
        else
        {
             agroMeter.fillAmount -= Time.fixedDeltaTime;
        }
        
        if(agroMeter.fillAmount <= 0)
        {
            agroMeter.gameObject.SetActive(false);
            animator.SetBool("Suspicious", false);
        }
        else if (Mathf.Approximately(agroMeter.fillAmount, agroMeter.fillAmount))
        {
            animator.SetBool("Suspicious", false);
            animator.SetBool("Chasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
