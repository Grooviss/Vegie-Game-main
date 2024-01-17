using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class VegetableCombineGame : MonoBehaviour
{
    public GameObject[] vegetables;
    public RawImage[] nextVegetableImages; // Assign these in the Unity Editor based on the number of prefabs
    public Text scoreText;
    private int[] vegetableScores = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110 };

    private GameObject currentVegetable;
    private GameObject nextVegetable;
    private int score;

    private float moveSpeed = 5f;
    private float borderX = 5f;
    private bool canMove = true;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        if (canMove)
        {
            MoveVegetable();

            // Check if the left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                // Drop the vegetable when the left mouse button is clicked
                DropVegetable();
            }
        }
    }

    void InitializeGame()
    {
        SpawnVegetable();
        SetNextVegetable();
        score = 0;
        UpdateScoreUI();
        canMove = true; // Set canMove to true to allow immediate movement
    }

    void SpawnVegetable()
    {
        currentVegetable = Instantiate(vegetables[Random.Range(0, 5)], new Vector2(0f, 8f), Quaternion.identity);

        // Disable gravity temporarily by setting gravityScale to 0
        Rigidbody2D rigidbody2D = currentVegetable.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 0f;
        }
    }

    void SetNextVegetable()
    {
        // Ensure nextVegetable index is limited to 0-4
        int nextVegetableIndex = Random.Range(0, 5);
        nextVegetable = vegetables[nextVegetableIndex];

        // Set the texture of the corresponding RawImage component
        if (nextVegetableImages != null && nextVegetableImages.Length > nextVegetableIndex)
        {
            RawImage currentNextVegetableImage = nextVegetableImages[nextVegetableIndex];

            if (currentNextVegetableImage != null)
            {
                // Assuming your prefab has a SpriteRenderer component
                SpriteRenderer nextVegetableSpriteRenderer = nextVegetable.GetComponent<SpriteRenderer>();

                if (nextVegetableSpriteRenderer != null)
                {
                    Texture2D nextVegetableTexture = nextVegetableSpriteRenderer.sprite.texture;
                    currentNextVegetableImage.texture = nextVegetableTexture;
                }
            }
        }
    }

    void UpdateScoreUI()
    {
        
    }

    void MoveVegetable()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 currentPosition = currentVegetable.transform.position;
        currentPosition.x += horizontalInput * moveSpeed * Time.deltaTime;

        // Border constraint
        currentPosition.x = Mathf.Clamp(currentPosition.x, -borderX, borderX);

        currentVegetable.transform.position = currentPosition;
    }

    void DropVegetable()
    {
        canMove = false;

        // Enable gravity before performing the raycast
        Rigidbody2D rigidbody2D = currentVegetable.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 1f;
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        // Invoke SpawnNextVegetable after 2 seconds
        Invoke("SpawnNextVegetable", 2f);
    }

    void SpawnNextVegetable()
    {
        // Spawn the next vegetable at the top middle
        currentVegetable = Instantiate(vegetables[Random.Range(0, 5)], new Vector2(0f, 8f), Quaternion.identity);

        // Disable gravity temporarily by setting gravityScale to 0
        Rigidbody2D rigidbody2D = currentVegetable.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        // Set canMove to true after a short delay to allow for physics to take effect
        Invoke("EnableMovement", 0.1f);

        SetNextVegetable();
    }

    void EnableMovement()
    {
        canMove = true;
    }

    public void CombineVegetables(GameObject droppedVegetable)
    {
        float combineDistance = 1.0f;  // Adjust this value based on your game's needs

        foreach (GameObject otherVegetable in vegetables)
        {
            if (otherVegetable != droppedVegetable && IsCloseEnough(droppedVegetable, otherVegetable, combineDistance))
            {
                // Combine logic
                CombineTwoVegetables(droppedVegetable, otherVegetable);

                // Remove the combined vegetables from the array
                vegetables = vegetables.Where(veggie => veggie != droppedVegetable && veggie != otherVegetable).ToArray();

                // Log the remaining vegetables for debugging
                Debug.Log("Remaining vegetables: " + string.Join(", ", vegetables.Select(veggie => veggie.name)));

                // Spawn new vegetable and set next vegetable
                SpawnVegetable();
                SetNextVegetable();
                canMove = true;

                return; // Exit the loop after combining the vegetables
            }
        }

        // If no matching vegetable is found, just reset canMove
        canMove = true;
    }

    bool IsCloseEnough(GameObject vegetable1, GameObject vegetable2, float combineDistance)
    {
        float distance = Vector2.Distance(vegetable1.transform.position, vegetable2.transform.position);
        return distance <= combineDistance;
    }

    void CombineTwoVegetables(GameObject vegetable1, GameObject vegetable2)
    {
        // Combine logic
        Vector2 newPosition = (vegetable1.transform.position + vegetable2.transform.position) / 2f;

        // Adjust the position to minimize the gap
        float gapAdjustment = 0.1f; // Tweak this value based on your preferences
        newPosition.y -= gapAdjustment;

        Instantiate(vegetables[Random.Range(0, 5)], newPosition, Quaternion.identity);

        // Destroy the original vegetables
        Destroy(vegetable1);
        Destroy(vegetable2);
    }
}