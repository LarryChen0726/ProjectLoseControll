using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SceneAllZombieSpawn : MonoBehaviour {

    public GameObject FPC;
    
    //隨機生成殭屍的種類
    int random ;
  
    //判定是否有碰撞到Scene1的殭屍動畫觸發區 第一關起初沒有殭屍 必須碰到劇情點才會引發殭屍出現
    private bool touch = false;
    
    //紀錄此重生點是否產出過殭屍 0為無 1為有 1個重生點只能產出一之殭屍
    float ZombieNumber = 0;

    //隨機變數 避免同時產出殭屍 導致音效完全重疊
    float RandomTime;

 
	// Use this for initialization
	void Start () {

		ZombieNumber = 0;

        touch = false;
        
        random = Random.Range(0,2);

        RandomTime = Random.Range(0f, 1f);

	}
	
	// Update is called once per frame
	void Update () {

       
        //避免同時產出殭屍 導致音效完全重疊
        if (ZombieNumber == 0)
        {
            RandomTime -= Time.deltaTime;
        }
        else
        {
            RandomTime = Random.Range(0f, 1f);
        }

        //判斷關卡是否在1或3關 若在第一關也必須觸碰到劇情動畫才會產出殭屍
		if (Scene1ZombieAnimationController.touch == true && SceneRecord.Scene == 1) {
			touch = true;
		} else if (SceneRecord.Scene == 3) {
			touch = true;
		}

        random = Random.Range(0, 2);

        //測重生點與玩家的距離
        float distance = Vector3.Distance(FPC.transform.position, this.gameObject.transform.position);

        
        if (touch == true)
        {
            //若玩家與重生點有一定距離 且此殭屍重生點並無產生過殭屍 
			if (distance > 35 && ZombieNumber == 0 && RandomTime < 0)
            {   

                //radnom用於產出不同殭屍類型
                if ((int)random == 0)
                {
                    //宣告GameObject為SceneAllZombie1(Spawn)之產出物件
                    GameObject ChildObject1 = Instantiate(GameObject.Find("SceneAllZombie1(Spawn)"),transform.position, transform.rotation);
                    //產出之物件為當前物件之子物件
                    ChildObject1.transform.parent = this.gameObject.transform;
                    //產生之角度隨機旋轉
                    ChildObject1.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                    //ChildObject1.transform.localPosition = new Vector3(0, 0, 0);
                    //ChildObject1.transform.localScale = new Vector3(1, 1, 1);
					ZombieNumber = ZombieNumber + 1;
                    
                }   
                else
                {
                    //宣告GameObject為SceneAllZombie1(Spawn)之產出物件
                    GameObject ChildObject2 = Instantiate(GameObject.Find("SceneAllZombie2(Spawn)"),transform.position, transform.rotation);
                    //產出之物件為當前物件之子物件
                    ChildObject2.transform.parent = this.gameObject.transform;
                    //產生之角度隨機旋轉
                    ChildObject2.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                    //ChildObject2.transform.localPosition = new Vector3(0, 0, 0);
                    //ChildObject2.transform.localScale = new Vector3(1, 1, 1);
                    //Instantiate(GameObject.Find("SceneAllZombie2"), transform.position, transform.rotation);
					ZombieNumber = ZombieNumber + 1;
                }
                
            }

          
        }

        
	}

    
    public void Decrease(int a)
    {
        StartCoroutine(wait());
    }

    //減少殭屍之數量 設定6秒後才減少 以避免殭屍瞬間重生
    IEnumerator wait()
    {
        yield return new WaitForSeconds(6f);
        ZombieNumber = ZombieNumber - 1;
    }

   
}
