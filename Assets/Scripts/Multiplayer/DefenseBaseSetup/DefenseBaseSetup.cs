using UnityEngine.SceneManagement;
using UnityEngine;

public class DefenseBaseSetup : MonoBehaviour
{
    const float pausedTimeScale = 0f;
    const float playSpeed = 1f;

    public bool isReady = false;

    [SerializeField]
    Vector2Int boardSize = new Vector2Int(11, 11);

    [SerializeField]
    DefenseBaseBoard board = default;

    [SerializeField]
    GameTileContentFactory tileContentFactory = default;

    TowerType selectedTowerType;

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    static DefenseBaseSetup instance;

    void OnEnable()
    {
        instance = this;
    }

    void Awake()
    {
        board.Initialize(boardSize, tileContentFactory);
        //board.ShowGrid = true;
    }

    void BeginNewGame()
    {
        board.Clear();
    }

    void OnValidate()
    {
        if (boardSize.x < 2)
        {
            boardSize.x = 2;
        }
        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (isReady)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            //board.ShowPaths = !board.ShowPaths;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            //board.ShowGrid = !board.ShowGrid;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTowerType = TowerType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTowerType = TowerType.Mortar;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale =
                Time.timeScale > pausedTimeScale ? pausedTimeScale : playSpeed;
        }
        else if (Time.timeScale > pausedTimeScale)
        {
            Time.timeScale = playSpeed;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            BeginNewGame();
        }

        Physics.SyncTransforms();
        board.GameUpdate();
    }

    void HandleAlternativeTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                board.ToggleDestination(tile);
            }
            else
            {
                board.ToggleSpawnPoint(tile);
            }
        }
    }

    void HandleTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                board.ToggleTower(tile, selectedTowerType);
            }
            else
            {
                board.ToggleWall(tile);
            }
        }
    }
}
