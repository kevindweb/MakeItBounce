using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// used to start game over again
using UnityEngine.UI;

public class Restart : MonoBehaviour {
	public GameObject explosive;
	public float explosiveSpeed;
	public int numExplosives = 1;
	public int celebrationTime = 3;
	public float startTime;
	public Button backButton;
	public Button restartButton;
	public Button quitButton;
	public Font myFont;
	private GUIStyle myStyle;
	private GUIStyle highScoreStyle;
	private DataLoader access;
	private int highScore;
	private string scoreFile = "score.dat";
	private int prevScore;
	private string prevScoreFile = "prevScore.dat";
	private int currScore;
	private string currScoreFile = "currScore.dat";
	private bool isHighScore = false;
	private GameObject[] explosives;
	private Vector3[] forces;
	private Color[] randomColors;
	void Start(){
		randomColors = new Color[] {Color.red, Color.blue, Color.yellow, Color.black, Color.white, Color.green};
		myStyle = new GUIStyle();
		myStyle.font = myFont;
		myStyle.alignment = TextAnchor.MiddleCenter;
		highScoreStyle = new GUIStyle();
		highScoreStyle.font = myFont;
		highScoreStyle.alignment = TextAnchor.MiddleCenter;
		highScoreStyle.fontSize = 26;
		access = ScriptableObject.CreateInstance("DataLoader") as DataLoader;
		highScore = access.Load(0, scoreFile);
		prevScore = access.Load(0, prevScoreFile);
		currScore = access.Load(0, currScoreFile);
		if(highScore > prevScore)
			isHighScore = true;
		explosives = new GameObject[numExplosives];
		forces = new Vector3[numExplosives];
		if(isHighScore){
			startTime = Time.time;
			for(int i=0; i < numExplosives; i++){
				explosives[i] = Instantiate(explosive, new Vector3(0, 0, 13), Quaternion.identity);
				explosives[i].GetComponent<Renderer>().material.color = randomColors[Random.Range(0, randomColors.Length - 1)];
				forces[i] = RandomVector(-explosiveSpeed, explosiveSpeed);
			}
		}
	}
	private Vector3 RandomVector(float min, float max) {
         var x = Random.Range(min, max);
         var y = Random.Range(min, max);
         return new Vector3(x, y, 0f);
    }
	void Update(){
		if(isHighScore){
			if(Time.time - startTime < celebrationTime){
				for(int i=0; i < numExplosives; i++){
					Rigidbody2D rg2d = explosives[i].GetComponent<Rigidbody2D>();
					rg2d.velocity = forces[i];
				}
			} else{
				// destroy explosives
				for(int i=0; i < numExplosives; i++){
					Destroy(explosives[i]);
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			// press back button
			backButton.onClick.Invoke();
		} else if(Input.GetKeyDown(KeyCode.R)){
			// restart game
			restartButton.onClick.Invoke();
		} else if(Input.GetKeyDown(KeyCode.Q)){
			// restart game
			quitButton.onClick.Invoke();
		}
	}
	void OnGUI(){
		int h = 30;
		int w = 100;
		float height = (Screen.height-h) * .25f;
		float width = (Screen.width - w) * .5f;
		GUI.Label(new Rect(width, height - h, w, h), "Score: " + currScore, myStyle);
		GUI.Label(new Rect(width, height, w, h), "GAME OVER", myStyle);
		if(isHighScore)
			GUI.Label(new Rect(width, height + h, w, h), "HIGH SCORE", highScoreStyle);
	}
	public void RestartGame(){
		SceneManager.LoadScene(1);
	}
	public void ClickLoad(){
		SceneManager.LoadScene(0);
	}
	public void ClickQuit(){
		Application.Quit();
	}
}
