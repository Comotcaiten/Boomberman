using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dpad : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    private void Start()
    {
        // Gắn sự kiện khi nhấn và nhả các nút
        AddEvent(upButton, () => Vertical = 1, () => Vertical = 0);
        AddEvent(downButton, () => Vertical = -1, () => Vertical = 0);
        AddEvent(leftButton, () => Horizontal = -1, () => Horizontal = 0);
        AddEvent(rightButton, () => Horizontal = 1, () => Horizontal = 0);
    }

    private void AddEvent(Button button, System.Action onPress, System.Action onRelease)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // Khi nhấn xuống
        EventTrigger.Entry pointerDown = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDown.callback.AddListener((eventData) => { onPress.Invoke(); });
        trigger.triggers.Add(pointerDown);

        // Khi nhả ra
        EventTrigger.Entry pointerUp = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUp.callback.AddListener((eventData) => { onRelease.Invoke(); });
        trigger.triggers.Add(pointerUp);

        // Khi rời khỏi vùng nút (tránh kẹt phím)
        EventTrigger.Entry pointerExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        pointerExit.callback.AddListener((eventData) => { onRelease.Invoke(); });
        trigger.triggers.Add(pointerExit);
    }
}
