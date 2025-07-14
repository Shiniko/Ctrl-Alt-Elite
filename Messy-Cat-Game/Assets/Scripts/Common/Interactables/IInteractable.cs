using UnityEngine;
/// <summary>
/// How To Use : Inherit the interface and implement Interact(). 
/// Make sure to change the implementation but keep the method signature the same.
/// Add a trigger collider to the GameObject (I recommend including "[RequireComponent(typeof(Collider))] to ensure it doesn't get removed during gameplay")
/// </summary>
public interface IInteractable
{
    //Important Note: This interface is designed to be inherited by other classes.
    //This function wont get called unless it is written in the inheriting class.
    //Default implementation is here just as a simple example.
    //DONT CHANGE THIS SCRIPT, INSTEAD CREATE A NEW SCRIPT THAT INHERITS THIS INTERFACE AND OVERRIDE THE METHODS.

    /// <summary>
    /// Default interaction method that can be overridden by implementing classes.
    /// </summary>
    void Interact()
    {
        Debug.Log("Interacted with!");
    }
}
