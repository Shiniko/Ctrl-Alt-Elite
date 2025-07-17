using UnityEngine;
using UnityEngine.UI;

public class StarGrabber : MonoBehaviour
{
    public Image[] finishStarImages;                           
    public Image[] avoidStarImages;                         
    public Image[] hiddenStarImages;

    public bool starsPopulated;

    public int[] levelCompletes;
    public Button[] levelButtons;

    public bool levelsPopulated;

    void Start()
    {
        PopulateStars();
        PopulateLevels();
    }

    private void PopulateStars()
    {
        starsPopulated = true;
    }

    private void PopulateLevels()
    {
        starsPopulated = true;
    }

}
