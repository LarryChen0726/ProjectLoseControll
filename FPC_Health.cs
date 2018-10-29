using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPC_Health : MonoBehaviour {


    //用於紀錄為第幾個關卡(Scene)之變數
    private int SceneNumber ;
    private string Scene;
    
    //設定主角之血量
	public static float health = 3 ;

    //血量顯示在銀幕上為一String型態 
    string h1 ;
    string h2 ;
    string h3 ;

    //Set Image width and height
    float width ;
    float height ;

    //死亡之畫面
    public Image Dead ;

    //判斷主角是否死亡並撥放音樂
    bool deadsound = false ;

    public CanvasGroup test ;

    //判斷死亡畫面是否執行完畢
    bool done = false ;
    
    //被攻擊後銀幕閃爍的畫面
	public Image blood ;                                                      

    //判斷是否被攻擊之變數 true為被攻擊
	public static bool attacked = false ;             
 
	public static Color flashColour = new Color(3f, 0f, 0f, 0.1f);    
	public static float flashSpeed = 5f;                                   

    //判斷是否撥放過心跳聲 
	bool heartbeat = false ;

    public GameObject boss ;

	private AudioSource audio ;

	public AudioClip[] clip ;

    //設定GUI之樣式 在Unity右邊Component編輯欄中
	public GUIStyle style ;
    public GUIStyle style1;

	// Use this for initialization
   

	void Start () {

        GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

        //Time.timeScale = 0 時 遊戲停止
        Time.timeScale = 1;
        health = 3;

        audio = GetComponent<AudioSource> ();

        //抓取紀錄關卡文件(SceneRecoed)的變數
        SceneNumber = SceneRecord.Scene;
        Scene = "Scene" + SceneNumber.ToString();


    }
	
	// Update is called once per frame
	void Update () {
        
        
        //抓取現行銀幕之寬及高
        width = Screen.width ;
        height = Screen.height ;

        //被攻擊後閃爍之畫面以及死亡畫面持續拉到滿屏
        blood.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        Dead.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        
        //被攻擊
        if (attacked && health > 0) {
			blood.color = flashColour;
            audio.PlayOneShot(clip[0]);
		}
		else{
            //將閃爍之畫面顏色漸漸消去
			blood.color = Color.Lerp (blood.color, Color.clear, flashSpeed * Time.deltaTime);
        }

		attacked = false;

        //判斷血量並顯示
        if (health == 3)
        {
            h1 = " ❤️ ";
            h2 = " ❤️ ";
            h3 = " ❤️ ";
        }
		if (health == 2) {
			h3 = "";
		}
		if(health == 1 && heartbeat == false){
			h2 = "" ;
            h3 = "";
            //若剩1格血 則撥放心跳聲
            HeartBeat();
		}
		if(health == 0){
			h1 = "";
            h2 = "";
            h3 = "";
            
        }

        //主角死亡
        if(health == 0 && deadsound == false)
        {
            
            //將控制主角之Script取消 使玩家無法繼續操作
            GameObject.Find("FPSController").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            audio.PlayOneShot(clip[2]);
            deadsound = true;
            StartCoroutine(ImageFadeIn(test, test.alpha, 1));
            
            
        }

        //當主角死亡且死亡畫面撥放完畢 案E重新開始
        if(health == 0 && done == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                SceneRestart();
                
            }
        }
        
		
	}

    //若被Boss碰撞後直接死亡
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == boss)
        {
            
            health = 0;
            audio.PlayOneShot(clip[0]);
           
        }
    }




	void OnGUI (){

        //若未勝利 則持續顯示血量在銀幕上
        if (FinalWin.win == false) {
            GUI.Label(new Rect((Screen.width / 10) * 4, (Screen.height / 10) * 9, (Screen.width / 10) * 4, (Screen.height / 10) * 9), h1 + " " + h2 + " " + h3, style);
        }

        //當死亡畫面漸層出現後之文提示(案E重新開始)
        if(done == true)
        {
            GUI.Label(new Rect((Screen.width / 10) * 4, (Screen.height / 10) * 9, (Screen.width / 10) * 4, (Screen.height / 10) * 9), "Press E To Restart", style1);
        }
	}

    //被攻擊
	public static void Attacked (){

        if (health > 0)
        {
            attacked = true;

            health = health - 1;
        }
        

	}

    //撥放心跳聲
	void HeartBeat(){

		heartbeat = true;
		audio.PlayOneShot (clip [1]);


	}

    //死亡畫面漸層
    IEnumerator ImageFadeIn(CanvasGroup cg , float start , float end , float lerpTime = 3f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            cg.alpha = currentValue;
            if (percentageComplete >= 1) break;
            yield return null;
        }

        done = true;
        Time.timeScale = 0;

    }


    //重新讀取關卡
    public void SceneRestart() {
        
        StartCoroutine(Restart());
        done = false;

    }

    //重新讀取關卡
    IEnumerator Restart()
    {
        
        AsyncOperation async = Application.LoadLevelAsync(Scene);
      
        yield return null;
       
    }







}
