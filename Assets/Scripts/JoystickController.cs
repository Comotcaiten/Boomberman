using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public PlayerController player;
    public Joystick joystick;
    public bool useJoystickOnly = false; // Nếu bạn muốn ép chỉ dùng joystick (mobile-only)

    void Update()
    {
        if (player != null)
        {
            Vector2 input;

            // #if UNITY_ANDROID || UNITY_IOS
            //             // Ưu tiên joystick trên mobile
            //             input = new Vector2(joystick.Horizontal, joystick.Vertical);
            // #else
            //             if (useJoystickOnly)
            //                 input = new Vector2(joystick.Horizontal, joystick.Vertical);
            //             else
            //                 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            // #endif

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                input = new Vector2(joystick.Horizontal, joystick.Vertical);
            }
            else
            {
                input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }


            player.SetMoveInput(input);
        }
    }
}
