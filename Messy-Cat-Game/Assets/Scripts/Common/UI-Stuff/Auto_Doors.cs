using UnityEngine;

public class Auto_Doors : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool open;
    private bool triggerOpen;

    private void Open()
    {
        if (!triggerOpen)
        {
            triggerOpen = true;
            open = true;

            if(anim != null)
            {
                anim.SetBool("isOpen", true);
            }
        }
    }

    private void Close()
    {
        triggerOpen = false;
        open = false;

        if (anim != null)
        {
            anim.SetBool("isOpen", false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (!open)
            {
                Open();
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (!open)
            {
                Open();
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (open)
            {
                Close();
            }
        }
    }
}
