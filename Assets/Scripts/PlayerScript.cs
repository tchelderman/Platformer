using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;

    public Text winText;

    public Text lose;

    public Text lives;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    private int scoreValue;

    private int livesValue;

    Animator anim;

    private bool facingRight = true;

    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        winText.text = "";
        lose.text = "";
        scoreValue = 0;
        livesValue = 3;
        SetScoreText ();
        SetLivesText ();
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
        else if (facingRight == true && hozMovement < 0)
            {
                Flip();
            }

        if (isJumping == false && vertMovement != 0)
            {
                anim.SetInteger("State", 2);
                isJumping = true;
            }
        else if (isJumping == true && vertMovement == 0)
            {
                anim.SetInteger("State", 0);
                isJumping = false;
            }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }
        
         if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
             anim.SetInteger("State", 0);
         }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if (scoreValue == 4)
        {
            transform.position = new Vector2(57, 1);
            livesValue = 3;
            SetLivesText ();
        }

        if (scoreValue >= 8)
        {
            winText.text = "You win!";
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            Destroy(this);
        }

        if (livesValue == 0)
        {
            lose.text = "You lose!";
            Destroy(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            SetScoreText ();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue = livesValue - 1;
            SetLivesText ();
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void SetLivesText ()
    {
        lives.text = "Lives:" + livesValue.ToString ();
    }

    void SetScoreText ()
    {
        score.text = "Score:" + scoreValue.ToString ();
    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}