using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject currentWeapon;
    public List<GameObject> weapons;
    public List<GameObject> weaponPickupDrops;
    public int starterWeaponId = 0;
    public int currentWeaponId = 0;

    public Transform switchEffect;

    // Select the starter weapon
    private void Start() {
        SelectWeapon(starterWeaponId);
    }

    // Select a weapon and performs the actual switching of sprites and logic
    public void SelectWeapon(int weaponId)
    {
        if (weaponId > weapons.Count - 1) return;

        if (currentWeapon) Destroy(currentWeapon);

        currentWeapon = Instantiate(weapons[weaponId], transform);

        currentWeaponId = weaponId;

        Instantiate(switchEffect, transform.position, Quaternion.identity);
    }
}
