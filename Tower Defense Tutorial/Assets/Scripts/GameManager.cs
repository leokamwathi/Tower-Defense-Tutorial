using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public TowerBtn ClickedBtn { get; set; }

	public int Currency
	{
		get
		{
			return currency;
		}

		set
		{
			this.currency = value;
			this.currencyTxt.text = value.ToString() + " <color=lime>$</color>";
		}
	}

	private int currency;

	private int wave = 0;

	private int lives;

	private bool gameOver = false;

	[SerializeField]
	private GameObject gameOverMenu;

	[SerializeField]
	private Text livesTxt;

	public int Lives
	{
		get
		{
			return lives;
		}

		set
		{
			this.lives = value;
			livesTxt.text = lives.ToString();
			if (lives <=0)
			{
				this.lives = 0;
				GameOver();
			}
		}
	}

	public bool WaveActive
	{
		get
		{
			return activeMonsters.Count > 0;
		}
	}

	[SerializeField]
	private Text waveTxt;

	[SerializeField]
	private Text currencyTxt;

	[SerializeField]
	private GameObject waveBtn;

	public ObjectPool Pool { get; set; }

	private List<Monster> activeMonsters = new List<Monster>();

	private void Awake()
	{
		Pool = GetComponent<ObjectPool>();
	}

	// Use this for initialization
	void Start ()
	{
		Lives = 10;
		Currency = 100;
	}
	
	// Update is called once per frame
	void Update ()
	{
		HandleEscape();	
	}

	public void PickTower(TowerBtn towerBtn)
	{
		if (Currency >=towerBtn.Price && !WaveActive)
		{
			this.ClickedBtn = towerBtn;
			Hover.Instance.Activate(towerBtn.Sprite);
		}
	}

	public void BuyTower()
	{
		if (Currency >= ClickedBtn.Price)
		{
			Currency -= ClickedBtn.Price;
			Hover.Instance.Deactivate();
		}
	}

	private void HandleEscape()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Hover.Instance.Deactivate();
		}
	}

	public void StartWave()
	{
		wave++;

		waveTxt.text = string.Format("Wave: <color=lime>{0}</color>", wave);

		StartCoroutine(SpawnWave());

		waveBtn.SetActive(false);
	}

	private IEnumerator SpawnWave()
	{ 
		LevelManager.Instance.GeneratePath();

		for (int i = 0; i < wave; i++)
		{
			int monsterIndex = Random.Range(0, 4);
			string type = string.Empty;
			switch (monsterIndex)
			{
				case 0:
					type = "BlueMonster";
					break;
				case 1:
					type = "RedMonster";
					break;
				case 2:
					type = "GreenMonster";
					break;
				case 3:
					type = "PurpleMonster";
					break;
			}

			Monster monster = Pool.GetObject(type).GetComponent<Monster>();

			monster.Spawn();

			activeMonsters.Add(monster);

			yield return new WaitForSeconds(2.5f);
		}
	}

	public void RemoveMonster(Monster monster)
	{
		activeMonsters.Remove(monster);

		if (!WaveActive && !gameOver)
		{
			waveBtn.SetActive(true);
		}
	}

	public void GameOver()
	{
		if (!gameOver)
		{
			gameOver = true;
			gameOverMenu.SetActive(true);
		}
	}

	public void Restart()
	{
		Time.timeScale = 1;

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
