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
    public StatManager statManager;
    private GameObject popUp;
    private Text acc_per;
    private Text WPM;
    private Text title_text;
    private Text author_text;
    float currentTime = 0f;

    private int maxHealth = 0;
    private int currentHealth;
    private string remainingWord = string.Empty;
    private string currentWord = string.Empty;


    // Start is called before the first frame update
    private void Start()
    {
        PopUp();
        maxHealth = calculateHealth();
        SetMaxHealth(maxHealth);
        UnityEngine.Debug.Log(currentHealth);
        if (!photonView.IsMine)
            return;
        SetCurrentWord();

    }

    private void PopUp()
    {
        popUp = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        GameObject temp = popUp.transform.GetChild(0).gameObject;
        temp = temp.transform.GetChild(0).gameObject;
        acc_per = temp.GetComponent<Text>();

        temp = popUp.transform.GetChild(1).gameObject;
        temp = temp.transform.GetChild(0).gameObject;
        WPM = temp.GetComponent<Text>();

        temp = popUp.transform.GetChild(2).gameObject;
        temp = temp.transform.GetChild(0).gameObject;
        temp = temp.transform.GetChild(0).gameObject;
        title_text = temp.GetComponent<Text>();

        temp = popUp.transform.GetChild(2).gameObject;
        temp = temp.transform.GetChild(1).gameObject;
        temp = temp.transform.GetChild(0).gameObject;
        author_text = temp.GetComponent<Text>();

    }

    private void SetHealth(int health)
    {
        slider.value = health;
        currentHealth = health;
    }


    private int GetHealth()
    {
        return currentHealth;
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
        currentTime += 1 * Time.deltaTime;
        statManager.Set_Time(currentTime);
    }


    private void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            string keysPressed = Input.inputString;

            if (keysPressed.Length == 1)
            {
                EnterLetter(keysPressed);
            }
        }
    }

    private void EnterLetter(string typedLetter)
    {
        if (IsCorrectLetter(typedLetter))
        {
            statManager.Set_Total_Chars(statManager.Get_Total_Chars() + 1);
            RemoveLetter();

            if (IsWordComplete())
            {
                photonView.RPC("Attack", RpcTarget.AllBuffered);
                SetCurrentWord();
            }
        }
        else
        {
            statManager.Set_Wrong_Chars(statManager.Get_Wrong_Chars() + 1);
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
        int word_total = statManager.Get_Corr_Words();
        statManager.Set_Corr_Words(word_total +  1);
        return remainingWord.Length == 0;
    }

    private void CheckHealth()
    {
        if (currentHealth == 0)
        {
            photonView.RPC("Die", RpcTarget.AllBuffered);
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
            if (pView.GetComponent<Wizard>().GetHealth() == 0)
            {
                popUp.SetActive(true);
                UnityEngine.Debug.Log(statManager.Print_String());
            }
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

    [PunRPC]
    void Die()
    {
        popUp.SetActive(true);
        animator.SetTrigger("isDead");
        this.enabled = false;
        UnityEngine.Debug.Log(statManager.Print_String());
        acc_per.text = statManager.Print_String();
        WPM.text = statManager.Print_WPM();
        title_text.text = GetTitle();
        author_text.text = GetAuthor();
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
        if (!isFlipped)
        {
            transform.rotation = Quaternion.Euler(Input.GetAxis("Horizontal"), 0, 0);
            wordOutput.transform.rotation = Quaternion.Euler(Input.GetAxis("Horizontal"), 0, 0);
        }
        //photonView.GetComponent<SpriteRenderer>().flipX = isFlipped;
        else
        {
            transform.rotation = Quaternion.Euler(Input.GetAxis("Horizontal"), 180, 0);
            wordOutput.transform.rotation = Quaternion.Euler(Input.GetAxis("Horizontal"), 0, 0);

        }
       

    }

    [PunRPC]
    void Run(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    [PunRPC]
    void Idle(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }




}