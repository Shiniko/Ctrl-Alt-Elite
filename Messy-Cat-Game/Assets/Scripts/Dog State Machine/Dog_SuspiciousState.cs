using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//This script is for when the dog has seen the cat for a brief moment
//Gets triggered when the cat has entered the dog's field of view
public class Dog_SuspiciousState : StateMachineBehaviour
{
    DogVision dogVision;
    DogContext dogContext;

    private Image agroMeter;
    private GameObject agroMeterParent;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dogVision = animator.GetComponent<DogContext>().dogVision;
        dogContext = animator.GetComponent<DogContext>();

        agroMeter = dogContext.GetAgroMeter();
        agroMeterParent = dogContext.GetAgroMeterParent();
        agroMeterParent.SetActive(true);
        agroMeter.fillAmount = 0;

        FindAnyObjectByType<AudioDispatcher>().PlayClip("DogAlert");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject cat = dogContext.player.gameObject;

        if (cat != null)
        {
            if (dogVision.CanSee(dogContext.player.gameObject))
            {
                agroMeter.fillAmount += Time.fixedDeltaTime / dogContext.GetSeeCatTime();
            }
            else
            {
                agroMeter.fillAmount -= Time.fixedDeltaTime / dogContext.GetSeeCatTime();
            }
        }
        
        if(agroMeter.fillAmount <= 0)
        {
            animator.SetBool("Suspicious", false);
        }
        else if (Mathf.Approximately(agroMeter.fillAmount, 1))
        {
            animator.SetBool("Chasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agroMeterParent.SetActive(false);
        animator.SetBool("Suspicious", false);
    }
}
