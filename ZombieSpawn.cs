using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour {

    private float time ;  // ----test1.time

    private bool touch;   // ----- test1.touch


    //殭屍重生之秒數 歸0則重生
    float SpawnTime = 1f;

    //產生殭屍之類型
    private float ZombieType ;

    //判斷Boss是否死亡
    private bool BossDie = false;

    //紀錄殭屍之數量 此場景之最大殭屍數量為50
    public static int Number = 0 ; 

 	// Use this for initialization
	void Start() {

        // Renew the number
        Number = 0;
        ZombieType = Random.Range(0, 3);

    }
	
	// Update is called once per frame
	void Update () {

     

        BossDie = Boss.die;

        //test1為控制 第一次殭屍來襲之文件 此為記錄test1中背景之剩餘時間以及是否有觸發到劇情開關
        //殭屍之重生會配合背景音樂
        time = test1.time ;
        touch = test1.touch;

        //若接偶觸發到且音樂尚未結束
        if (touch == true && time > 0)
        {
            SpawnTime -= Time.deltaTime;
        }
        //或是當Boss死亡後
        else if(BossDie == true)
        {
            SpawnTime -= Time.deltaTime;
        }



        

        //當重生時間小於0且總數量小於50 則產生殭屍
        if(SpawnTime < 0 && Number < 50)
        {
            //記錄總殭屍數量
            Number = Number + 1;

            if ((int)ZombieType == 0)
            {
                Instantiate(GameObject.Find("Scene6 Zombie2"), transform.position, transform.rotation);
            }
           else if((int)ZombieType == 1)
            {
                Instantiate(GameObject.Find("Scene6 Zombie1"), transform.position, transform.rotation);
            }
            else
            {
                Instantiate(GameObject.Find("PoliceZombie"), transform.position, transform.rotation);
            }

            //不同階段殭屍重生時間長短不一樣
           if(BossDie == false)
            {
                SpawnTime = 7.5f;
            }
           else
            {
                SpawnTime = 5f;
            }
            
            ZombieType = Random.Range(0, 3);
            
        }
	}

    //若殭屍被擊殺 則會由其他物件去呼叫此Method 並減少殭屍總數量
    public static void DecreaseNumber()
    {
        Number = Number - 1;
    }

    
}
