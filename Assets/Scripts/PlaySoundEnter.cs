using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] private SoundType soundType;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator == null || animator.gameObject == null)
        {
            Debug.LogWarning("Animator or GameObject is null in PlaySoundEnter.");
            return;
        }

        // Play the sound using SoundManager
        SoundManager.PlaySound(soundType);
    }
    
}