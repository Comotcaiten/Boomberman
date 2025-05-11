using System.Collections;
using UnityEngine;
public class FlameItem : Item
{

    public int flameRange = 1;
    public float flameDuration = 5f; // Duration of the flame effect

    private BombController bomb;

    protected override IEnumerator Effect()
    {
        bomb = GameObject.Find("Player").GetComponent<BombController>();

        if (bomb != null)
        {
            bomb.bombRange += flameRange;
            yield return new WaitForSeconds(flameDuration);
            bomb.bombRange -= flameRange;
        }
        else
        {
            Debug.Log("BombController not found");
        }

    }
}
