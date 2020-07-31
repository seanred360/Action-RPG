using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventMessenger : MonoBehaviour
{
    [SerializeField]
    CharacterCombat combat;

    public void OpenWeaponColliderAnimationEvent()
    {
        combat.OpenWeaponColliderAnimationEvent();
    }

    public void AttackHitAnimationEvent()
    {
        combat.AttackHitAnimationEvent();
    }
}
