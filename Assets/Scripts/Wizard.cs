using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Wizard : WordBank
{
    public Text wordOutput = null;
    public Slider slider;
    public Animator animator;
    public Transform SpellPoint;
    public float spellRange = 1f;
    public float moveSpeed = 75f;
    public LayerMask enemyLayers;
    
    private int maxHealth = 0;
    private int currentHealth;
    private string remainingWord = string.Empty;
    private string currentWord = string.Empty;

    // Start is called before the first frame update
    private void Start()
    {
        maxHealth = calculateHealth();
        SetMaxHealth(maxHealth);
        if (!photonView.IsMine)
            return;
        SetCurrentWord();

    }

    private void SetHealth(int health)
    {
        slider.value = health;
        currentHealth = health;
    }

    private void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        currentHealth = health;
    }

    private void SetCurrentWord()
    {
        currentWord = GetWord();
        photonView.RPC("SetRemainingWord", RpcTarget.AllBuffered, currentWord);

    }

    [PunRPC]
    private void SetRemainingWord(string newString)
    {
        remainingWord = newString;
        wordOutput.text = remainingWord;
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerControls();
    }


    private void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            string keysPressed = Input.inputString;

            if(keysPressed.Length == 1)
            {
                EnterLetter(keysPressed);
            }
        }
    }

    private void EnterLetter(string typedLetter)
    {
        if (IsCorrectLetter(typedLetter))
        {
            RemoveLetter();

            if (IsWordComplete())
            {
                photonView.RPC("Attack", RpcTarget.AllBuffered);
                SetCurrentWord();
            }
        }
    }

    private bool IsCorrectLetter(string letter)
    {
        return remainingWord.IndexOf(letter) == 0;
    }

    private void RemoveLetter()
    {
        string newString = remainingWord.Remove(0, 1);
        photonView.RPC("SetRemainingWord", RpcTarget.AllBuffered, newString);
    }

    private bool IsWordComplete()
    {
        return remainingWord.Length == 0;
    }

    private void CheckHealth()
    {
        if (currentHealth == 0)
        {
            Die();
        }
    }

    [PunRPC]
    void Attack()
    {
        if (photonView.IsMine)
            animator.SetTrigger("Casted");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(SpellPoint.position, spellRange, enemyLayers);
        PhotonView pView;
        foreach (Collider2D enemy in hitEnemies)
        {
            UnityEngine.Debug.Log("Hit " + enemy.name); ;
            pView = enemy.GetComponent<PhotonView>();
            pView.GetComponent<Wizard>().TakeDamage();
        }

    }

    void OnDrawGizmosSelected()
    {
        if (SpellPoint == null)
            return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(SpellPoint.position, spellRange);
    }


    private void TakeDamage()
    {
        animator.SetTrigger("isDamaged");
        currentHealth -= 1;
        SetHealth(currentHealth);
        CheckHealth();

    }

    void Die()
    {
        animator.SetTrigger("isDead");
        this.enabled = false;
    }

    private void PlayerControls()
    {
        if (photonView.IsMine)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
            transform.position += movement * Time.deltaTime * moveSpeed;
            if (Input.GetKey("left"))
            {
                bool isFlipped = false;
                if (photonView.gameObject.name == "Red_Wizard(Clone)")
                    isFlipped = true;
                photonView.RPC("Run", RpcTarget.AllBuffered, true);
                photonView.RPC("FlipSprite", RpcTarget.AllBuffered, isFlipped);
            }
            else if (Input.GetKey("right"))
            {
                bool isFlipped = true;
                if (photonView.gameObject.name == "Red_Wizard(Clone)")
                    isFlipped = false;
                photonView.RPC("Run", RpcTarget.AllBuffered, true);
                photonView.RPC("FlipSprite", RpcTarget.AllBuffered, isFlipped);
            }
            else
            {
                photonView.RPC("Idle", RpcTarget.AllBuffered, false);
            }

            //FlipSprite();
            CheckInput();
        }
    }

    [PunRPC]
    void FlipSprite(bool isFlipped)
    {
        photonView.GetComponent<SpriteRenderer>().flipX = isFlipped;
       
    }

    [PunRPC]
    void Run(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    [PunRPC]
    void Idle(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning );
    }

}
