using System.Collections;
using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public bool invulnerable = false;

    public Stat damage;
    public Stat armor;

    public event System.Action<int, int> OnHealthChanged;

    Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    TakeDamage(10);
        //}
    }

    public void TakeDamage(int damage)
    {
        if(!invulnerable)
        {
            //animator.SetTrigger("takeDamage");
            damage -= armor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentHealth -= damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            if (OnHealthChanged != null)
            {
                OnHealthChanged(maxHealth, currentHealth);
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        animator.SetTrigger("die");
    }
}
