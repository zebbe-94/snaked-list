using UnityEngine;

public class Fruit : PlayfieldItem
{
    public void Respawn()
    {
        Vector3Int randomPosition;
        do
        {
            randomPosition = new Vector3Int(
                Random.Range(0, playfield.size.x),
                Random.Range(0, playfield.size.y),
                Random.Range(0, playfield.size.z));
        } while (playfield[randomPosition]);
        
        MoveTo(randomPosition);
    }
}
