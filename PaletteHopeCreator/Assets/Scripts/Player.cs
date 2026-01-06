using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] sprites;
    public float gravity = -9.8f;
    public float strength = 5f;

    private int spriteIdx;
    private Vector3 direction;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
        }

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    private void AnimateSprite()
    {
        spriteIdx++;

        if (spriteIdx >= sprites.Length)
            spriteIdx = 0;

        spriteRenderer.sprite = sprites[spriteIdx];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle")
            FindObjectOfType<GameManager>().GameOver();
        else if (other.gameObject.tag == "Scoring")
            FindObjectOfType<GameManager>().IncreaseScore();
    }
}
