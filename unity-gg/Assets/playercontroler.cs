using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform visual;
    public Vector3 startPosition;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;

    public string playerName = "Player";
    public int hp = 100;
    public float jumpPower = 8f;
    public float fallLimit = -10f;
    public int lives = 3;

    private bool isGameOver = false;
    private bool isGrounded = false;

    public Vector2 airScale = new Vector2(0.95f, 1.05f);
    private float facing = 1f;

    public Image[] heartImages;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    public GameObject gameOverPanel;
    public string nextSceneName = "Stage2";

    void Start()
    {
        if (hp >= 70)
        {
            Debug.Log("건강");
        }
        else if (hp >= 30)
        {
            Debug.Log("주의");
        }
        else
        {
            Debug.Log("위험");
        }

        rb = GetComponent<Rigidbody2D>();

        if (visual != null)
        {
            animator = visual.GetComponent<Animator>();
        }

        transform.position = startPosition;

        Debug.Log(playerName + " 시작. 체력 " + hp);
        Debug.Log("피격 후 체력 " + (hp - 30));
        Debug.Log("달리기 속도 " + moveSpeed * 2);
        Debug.Log("10/4=" + 10 / 4);

        UpdateLivesUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.x > 0)
        {
            facing = 1f;
        }
        else if (moveInput.x < 0)
        {
            facing = -1f;
        }
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded && !isGameOver)
        {
            Debug.Log("점프");
            rb.linearVelocity =
                new Vector2(rb.linearVelocity.x, jumpPower);
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            transform.Translate(
                Vector3.right * moveInput.x *
                moveSpeed * Time.deltaTime
            );
        }

        if (transform.position.y < fallLimit && !isGameOver)
        {
            Loselife();
        }

        if (visual != null)
        {
            if (isGrounded)
            {
                visual.localScale = new Vector3(facing, 1f, 1f);
            }
            else
            {
                visual.localScale = new Vector3(
                    facing * airScale.x,
                    airScale.y,
                    1f
                );
            }
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("착지");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("공중");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver)
            return;

        if (other.CompareTag("Goal"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else if (other.CompareTag("trap"))
        {
            Loselife();
        }
    }

    void UpdateLivesUI()
    {
        if (heartImages == null)
            return;

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
            {
                heartImages[i].sprite =
                    (i < lives) ? fullHeartSprite : emptyHeartSprite;
            }
        }
    }

    void Loselife()
    {
        if (isGameOver)
            return;

        lives--;
        lives = Mathf.Max(0, lives);

        UpdateLivesUI();

        transform.position = startPosition;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        isGrounded = false;

        Debug.Log("피격 또는 낙사. 남은 목숨 " + lives);

        if (lives <= 0)
        {
            isGameOver = true;
            moveInput = Vector2.zero;

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }
    }
}