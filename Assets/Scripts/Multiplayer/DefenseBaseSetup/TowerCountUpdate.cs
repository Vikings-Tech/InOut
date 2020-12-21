using UnityEngine;
using UnityEngine.UI;

public class TowerCountUpdate : MonoBehaviour
{
    public Text WallText, LaserText, MortarText;

    public void SetWallText(int max , int current)
    {
        int leftCount = max - current;
        if (leftCount > 0)
        {
            WallText.text = "Walls: \n" + leftCount + " Left";
            return;
        }
        WallText.text = "Walls \n Maxed Out";
    }

    public void SetLaserText(int max, int current)
    {
        int leftCount = max - current;
        if (leftCount > 0)
        {
            LaserText.text = "Laser Shooter: \n" + leftCount + " Left";
            return;
        }
        LaserText.text = "Laser Shooters \n Maxed Out";
    }

    public void SetMortarText(int max, int current)
    {
        int leftCount = max - current;
        if (leftCount > 0)
        {
            MortarText.text = "Mortar: \n" + leftCount + " Left";
            return;
        }
        MortarText.text = "Mortars \n Maxed Out";
    }
}
