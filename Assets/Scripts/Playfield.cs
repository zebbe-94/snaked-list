using UnityEngine;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class Playfield : MonoBehaviour
{
    public Vector3Int size;
    public bool screenWrap;
    private GameObject[,,] playfield;
    [SerializeField] private GameObject snakeHeadPrefab;
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private GameObject gameOverPrefab;

    public GameObject this[int x, int y, int z]
    {
        get { return playfield[x,y,z]; }
        set { playfield[x, y, z] = value; }
    }
    public GameObject this[Vector3Int position]
    {
        get { return playfield[position.x,position.y,position.z]; }
        set { playfield[position.x, position.y, position.z] = value; }
    }
    public int GetDimensionLength(int dimension)
    {
        int l = playfield.GetLength(dimension);
        return l;
    }

    public void GameOver()
    {
        GameObject gameOverScreen = Instantiate(gameOverPrefab);
        gameOverScreen.GetComponentInChildren<Button>().onClick.AddListener(Restart);
        gameOverScreen.GetComponentInChildren<Button>().onClick.AddListener(delegate { Destroy(gameOverScreen); });
    }

    public void Restart()
    {
        foreach (GameObject go in playfield)
        {
            Destroy(go);
        }
        Start();
    }

    private void Start()
    {
        playfield = new GameObject[size.x, size.y, size.z];
        
        Vector3Int startPosition = size / 2;
        InstantiatePlayfieldItem(snakeHeadPrefab, startPosition);

        SpawnFruit();
    }

    private void SpawnFruit()
    {
        Vector3Int randomPosition;
        do
        {
            randomPosition = new Vector3Int(
                Random.Range(0, size.x),
                Random.Range(0, size.y),
                Random.Range(0, size.z));
        } while (playfield[randomPosition.x, randomPosition.y, randomPosition.z] != null);
        
        InstantiatePlayfieldItem(fruitPrefab, randomPosition);
    }

    private void InstantiatePlayfieldItem(GameObject prefab, Vector3Int position)
    {
        GameObject gameObjectInstance = Instantiate(prefab, position, Quaternion.identity);
        PlayfieldItem component = gameObjectInstance.GetComponent<PlayfieldItem>();
        component.playfield = this;
        component.playFieldPosition = position;
        component.playfield[position] = gameObjectInstance;
    }

}
