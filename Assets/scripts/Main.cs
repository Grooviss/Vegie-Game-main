using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject[] vegetables;
    private GameObject currentVegetable;

    private float moveSpeed = 5f;
    private float borderX = 5f;
    private bool canMove = true;

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
        currentVegetable = Instantiate(vegetables[randomIndex], new Vector2(0f, 8f), Quaternion.identity);
        Rigidbody2D rigidbody2D = currentVegetable.GetComponent<Rigidbody2D>();

        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 0f;
        }
    }

    void MoveVegetable()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 currentPosition = currentVegetable.transform.position;
        currentPosition.x += horizontalInput * moveSpeed * Time.deltaTime;

        currentPosition.x = Mathf.Clamp(currentPosition.x, -borderX, borderX);
        currentVegetable.transform.position = currentPosition;
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
        currentVegetable = Instantiate(vegetables[Random.Range(0, 5)], new Vector2(0f, 8f), Quaternion.identity);
        Rigidbody2D rigidbody2D = currentVegetable.GetComponent<Rigidbody2D>();

        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        Invoke("EnableMovement", 0.1f);
    }

    void EnableMovement()
    {
        canMove = true;
    }
}
