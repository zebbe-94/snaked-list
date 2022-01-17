using UnityEngine;

public abstract class PlayfieldItem : MonoBehaviour
{
    [HideInInspector]public Vector3Int playFieldPosition;
    [HideInInspector]public Playfield playfield;

    public void MoveTo(Vector3Int pos)
    {
        playfield[playFieldPosition] = null;
        playFieldPosition = pos;
        playfield[playFieldPosition] = gameObject;
        
        // TODO look into position and scaling etc
        transform.position = playFieldPosition; 
    }
}
