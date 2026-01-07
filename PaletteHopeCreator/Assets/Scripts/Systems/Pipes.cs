using UnityEngine;

public class Pipes : MonoBehaviour
{
    public float speed = 5f;
    private float leftEdge;
    private Camera mainCamera;
    private GameManager gameManager;

    void Start()
    {
        mainCamera = Camera.main;
        leftEdge = mainCamera.ScreenToWorldPoint(Vector3.zero).x - 1f;
        
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.RegisterPipe(this);
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftEdge)
        {
            if (gameManager != null)
                gameManager.UnregisterPipe(this);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (gameManager != null)
            gameManager.UnregisterPipe(this);
    }
}
