using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform visual; 
    public Vector3 startPosition;
    private Vector2 moveInput;
    public string playerName = "Player";
    public int hp = 100;
    public float jumpPower = 8f;
    private Rigidbody2D rb;

public float fallLimit=-10f;
public int lives=3;
private bool isGameOver = false;
public Vector2 airScale = new Vector2(0.8f,1.2f);
private float facing=1f;

void Start()

{
    if(hp>=70)
    {
        Debug.Log("건강");
    }
    
        else if(hp>=30)
    {
        Debug.Log("주의");
    }
    else
    {
        Debug.Log("위험");
    }
    
    rb = GetComponent<Rigidbody2D>();
    transform.position = startPosition;
    Debug.Log(playerName + "시작.체력"+ hp);
    Debug.Log("피격 후 체력"+(hp - 30));
    Debug.Log("달리기 속도"+moveSpeed*2);
    Debug.Log("10/4="+10/4);
}

  void OnMove(InputValue value)
  {
    moveInput = value.Get<Vector2>();
    if (moveInput.x > 0)
        {
           facing = 1f;
        }
        else if(moveInput.x < 0)
        {
           facing = -1f;
        }
  }
 
void OnJump(InputValue value)
{
    if(value.isPressed&&isGrounded)
    {
        Debug.Log("점프");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
    }
}
  

  void Update()
  {
    //Debug.Log("Update");
    if(!isGameOver)
        {
            transform.Translate(Vector3.right * moveInput.x
             * moveSpeed * Time.deltaTime);
        }

    if(transform.position.y<fallLimit)
        {
            lives-=1;
            transform.position=startPosition;
            rb.linearVelocity=Vector2.zero;
            Debug.Log("낙사.남은 목숨"+lives);
            if(lives<=0)
            {
                isGameOver=true;
                Debug.Log("게임오버");
            }
        }
    if(isGrounded)
        {
            visual.localScale=new Vector3(facing,1f,1f);
        }
    else
        {
            visual.localScale=new Vector3(
                facing*airScale.x,airScale.y,1f);
            
        }
    }

private bool isGrounded = false;

void OnCollisionEnter2D(Collision2D Collision)
{
    if(Collision.gameObject.CompareTag("Ground"))
    {
        isGrounded=true;
        Debug.Log("착지");
    }
}

void OnCollisionExit2D(Collision2D Collision)
{
    if(Collision.gameObject.CompareTag("Ground"))
    {
        isGrounded=false;
        Debug.Log("공중");
    }
}



}
