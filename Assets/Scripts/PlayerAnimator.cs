using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    public WeaponAnimations[] weaponAnimations;
    Dictionary<Equipment, AnimationClip[]> weaponAnimationsDict;

    public AnimationClip replaceableAttackAnim;
    public AnimationClip[] defaultAttackanimSet;
    protected AnimationClip[] currentAttackAnimSet;

    public AnimatorOverrideController overrideController;

    Vector3 lastPosition;

    protected override void Start()
    {
        base.Start();
        EquipmentManager.instance.onequipmentChanged += OnEquipmentChanged;

        weaponAnimationsDict = new Dictionary<Equipment, AnimationClip[]>();
        foreach(WeaponAnimations a in weaponAnimations)
        {
            weaponAnimationsDict.Add(a.weapon, a.clips);
        }

        if (overrideController == null)
        {
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        }

        animator.runtimeAnimatorController = overrideController;

        currentAttackAnimSet = defaultAttackanimSet;

    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null && newItem.equipSlot == EquipmentSlot.Weapon)
        {
            if(weaponAnimationsDict.ContainsKey(newItem))
            {
                currentAttackAnimSet = weaponAnimationsDict[newItem];
            }
        }
        else if (newItem == null && oldItem != null && oldItem.equipSlot == EquipmentSlot.Weapon)
        {
            currentAttackAnimSet = defaultAttackanimSet;
        }
    }

    protected override void Update()
    {
        speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 100f);
        lastPosition = transform.position;
        base.Update();
    }

    [System.Serializable]
    public struct WeaponAnimations
    {
        public Equipment weapon;
        public AnimationClip[] clips;
    }
}
