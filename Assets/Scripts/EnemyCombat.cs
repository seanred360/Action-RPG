﻿using DG.Tweening;
using UnityEngine;

public class EnemyCombat : CharacterCombat
{
    public override void OpenWeaponColliderAnimationEvent()
    {
        currentWeapon.EnableDamageCollider();
    }

    public override void AttackHitAnimationEvent()
    {
        //Collider[] hitEnemies = Physics.OverlapSphere(myAttackPoint.position, attackRange, enemyLayers);
        //foreach (Collider enemy in hitEnemies)
        //{
        //    Debug.Log("we hit" + enemy.name);
        //    //DoDamage(enemy.GetComponent<CharacterStats>());
        //}

        foreach (Collider hitEnemy in currentWeapon.hitEnemies)
        {
            DoDamage(hitEnemy.GetComponent<CharacterStats>());
        }
        currentWeapon.hitEnemies.Clear();
        currentWeapon.DisableDamageCollider();
    }
}
