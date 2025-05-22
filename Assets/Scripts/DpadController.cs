using UnityEngine;

public class DpadController : MonoBehaviour
{
    public PlayerController player;
    public Dpad dpad;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector2 input;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                input = new Vector2(dpad.Horizontal, dpad.Vertical);
            }
            else
            {
                input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
            player.SetMoveInput(input);
        }
    }
}

