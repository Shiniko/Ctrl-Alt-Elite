using UnityEngine;

public class CatMenuDealer : MonoBehaviour
{
    [SerializeField] private MainMenuCat catMenu;

    public void SwitchReady()
    {
        if(catMenu != null)
        {
            catMenu.Switchready();
        }
    }
}
