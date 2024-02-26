using UnityEngine;

public class Inventory : MonoBehaviour
{
    private void Update()
    {
        // Check for the "C" key press
        if (Input.GetKeyDown(KeyCode.C))
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        // Raycast to check for a nearby gun
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            // Check if the hit object has the "Gun" tag
            if (hit.collider.CompareTag("Gun(Clone)"))
            {
                PickupGun(hit.collider.gameObject);
            }
        }
    }

    private void PickupGun(GameObject gunObject)
    {
        // Add the gun to the inventory
        Inventorys.AddGun(gunObject);

        // Provide feedback to the player (you can customize this part)
        Debug.Log("Gun picked up!");

        // Remove the gun from the game world
        Destroy(gunObject);
    }
}

public class Inventorys : MonoBehaviour
{
    private static GameObject[] guns = new GameObject[10]; // Assuming a maximum of 10 guns in the inventory

    public static void AddGun(GameObject gun)
    {
        // Find an empty slot in the inventory and add the gun
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] == null)
            {
                guns[i] = gun;
                break;
            }
        }
    }
}
