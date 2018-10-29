using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActive : MonoBehaviour {

	//設定手電筒照射(觸發殭屍之射線距離)之距離 距離內照射到即觸發殭屍
	private float interactiveDistance = 16f ;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



		if (FlashLight.on == true) {

			Ray ray = new Ray (transform.position , transform.forward);
			RaycastHit hit ;
            
			
			if(Physics.Raycast(ray, out hit , interactiveDistance))

				//當射線長度碰撞到物體且碰撞之物體標籤為SceneZombie 
				if(hit.collider.CompareTag("SceneZombie")){

					//傳送變數true至被擊中之殭屍( public void Chase(bool   ) )
                    hit.transform.SendMessage("Chase",true);

                }

		}
		
	}
}
