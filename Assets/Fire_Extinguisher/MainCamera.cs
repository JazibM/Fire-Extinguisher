using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	
	//private StartOption startOption;
	private TextMesh gameOverText;
	//private Player player;

	
	
	
	
	// Use this for initialization
	void Start () {
		//startOption = FindObjectOfType<StartOption>().GetComponent<StartOption>();
		gameOverText = GetComponentInChildren<TextMesh>();
		//player = FindObjectOfType<Player>().GetComponent<Player>();
		
		
	}
	
	public void resetGame(){
		//player.Reset();
		//startOption.gameStarted = false;
		//startOption.gameReset();	
		
	}
	
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
