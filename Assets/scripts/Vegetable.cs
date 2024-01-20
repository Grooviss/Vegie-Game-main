using UnityEngine;

public class VegetableCollision : MonoBehaviour
{
    public GameObject[] vegetables;



    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(gameObject.tag))
        {
            // Get the index of the current vegetable in the array
            int currentIndex = GetVegetableIndex(gameObject.tag);

            // Calculate the next index in a circular manner
            int nextIndex = (currentIndex + 1) % vegetables.Length;

            // Get the next vegetable prefab from the array
            GameObject nextVegetablePrefab = vegetables[nextIndex];

            // Calculate the combined vegetable position
            Vector2 newPosition = (transform.position + col.transform.position) / 2f;

            // Instantiate the next vegetable at the combined position
            Instantiate(nextVegetablePrefab, newPosition, Quaternion.identity);

            // Set the flag to true to prevent further combinations until the next collision

            // Destroy the original vegetables
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }

    private int GetVegetableIndex(string tag)
    {
        // Find the index of the vegetable based on its tag
        for (int i = 0; i < vegetables.Length; i++)
        {
            if (vegetables[i].CompareTag(tag))
            {
                return i;
            }
        }

        // Return -1 if the tag is not found
        return -1;
    }
}