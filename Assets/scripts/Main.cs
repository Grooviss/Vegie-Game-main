using UnityEngine;
using TMPro;

public class Main : MonoBehaviour
{
    public GameObject[] vegetables;
    private GameObject currentVegetable;
    private GameObject nextVegetablePrefab;
    private GameObject nextVegetable;
    public int[] veggiePoints;
    public int points;
    private float moveSpeed = 5f;
    private float borderX = 10f;
    public TMP_Text pointsText;
    float countdown = 5f;
    public static Main instance;
    public TMP_Text countdownText;
    private bool canMove = true;
    public GameObject holder;
    public Vector2 holderOffset = new Vector2(1f, 1f);
    public float poleYOffset = 0.5f;
    public GameObject pole;

    void Start()
    {
        SpawnVegetable();
    }

    void Update()
    {
        if (canMove)
        {
            MoveVegetable();

            if (Input.GetMouseButtonDown(0))
            {
                DropVegetable();
            }
        }
    }

    void SpawnVegetable()
    {
        int randomIndex = Random.Range(0, 5);
        currentVegetable = Instantiate(vegetables[randomIndex], new Vector2(0f, 12f), Quaternion.identity);
        Rigidbody2D rigidbody2D = currentVegetable.GetComponent<Rigidbody2D>();

        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 0f;
        }

        // Update nextVegetablePrefab
        nextVegetablePrefab = vegetables[Random.Range(0, 5)];

        // Spawn the next vegetable at the specified position (x 19, y 10)
        nextVegetable = Instantiate(nextVegetablePrefab, new Vector2(19f, 10f), Quaternion.identity);
        Rigidbody2D nextRigidbody2D = nextVegetable.GetComponent<Rigidbody2D>();

        if (nextRigidbody2D != null)
        {
            nextRigidbody2D.gravityScale = 0f;
            nextRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void MoveVegetable()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 currentPosition = currentVegetable.transform.position;
        currentPosition.x += horizontalInput * moveSpeed * Time.deltaTime;

        currentPosition.x = Mathf.Clamp(currentPosition.x, -borderX, borderX);
        currentVegetable.transform.position = currentPosition;
        // Move the pole along with the current vegetable
        MovePole(currentPosition);

        // Move the holder slightly to the top right of the current vegetable
        MoveHolder(currentPosition);
    }
    void MovePole(Vector2 position)
    {
        // Move the pole to the specified offset from the current vegetable
        pole.transform.position = new Vector2(position.x, position.y + poleYOffset);
    }
    void MoveHolder(Vector2 position)
    {
        // Move the holder to the specified offset from the current vegetable
        holder.transform.position = position + holderOffset;
    }


    void DropVegetable()
    {
        canMove = false;

        Rigidbody2D rigidbody2D = currentVegetable.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 1f;
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        Invoke("SpawnNextVegetable", 2f);
    }

    void SpawnNextVegetable()
    {
        Destroy(nextVegetable); // Destroy the current vegetable

        // Spawn the next vegetable at the specified position (x 19, y 10)
        currentVegetable = Instantiate(nextVegetablePrefab, new Vector2(0f, 12f), Quaternion.identity);
        Rigidbody2D rigidbody2D = currentVegetable.GetComponent<Rigidbody2D>();

        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        // Update nextVegetablePrefab
        nextVegetablePrefab = vegetables[Random.Range(0, 5)];

        // Destroy the previous next vegetable
        Destroy(nextVegetable);

        // Spawn the new next vegetable at the specified position (x 19, y 10)
        nextVegetable = Instantiate(nextVegetablePrefab, new Vector2(19f, 10f), Quaternion.identity);
        Rigidbody2D nextRigidbody2D = nextVegetable.GetComponent<Rigidbody2D>();

        if (nextRigidbody2D != null)
        {
            nextRigidbody2D.gravityScale = 0f;
            nextRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        Invoke("EnableMovement", 0.1f);
    }

    void EnableMovement()
    {
        canMove = true;
    }
}
