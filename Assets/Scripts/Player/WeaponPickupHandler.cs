using TMPro;
using UnityEngine;

public class WeaponPickupHandler : MonoBehaviour
{
    public LayerMask weaponPickupLayer;
    public float weaponPickupRadius = 2f;
    public WeaponSelector weaponSelector;
    public bool pickupInRadius = false;
    public WeaponPickupMeta weaponPickupMeta;
    public TextMeshProUGUI weaponPickupText;

    // Handle weapon switching input
    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && pickupInRadius)
        {
            SwitchWeapons();
        }
    }

    // Check for weapons nearby and update flags accordingly
    private void FixedUpdate() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, weaponPickupRadius, weaponPickupLayer);

        if (colliders.Length <= 0)
        {
            pickupInRadius = false;
            weaponPickupText.enabled = false;
            return;
        }

        Collider2D weaponCollider = colliders[0];

        // Display pop up text

        weaponPickupMeta = weaponCollider.GetComponent<WeaponPickupMeta>();

        pickupInRadius = true;
        weaponPickupText.enabled = true;
        weaponPickupText.text = $"<mark=#33333322>[E] Pickup <i>{weaponPickupMeta.WeaponName}</i></mark>";
    }

    // Switch weapons with the current equipped weapon
    private void SwitchWeapons()
    {
        Instantiate(weaponSelector.weaponPickupDrops[weaponSelector.currentWeaponId], transform.position, Quaternion.identity);
        Destroy(weaponPickupMeta.gameObject);

        weaponSelector.SelectWeapon(weaponPickupMeta.weaponId);
    }
}
