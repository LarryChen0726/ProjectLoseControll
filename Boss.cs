using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	private UnityEngine.AI.NavMeshAgent agent ; //Unity自動導航

	public Transform target; //自動導航之目標

	private bool getmusic = false ;  

	private AudioSource audio ;

	public AudioClip[] random ;

    int index ;

    bool RandomGo = false ;

    public float health = 1000f; //設定Boss之血量

    public static bool IsDead = false ; //判斷Boss是否死亡

    public static bool die = false ;   //用於判斷Boss是否死亡並決定是否要繼續產生殭屍 false則不產生 true則產生

    Animator animator;

    bool hitboss = false; //判斷是否擊中Boss

    
    // Use this for initialization
    void Start () {

        IsDead = false;
        die = false;
        RandomGo = false;
        getmusic = false;

        animator = GetComponent<Animator>();

        //指定自動導航員為當前之物件
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        audio = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {

        //擊中Boss之後 Boss之行走速度變慢
        if (hitboss)
        {
            agent.speed = 2.5f;
        }
        else
        {   
            agent.speed = 5f;
        }
        

        //當Boss死亡之後
        if(health <= 0 && IsDead == false)
        {
            die = true ;
            IsDead = true;

            //消除Boss身上之實體Collider 以便於玩家通過Boss之屍體而不被卡住 
            this.GetComponent<CapsuleCollider>().enabled = false;

            //停止自動導航
            agent.isStopped = true;
            agent.ResetPath();

            //播放Boss死亡之音效
            audio.PlayOneShot(random[4]);

            animator.SetBool("Die",true);

        }

        //抓取test1物件之音樂是否撥放完畢 當為true時 則代表秒數結束
		getmusic = test1.gomusic;

        //秒數結束之後 則Boss開始自動導航追擊目標
		if (getmusic == true && IsDead == false) {
            
                agent.SetDestination(target.position);
        }

        //判斷Boss是否活著且音樂有無撥放並隨機撥放音樂
		if(getmusic == true && RandomGo == false && IsDead == false ){

            index = Random.Range(0, random.Length - 1);

            StartCoroutine(RandomAudio());      

			RandomGo = true;
  
        }

        hitboss = false;
	}



    //隨機撥放音樂
    IEnumerator RandomAudio() {   

        if (index == 2)
        {

            audio.PlayOneShot(random[index], 0.65f);

        }
        else { 

        audio.PlayOneShot(random[index]);

        }

        //等待音樂撥放完後5秒才變換變數RandoGo的狀態 並再次進行音樂隨機撥放
        yield return new WaitForSeconds(random[index].length + 5);


        RandomGo = false ;


	}

    //Boss被擊中
    public void TakeDamage(float amount)
    {

        health -= amount;
        hitboss = true;
    }

  




}

