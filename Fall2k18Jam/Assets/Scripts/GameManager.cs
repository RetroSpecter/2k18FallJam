using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {

	private int points = 0;
	[HideInInspector]
	public bool gameOver = false;

	private TMPro.TextMeshProUGUI scoreText;
	private TMPro.TextMeshProUGUI finalScore;

	private GameObject inGameHUD;
	private GameObject loseHUD;

	private GameObject vip;

#region Singleton
	public static GameManager instance;
	void Awake() {
		if (instance == null) 
			instance = this;
		else
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
		inGameHUD = transform.Find("Canvas/InGameHUD").gameObject;
		loseHUD = transform.Find("Canvas/LoseHUD").gameObject;

		scoreText = inGameHUD.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
		finalScore = loseHUD.transform.Find("Panel/Score").GetComponent<TMPro.TextMeshProUGUI>();

		loseHUD.SetActive(false);
		inGameHUD.SetActive(false);
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
			AudioManager.instance.Play("TitleTheme");
	}
#endregion

	public void IncrementPoints() {
		points++;
		SpawnNewVIP();
	}

	private void SpawnNewVIP() {
		vip = FindObjectOfType<VIPBehavior>().gameObject;
		if (vip != null)
			vip = Instantiate(vip, RandomNavmeshLocation(200f), Quaternion.identity);
	}

	public int GetPoints() {
		return points;
	}

	public void TriggerGameOver() {
		if (gameOver) {
			return;
		}
		AudioManager.instance.Play("DeathGrunt1");
		gameOver = true;
		inGameHUD.SetActive(false);
		loseHUD.SetActive(true);
		finalScore.text = "Score: " + points;
		// Show that you died and with how many points.
		// then set points = 0;
	}

	public void StartGame() {
		points = 0;
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
		loseHUD.SetActive(false);
		inGameHUD.SetActive(true);
		AudioManager.instance.StopAllSounds();
		AudioManager.instance.Play("LevelTheme");
	}

	public void GoToMainMenu() {
		loseHUD.SetActive(false);
		inGameHUD.SetActive(false);
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		AudioManager.instance.StopAllSounds();
		AudioManager.instance.Play("TitleTheme");
	}

	void Update() {
		if (scoreText != null) {
			scoreText.text = "Score: " + points;
		}
	}

	public Vector3 RandomNavmeshLocation(float radius) {
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection += transform.position;
		NavMeshHit hit;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
			finalPosition = hit.position;            
		}
		return finalPosition;
	}
}
