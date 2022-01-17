using UnityEngine;

public class SnakeHead : SnakePart
{
    [SerializeField] private float movesPerSecond;
    [SerializeField] private GameObject snakePartPrefab;
    [SerializeField][Min(0)] private int startingBodyParts;
    private SinglyLinkedList<PlayfieldItem> body = new SinglyLinkedList<PlayfieldItem>();
    private float timeToNextMove;
    private Vector3Int currentMoveDirection;
    private Vector3Int nextMoveDirection;
    private int queuedSnakeParts;
    private bool stop;
    

    private void Start()
    {
        nextMoveDirection = GetRandomDirection();
        body.Add(this);
        queuedSnakeParts = startingBodyParts;
        timeToNextMove = 1f / movesPerSecond;
    }

    private void Update()
    {
        if (stop)
        {
            return;
        }
        
        if(Input.anyKeyDown)
        {
            UpdateMoveDirection();
        }

        if(timeToNextMove <= 0f)
        {
            currentMoveDirection = nextMoveDirection;
            MoveSnake();
            timeToNextMove = 1f / movesPerSecond;
        }
        timeToNextMove -= Time.deltaTime;
    }

    private void MoveSnake()
    {
        Vector3Int nextPosition = playFieldPosition + currentMoveDirection;
        if(playfield.screenWrap || PositionIsInPlayfield(nextPosition))
        {
            Vector3Int previousPosition = playFieldPosition;
            if (playfield.screenWrap)
            {
                nextPosition = new Vector3Int(
                    modulo(nextPosition.x, playfield.size.x),
                    modulo(nextPosition.y, playfield.size.y),
                    modulo(nextPosition.z, playfield.size.z));
            }

            if (playfield[nextPosition]?.GetComponent<Fruit>() != null)
            {
                queuedSnakeParts++;
                playfield[nextPosition].GetComponent<Fruit>().Respawn();
            }
            if (playfield[nextPosition]?.GetComponent<SnakePart>() != null || playfield[nextPosition]?.GetComponent<Obsticle>() != null)
            {
                stop = true;
                playfield.GameOver();
                return;
            }
            
            MoveTo(nextPosition);
            SinglyLinkedList<PlayfieldItem>.Node<PlayfieldItem> node = body.head;
            while (node.next != null)
            {
                node = node.next;
                Vector3Int tempPos = node.data.playFieldPosition;
                node.data.MoveTo(previousPosition);
                previousPosition = tempPos;
            }

            if (queuedSnakeParts > 0)
            {
                AddNewSnakePart(previousPosition);
                queuedSnakeParts--;
            }
        }
        else
        {
            stop = true;
            playfield.GameOver();
        }
    }

    private void AddNewSnakePart(Vector3Int positionInPlayfield)
    {
        positionInPlayfield = new Vector3Int( 
            Mathf.Clamp(positionInPlayfield.x, 0, playfield.GetDimensionLength(0)),
            Mathf.Clamp(positionInPlayfield.y, 0, playfield.GetDimensionLength(1)),
            Mathf.Clamp(positionInPlayfield.z, 0, playfield.GetDimensionLength(2)));
        
        GameObject snakePartGameObject = Instantiate(snakePartPrefab, positionInPlayfield, Quaternion.identity);
        SnakePart snakePart = snakePartGameObject.GetComponent<SnakePart>();
        snakePart.playfield = this.playfield;
        snakePart.playFieldPosition = positionInPlayfield;
        playfield[positionInPlayfield] = snakePartGameObject;
        body.Add(snakePart);
    }

    private void UpdateMoveDirection()
    {
        if (currentMoveDirection != Vector3Int.back && Input.GetKeyDown(KeyCode.W) && playfield.GetDimensionLength(2) > 1)
        {
            nextMoveDirection = Vector3Int.forward;
        }
        else if (currentMoveDirection != Vector3Int.right && Input.GetKeyDown(KeyCode.A) && playfield.GetDimensionLength(0) > 1)
        {
            nextMoveDirection = Vector3Int.left;
        }
        else if (currentMoveDirection != Vector3Int.forward && Input.GetKeyDown(KeyCode.S) && playfield.GetDimensionLength(2) > 1)
        {
            nextMoveDirection = Vector3Int.back;
        }
        else if (currentMoveDirection != Vector3Int.left && Input.GetKeyDown(KeyCode.D) && playfield.GetDimensionLength(0) > 1)
        {
            nextMoveDirection = Vector3Int.right;
        }
        else if(currentMoveDirection != Vector3Int.up && Input.GetKeyDown(KeyCode.Q) && playfield.GetDimensionLength(1) > 1)
        {
            nextMoveDirection = Vector3Int.down;
        }
        else if(currentMoveDirection != Vector3Int.down && Input.GetKeyDown(KeyCode.E) && playfield.GetDimensionLength(1) > 1)
        {
            nextMoveDirection = Vector3Int.up;
        }
    }

    private Vector3Int GetRandomDirection()
    {
        int r = Random.Range(0, 3);
        if (r == 0)
        {
            return Vector3Int.forward;
        }
        else if (r == 1)
        {
            return Vector3Int.left;
        }
        else if (r == 2)
        {
            return Vector3Int.back;
        }
        else
        {
            return Vector3Int.right;
        }
    }

    private bool PositionIsInPlayfield(Vector3Int position)
    {
        if( position.x >= 0 && position.x < playfield.GetDimensionLength(0) &&
            position.y >= 0 && position.y < playfield.GetDimensionLength(1) &&
            position.z >= 0 && position.z < playfield.GetDimensionLength(2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private int modulo(int x, int y)
    {
        int result = x % y;
        return result < 0 ? result + y : result;
    }
}
