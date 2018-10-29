using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneAllZombie : MonoBehaviour {

    
    //自動導航追擊之目標
    public Transform target;

    //設定自動導航
    private UnityEngine.AI.NavMeshAgent agent;
    
    //判斷殭屍使否開始追擊
    bool go = false ;

    //控制殭屍追擊撥放音效之變數 
    bool run = false ;

    public GameObject FPC;

    // For Zombie Audio and Animation
    Animator animator;
    AudioSource audio;
    public AudioClip[] clip;
 

    //殭屍接觸到人類之時間 時間歸零則代表施展攻擊1次
    float healthtime = 1.65f;

    //殭屍消失之時間
    float TimeToDestroy = 5f;

 
    //判斷殭屍是否死亡
    bool IsDead = false;

    bool die = false;

    //殭屍之血量 
    public float health = 50f;

    
    // Use this for initialization
    void Start () {
        
        this.GetComponent<AudioSource>().enabled = true;

        audio = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        

    }
	
	// Update is called once per frame
	void Update () {

        //主角死亡後停止撥放所有殭屍之音效
        if(FPC_Health.health == 0)
        {
            this.GetComponent<AudioSource>().enabled = false ;
            audio.Stop();
        }

        //殭屍死亡
        if (health <= 0f && IsDead == false)
        {

            
            if (this.gameObject.name == "SceneAllZombie1(Spawn)(Clone)" || this.gameObject.name == "SceneAllZombie2(Spawn)(Clone)")
            {
                //殭屍為一定點方格所產生 當殭屍被擊殺 則尋找此殭屍物件之父物件 並傳送指令至 public void Decrease(int )中 減少殭屍總體數量
                //殭屍總體數量有最大上限 若超過一定數量則不再產生
                this.GetComponentInParent<SceneAllZombieSpawn>().SendMessage("Decrease", 1);
            }


            IsDead = true;
            audio.Stop();
            animator.SetBool("die", true);

            agent.isStopped = true;
            agent.ResetPath();

            go = false;
            die = true;

            this.GetComponent<CapsuleCollider>().enabled = false;  // This is Important

            audio.PlayOneShot(clip[2]);

            
        }

        //當殭屍死亡則開始倒數計時 時間歸零則摧毀殭屍物件
        if (die == true && TimeToDestroy > 0)
        {
            TimeToDestroy -= Time.deltaTime;
        }
        if (TimeToDestroy < 0)
        {
            Destroy(this.gameObject);
        }

        //當殭屍被觸發且血量大於0則開始追擊目標
        if (go == true && health > 0)
        {
            agent.SetDestination(target.position);
        }

        //當殭屍開始追擊時撥放觸發音效 run變數為控制音效只撥放一次
        if (go == true && run == false)
        {
            audio.PlayOneShot(clip[1]);
            animator.SetBool("run", true);
            run = true;
        }



        //當攻擊時間歸0則代表攻擊一次 而下次的殭屍攻擊時間為2.65秒(配合動畫之速度)
        if (healthtime < 0)
        {

            healthtime = 2.65f;

            FPC_Health.Attacked();

        }

    }

    
    void OnTriggerStay(Collider other)
    {
        //當殭屍碰到玩家 則轉變殭屍之動畫從跑至定點攻擊 
        if (other.gameObject == FPC && die == false)
        {

            animator.SetBool("attack", true);
            animator.SetBool("run", false);
            agent.isStopped = true;
            agent.ResetPath();

            //殭屍持續接觸玩家之秒數 若歸零則攻擊1次(配合動畫之速數)
            healthtime -= Time.deltaTime;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        //若玩家觸碰至殭屍則也會直接觸發殭屍追擊
        if(other.gameObject == FPC && die == false)
        {
            go = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //當玩家離開殭屍一定位置 則將施重新展開追擊 開啟自動導航 重製攻擊時間
        if (other.gameObject == FPC)
        {

            agent.SetDestination(target.position);
            animator.SetBool("attack", false);
            animator.SetBool("run", true);
            healthtime = 1.65f;


        }
    }

    //Zombie Health Decrease
    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    //For FlashLightTouch.cs  
    public void Chase(bool aaa)
    {
        go = aaa ;
    } 
}
