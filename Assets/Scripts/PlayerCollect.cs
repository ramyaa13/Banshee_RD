using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Gem"))
        {
            CollectGem(other.gameObject);
        }
        else if (other.CompareTag("Crystal"))
        {
            CollectCrystal(other.gameObject);
        }
        else if (other.CompareTag("Star"))
        {
            CollectStar(other.gameObject);
        }
        else if (other.CompareTag("Loot"))
        {
            CollectLoot(other.gameObject);
        }
    }

    private void CollectGem(GameObject gem)
    {
        // Implement gem collection logic here
        Debug.Log("Collected Gem!");
        Destroy(gem); // You might want to disable or pool the object instead of destroying it
    }

    private void CollectCrystal(GameObject crystal)
    {
        // Implement crystal collection logic here
        Debug.Log("Collected Crystal!");
        Destroy(crystal);
    }

    private void CollectStar(GameObject star)
    {
        // Implement star collection logic here
        Debug.Log("Collected Star!");
        Destroy(star);
    }

    private void CollectLoot(GameObject loot)
    {
        // Implement loot collection logic here
        Debug.Log("Collected Loot!");
        Destroy(loot);
    }
}
