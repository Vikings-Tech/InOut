using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    private EnemyType selectedEnemyType;

    public AttackBaseBoard board;

    [SerializeField]
    public GameBoard boardOld;

    [SerializeField]
    WarFactory warFactory = default;

    //The input ray
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    GameBehaviorCollection enemies = new GameBehaviorCollection();
    GameBehaviorCollection nonEnemies = new GameBehaviorCollection();

    [SerializeField]
    EnemyFactory factory;

    static SpawnHandler instance;

    private void Start()
    {
        selectedEnemyType = 0;
    }

    private void OnEnable()
    {
        instance = this;
    }

    public void SelectSquad(int index)
    {
        selectedEnemyType = (EnemyType)index;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }

        enemies.GameUpdate();
        nonEnemies.GameUpdate();
        board.GameUpdate();
    }

    public static Explosion SpawnExplosion()
    {
        Explosion explosion = instance.warFactory.Explosion;
        instance.nonEnemies.Add(explosion);
        return explosion;
    }

    public static Shell SpawnShell()
    {
        Shell shell = instance.warFactory.Shell;
        instance.nonEnemies.Add(shell);
        return shell;
    }

    public void HandleTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile.Content.Type != GameTileContentType.SpawnPoint)
            return;

        Enemy enemy = factory.Get(selectedEnemyType);
        enemy.SpawnOn(tile);
        instance.enemies.Add(enemy);
    }
}
