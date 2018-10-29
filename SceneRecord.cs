using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRecord : MonoBehaviour {


    //紀錄子彈

    //關卡開始時 手槍當前子彈
    public static int StartAmmo_HandGun ;
    //關卡開始時 衝鋒槍當前子彈
    public static int StartAmmo_SMG;
    //關卡開始時 所有備用子彈總量
    public static int StartAmmo_All;

    //關卡結束時 手槍當前子彈
    public static int EndAmmo_HandGun;
    //關卡結束時 衝鋒槍當前子彈
    public static int EndAmmo_SMG;
    //關卡結束時 所有備用子彈總量
    public static int EndAmmo_All;


    //記錄場景
    public static int Scene = 1 ;
    

	// Use this for initialization

	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {



        //從各個文件中抓取當前子彈之數量
        EndAmmo_SMG = SMGGlobalAmmo.SMGLoadedAmmo ;
        EndAmmo_HandGun = GlobalAmmo.HandGunLoadedAmmo ;
        EndAmmo_All = GlobalAmmo.CurrentAmmo ;


        //測試卡關自殺按鍵
        if (FPC_Health.health > 0 && PAUSE.pause == false)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                while (FPC_Health.health > 0)
                {
                    FPC_Health.Attacked();
                }
                
            }
        }

    }

    //通關之Method 當玩家通過一定地點會由其他物件呼叫此Method
    public static void NextLevel()
    {
        //將關卡結束時的數量放置於變數內 並在下個關卡開始時讀取數量
        StartAmmo_HandGun = EndAmmo_HandGun ;
        StartAmmo_SMG = EndAmmo_SMG ;
        StartAmmo_All = EndAmmo_All;

        Scene = Scene + 1 ;


    }
}
