using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject[] equipmentHolders;
    Equipment[] currentEquipment;
    GameObject[] currentMeshes;
    public Transform attackPoint;
    public WeaponCollider weaponCollider;

    public delegate void OnequipmentChanged(Equipment newItem, Equipment oldItem);
    public OnequipmentChanged onequipmentChanged;

    Inventory inventory;

    //for testing purposes delete later
    public Equipment defaultSword, defaultShield;
    public bool useDefaultEquipment;

    private void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[numSlots];
        currentMeshes = new GameObject[numSlots];

        if (useDefaultEquipment)
        {
            Equip(defaultSword);
            Equip(defaultShield);
        }
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = Unequip(slotIndex);

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        if (onequipmentChanged != null)
        {
            onequipmentChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;
        GameObject newMesh = Instantiate<GameObject>(newItem.equipmentObject, equipmentHolders[slotIndex].transform);

        currentMeshes[slotIndex] = newMesh;
        //attackPoint = currentMeshes[slotIndex].transform.Find("AttackPoint");
        if (slotIndex == 3) 
        {
            weaponCollider = newMesh.GetComponent<WeaponCollider>();
            //attackPoint = currentMeshes[3].transform.Find("AttackPoint");
            //Debug.Log(attackPoint.name + "Found");
        }
    }

    public Equipment Unequip(int slotIndex)
    {
        if(currentEquipment[slotIndex] != null)
        {
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onequipmentChanged != null)
            {
                onequipmentChanged.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }

    public void UnequipAll()
    {
        for(int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }

    private void Update()
    {
        //if (InputManager.instance.inputEnabled)
        //{
        //    //if (Input.GetKeyDown(KeyCode.U))
        //    //    UnequipAll();
        //}    
    }
}
