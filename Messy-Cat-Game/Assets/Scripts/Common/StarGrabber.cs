using UnityEngine;
using UnityEngine.UI;

public class StarGrabber : MonoBehaviour
{
    public Image[] finishStarImages;                           
    public Image[] avoidStarImages;                         
    public Image[] hiddenStarImages;

    public bool starsPopulated;

    void Start()
    {
        PopulateStars();
    }

    private void PopulateStars()
    {
        starsPopulated = true;
    }

}
