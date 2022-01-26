using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [Header("Stats")]
    public int curHp;
    public int maxHp;
    public int moveSpeed;  
    public int damage;
    public float interactRange;
    public List<string> inventory = new List<string>();

    private Vector2 facingDirection;  

    [Header("Combat")]
    public KeyCode attackKey;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;

    [Header("Experience")]
    public int curLevel;
    public int curXp;
    public int xpToNextLevel;
    public float levelXpModifier;


    [Header("Sprites")]
    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    //components

    private Rigidbody2D rig;
    private SpriteRenderer sr;
    private ParticleSystem hitEffect;
    private PlayerUI ui;

    void Awake(){
        //Get the Components
        rig = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        hitEffect = gameObject.GetComponentInChildren<ParticleSystem>();
        ui = FindObjectOfType<PlayerUI>();
    }

    void Start(){
        //initialize the ui elements
        ui.UpdateHealtBar();
        ui.UpdateXpBar();
        ui.UpdateLevelText();
    }

    void Update(){
        Move();

        if(Input.GetKeyDown(attackKey)){
            if(Time.time - lastAttackTime >= attackRate)
                Attack();
        }
        CheckInteract();
    }

    void CheckInteract(){
         RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, interactRange, 1 << 9);

         if(hit.collider != null){
             Interactable interactable = hit.collider.GetComponent<Interactable>();
             ui.SetInteractText(hit.collider.transform.position, interactable.interactDescription);

             if(Input.GetKeyDown(attackKey))
                interactable.Interact();
         }
         else{
             ui.DisableInteractText();
         }
    }

    void Attack(){
        lastAttackTime = Time.time;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, attackRange, 1 << 8);

        if(hit.collider != null){
            hit.collider.GetComponent<Enemy>().TakeDamage(damage);

            //play the hit effect
            hitEffect.transform.position = hit.collider.transform.position;
            hitEffect.Play();
        }
    }

    void Move(){
        //get the horizontal and vertical keyboard inputs

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 vel = new Vector2(x, y);
        if(vel.magnitude != 0)
            facingDirection = vel;

        UpdateSpriteDirection();

        rig.velocity = vel * moveSpeed;
    }

    void UpdateSpriteDirection(){
        if(facingDirection == Vector2.up)
            sr.sprite = upSprite;
        else if(facingDirection == Vector2.down)
            sr.sprite = downSprite;
        else if(facingDirection  == Vector2.left)
            sr.sprite = leftSprite;
        else if(facingDirection == Vector2.right)
            sr.sprite = rightSprite;
    }

    public void TakeDamage(int damageTaken){
        curHp -= damageTaken;

        ui.UpdateHealtBar();

        if(curHp <= 0){
            Die();
        }
    }

    void Die(){
       UnityEngine.SceneManagement.SceneManager.LoadScene("Scene4");
    }

    public void AddXp(int xp){
        curXp += xp;

        ui.UpdateXpBar();

        if(curXp >= xpToNextLevel)
            LevelUp();
    }
    
    void LevelUp(){
        curXp -= xpToNextLevel;
        curLevel++;

        xpToNextLevel = Mathf.RoundToInt((float)xpToNextLevel * levelXpModifier);

        ui.UpdateLevelText();
        ui.UpdateXpBar();
    }

    public void AddItemToInventory(string item){

        inventory.Add(item);
        ui.UpdateInventoryText();
    }
}
