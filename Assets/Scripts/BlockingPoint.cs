using System.Collections;
using UnityEngine;

public class BlockingPoint : MonoBehaviour
{
    public Vector2 Position { get; private set; }

    private void OnEnable()
    {
        int posX = Random.Range(0, GameBoard.Instance.columnCount);
        int posY = Random.Range(0, GameBoard.Instance.rowCount);
        Position = new Vector2(posX, posY);
    }
}
