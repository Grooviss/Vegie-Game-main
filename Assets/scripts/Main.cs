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
    private float borderX = 11.5f;
    public TMP_Text pointsText;
    public static Main instance;
    private bool canMove = true;
    public GameObject holder;
    public Vector2 holderOffset = new Vector2(1f, 1f);
    public float poleYOffset = 0.5f;
    public GameObject pole;
    public float[] veggieSizes;
    public Transform objecttocheck;
    public Transform Line;
    private float vegetableTimer = 0f;
    private float maxVegetableTime = 5f;

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
            CheckVegetableTime();
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

       
        nextVegetablePrefab = vegetables[Random.Range(0, 5)];

       
        nextVegetable = Instantiate(nextVegetablePrefab, new Vector2(21f, 11f), Quaternion.identity);
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
        MovePole(currentPosition);

        
        MoveHolder(currentPosition);
    }
    void MovePole(Vector2 position)
    {
        
        pole.transform.position = new Vector2(position.x, position.y + poleYOffset);
    }
    void MoveHolder(Vector2 position)
    {
        
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
    void CheckVegetableTime()
    {
       
        if (currentVegetable != null)
        {
            
            if (currentVegetable.transform.position.y < 5f) 
            {
                vegetableTimer += Time.deltaTime;

                
                if (vegetableTimer >= maxVegetableTime)
                {
                   
                    EndGame();
                }
            }
            else
            {
               
                vegetableTimer = 0f;
            }
        }
    }

    void EndGame()
    {
        
        Debug.Log("Game Over");
        
    }

    


void SpawnNextVegetable()
    {
        Destroy(nextVegetable); 

        
        currentVegetable = Instantiate(nextVegetablePrefab, new Vector2(0f, 12f), Quaternion.identity);
        Rigidbody2D rigidbody2D = currentVegetable.GetComponent<Rigidbody2D>();

        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

       
        nextVegetablePrefab = vegetables[Random.Range(0, 5)];

        
        Destroy(nextVegetable);

       
        nextVegetable = Instantiate(nextVegetablePrefab, new Vector2(21f, 11f), Quaternion.identity);
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
