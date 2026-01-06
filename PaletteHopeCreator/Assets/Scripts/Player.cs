using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Default Stats")]
    public Sprite[] sprites;
    public float gravity = -9.8f;
    public float strength = 5f;

    private int spriteIdx;
    private Vector3 direction;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    private CharacterData currentCharacterData;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        ApplyCharacterData();
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
        ApplyCharacterData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            direction = Vector3.up * GetCurrentStrength();

        direction.y += GetCurrentGravity() * Time.deltaTime * GetSlowMotionMultiplier();
        transform.position += direction * Time.deltaTime * GetSlowMotionMultiplier();
    }

    private void AnimateSprite()
    {
        spriteIdx++;

        Sprite[] currentSprites = GetCurrentSprites();
        if (currentSprites != null && currentSprites.Length > 0)
        {
            if (spriteIdx >= currentSprites.Length)
                spriteIdx = 0;
            spriteRenderer.sprite = currentSprites[spriteIdx];
        }
    }

    public void ApplyCharacterData()
    {
        CharacterInventory inventory = CharacterInventory.Instance;
        if (inventory != null)
        {
            currentCharacterData = inventory.GetSelectedCharacter();
        }

        if (currentCharacterData != null)
        {
            // 스프라이트 적용
            if (currentCharacterData.animationSprites != null && currentCharacterData.animationSprites.Length > 0)
            {
                // animationSprites 사용
            }
            else if (currentCharacterData.characterSprite != null)
            {
                spriteRenderer.sprite = currentCharacterData.characterSprite;
            }

            // 색상 적용
            spriteRenderer.color = currentCharacterData.characterColor;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    private float GetCurrentGravity()
    {
        return currentCharacterData != null ? currentCharacterData.gravity : gravity;
    }

    private float GetCurrentStrength()
    {
        return currentCharacterData != null ? currentCharacterData.strength : strength;
    }

    private Sprite[] GetCurrentSprites()
    {
        if (currentCharacterData != null && currentCharacterData.animationSprites != null && currentCharacterData.animationSprites.Length > 0)
            return currentCharacterData.animationSprites;
        return sprites;
    }

    private float GetSlowMotionMultiplier()
    {
        return currentCharacterData != null ? currentCharacterData.slowMotionMultiplier : 1.0f;
    }

    public float GetScoreMultiplier()
    {
        return currentCharacterData != null ? currentCharacterData.scoreMultiplier : 1.0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
            gameManager.GameOver();
        else if (other.CompareTag("Scoring"))
            gameManager.IncreaseScore();
    }
}
