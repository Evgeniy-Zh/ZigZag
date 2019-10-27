using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public delegate void Action0();

    private MapGenerator mapGenerator;
    [SerializeField]
    private GameObject playerObj;
    private ObjectPool tilePool;
    private ObjectPool crystalPool;
    public GameObject player;
    public event Action0 OnGameStarted;
    public event Action0 OnGameOver;
    public bool RandomCrystals = false;

    private static GameManager _instanse;
    public static GameManager Instance
    {
        get
        {
            if (_instanse == null) _instanse = FindObjectOfType<GameManager>();
            return _instanse;
        }
    }

    [SerializeField, Range(1, 3)]
    private int _complexity = 1;
    public int Complexity
    {
        set
        {
            _complexity = value;
            if (_complexity < 1) _complexity = 1;
            if (_complexity > 3) _complexity = 3;
        }
        get
        {
            return _complexity;
        }
    }

    public int Points { get; private set; }

    public bool IsGameOver
    {
        get; private set;
    }

    public int tilePassedCount { get; private set; }

    private void Awake()
    {

        if (_instanse == null)
        {
            _instanse = this;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }

    }

    void Start()
    {
        var p = FindObjectOfType<PlayerController>();
        if (p != null) Destroy(p.gameObject);

        tilePool = GameObject.FindGameObjectWithTag("Tile pool").GetComponent<ObjectPool>();
        crystalPool = GameObject.FindGameObjectWithTag("Crystal pool").GetComponent<ObjectPool>();
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    private void Init()
    {
        mapGenerator = new MapGenerator(tilePool, crystalPool, Vector3.zero, Vector3.one);

        IsGameOver = false;
        tilePassedCount = 0;

        if (player != null) Destroy(player);
        player = Instantiate(playerObj, new Vector3(0, 0.75f, 0), Quaternion.identity);

        mapGenerator.randomCrystals = RandomCrystals;
        mapGenerator.GenerateStartSector();
        mapGenerator.GenerateNextSector(4);
        for (int i = 0; i < 4; i++)
        {
            mapGenerator.GenerateNextSector(CalculateSectorSize());
        }
        player.SetActive(true);
        Points = 0;
        OnGameStarted?.Invoke();
    }

    public void Restart()
    {
        tilePool.DisableAll();
        crystalPool.DisableAll();
    
        Init();
    }

    private int CalculateSectorSize()
    {
        var s = 4;
        if (mapGenerator.direction == Const.RIGHT)
            s = Random.Range(1, 6);
        else
            s = 4 - Complexity;
        return s;
    }

    public void AddPoints(int points)
    {
        Points += points;
    }

    public void TilePassed(Tile tile)
    {
        tilePassedCount++;

        if (tilePassedCount > 3) tile.Drop();
    }

    public void SectorPassed()
    {
        mapGenerator.GenerateNextSector(CalculateSectorSize());
    }

    public void GameOver()
    {
        print("Game over!");
        IsGameOver = true;
    }
}
