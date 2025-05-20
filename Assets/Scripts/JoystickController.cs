using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public PlayerController player;
    public Joystick joystick;

    void Update()
    {
        if (player != null)
        {
            Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
            player.SetMoveInput(input);
        }
    }
}
