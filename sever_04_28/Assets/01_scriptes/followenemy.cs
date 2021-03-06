using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class followenemy : MonoBehaviour
{
       SpriteRenderer spriteRenderer;
  private bool isspawn=false;
    PlayerHP playerHP;
    Animator animator;

    [SerializeField]
    float agroRange;

    [SerializeField]
    float movespeed;
    int rnadomwait; 
    Rigidbody2D rb2d;
    private float curtime;
    GameObject playerpos;
    [SerializeField]
    private float damage=5;
    [SerializeField]
    private float enemyHP;
       [SerializeField]private AudioSource mysfx;
     [SerializeField]private AudioClip deathsound;
  [SerializeField]private AudioClip attack;
   [SerializeField]private AudioClip walksound;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHP=Random.Range(10,20);
       playerHP=GetComponent<PlayerHP>();
        playerpos = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
       rnadomwait= Random.Range(1,3);
       spawnenemy();
    }
private void spawnenemy(){
   animator.SetTrigger("spawn");
}
    // Update is called once per frame
    void Update()
    {
      //mysfx.PlayOneShot(walksound);
      if(PlayerHP.currentHP<0){
        Destroy(gameObject);
      }
        if(enemyHP<=0)
        {
            Destroy(gameObject);
        }
        curtime+=Time.deltaTime;
        if(curtime>1)
        {
       isspawn=true;
        }
        
        float distoplayer = Vector2.Distance(transform.position,playerpos.transform.position);
        if(distoplayer<agroRange && curtime>1)
        {
          StartCoroutine(ChasePlayer());
          StopCoroutine(SotpChase());
        }
        else
        {
        StopCoroutine(ChasePlayer());
         StartCoroutine(SotpChase());
        }     
    }
    IEnumerator ChasePlayer()
    {
     if(transform.position.x<playerpos.transform.position.x)
     {
      rb2d.velocity = new Vector3(movespeed,0);  
       //transform.eulerAngles= new Vector3(0,0,0);
      transform.eulerAngles = new Vector3(0,180,0);       
     }
     else
     {
          //transform.eulerAngles= new Vector3(0,180,0);
          rb2d.velocity = new Vector3(-movespeed,0);
          transform.eulerAngles = new Vector3(0,0,0);    
     }
     yield return null;
    }
    IEnumerator SotpChase()
    {
       rb2d.velocity = new Vector3(0,0,0);
       yield return null;
    }
    IEnumerator randomfollow()
    { if(transform.position.x<playerpos.transform.position.x)
     {
         transform.eulerAngles= new Vector3(0,0,0);
     }
         rb2d.velocity = new Vector2(rnadomwait,0);
        Vector3 vce = Vector3.right;
     yield return new WaitForSeconds(1);
      transform.eulerAngles= new Vector3(0,180,0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
      if(other.CompareTag("bullet"))
      {
      
         enemyHP-=playerBullet.bulletdamage;
           score sc = GameObject.Find("score").GetComponent<score>();
           sc.SetScore(sc.GetScore()+10);
      
      }
    }
   private void OnCollisionEnter2D(Collision2D collision)
    {
      if(isspawn)
      {
        if(collision.gameObject.CompareTag("bullet")){
          mysfx.PlayOneShot(deathsound);
                  StopCoroutine("HitColorAnimation");
        StartCoroutine("HitColorAnimation");  
        }
      if(collision.gameObject.CompareTag("Player"))
      {
                StopCoroutine("HitColorAnimation");
        StartCoroutine("HitColorAnimation");  
        mysfx.PlayOneShot(attack);
        PlayerHP.currentHP-=damage;
        //collision.GetComponent<PlayerHP>().HPdamager(damage);
      }
      }
    }
   public void enemydestroy()
    {
     Destroy(gameObject);
    }
    IEnumerator HitColorAnimation()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.color = Color.white;
    }
}
