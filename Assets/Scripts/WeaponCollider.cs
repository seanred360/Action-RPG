using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    Collider damageCollider;
    public List<Collider> hitEnemies = new List<Collider>();

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        //damageCollider.enabled = false;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        hitEnemies.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Attackable")
        {
            hitEnemies.Add(other);
            Debug.Log(gameObject.name + "hit" + other.name);
        }
    }
}
