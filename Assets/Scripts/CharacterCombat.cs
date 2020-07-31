using UnityEngine.AI;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;
    const float combatCooldown = 5;
    float lastAttackTime;
    public LayerMask enemyLayers;
    protected float attackRange = 1f;

    public float attackDelay = .6f;

    public bool InCombat;
    public event System.Action OnAttack;

    protected CharacterStats myStats;
    protected CharacterStats opponentStats;

    public WeaponCollider currentWeapon;

    protected virtual void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        if(Time.time - lastAttackTime > combatCooldown)
        {
            //InCombat = false;
        }
    }

    public virtual void OpenWeaponColliderAnimationEvent()
    {
        EquipmentManager.instance.weaponCollider.EnableDamageCollider();
    }

    public virtual void AttackHitAnimationEvent()
    {
        currentWeapon = EquipmentManager.instance.weaponCollider;
        foreach (Collider hitEnemy in currentWeapon.hitEnemies)
        {
            DoDamage(hitEnemy.GetComponent<CharacterStats>());
        }
        currentWeapon.hitEnemies.Clear();
        currentWeapon.DisableDamageCollider();
    }

    public void DoDamage(CharacterStats targetStats)
    {
        opponentStats = targetStats;

        if (opponentStats != null)
        {
            if (attackCooldown <= 0f && myStats.currentHealth > 0 && opponentStats.currentHealth > 0)
            {
                attackCooldown = 1f / attackSpeed;
                InCombat = true;
                lastAttackTime = Time.time;

                opponentStats.TakeDamage(myStats.damage.GetValue());
                Vector3 direction = (opponentStats.transform.position - transform.position).normalized;
                Rigidbody rb = opponentStats.gameObject.GetComponent<Rigidbody>();
                //rb.DOMove(rb.position + direction * 2, .5f);

                opponentStats.gameObject.GetComponent<Rigidbody>().DOMove(opponentStats.transform.position + direction * 2, .5f);

                if (opponentStats.currentHealth <= 0)
                {
                    InCombat = false;
                }
            }
        }
    }
}
