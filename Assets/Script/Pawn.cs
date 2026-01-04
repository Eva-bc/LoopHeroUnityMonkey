using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Board _board;
    [SerializeField] private float _heightOffset = 1f;

    private void Start()
    {
        Debug.Log("Pawn Start - Moving to initial cell");
        MoveToCell();
    }

    private void MoveToCell()
    {
        if (_board == null)
        {
            Debug.LogError("Board is not assigned to Pawn!");
            return;
        }

        Cell cell = _board.GetCellByNumber(_playerData._cellNumber);
        if (cell == null)
        {
            Debug.LogError($"Cell at index {_playerData._cellNumber} is NULL in Board!");
            return;
        }

        Transform cellTransform = cell.transform;
        Vector3 targetPosition = cellTransform.position;
        targetPosition.y += _heightOffset;

        transform.position = targetPosition;
        transform.rotation = cellTransform.rotation;

        Debug.Log($"Pawn moved to Cell {_playerData._cellNumber} at position {targetPosition}");
    }

    public void TryMoving(int value)
    {
        int oldCell = _playerData._cellNumber;
        _playerData._cellNumber = _board.GetNextCellToMove(_playerData._cellNumber + value);
        Debug.Log($"Pawn moving from Cell {oldCell} to Cell {_playerData._cellNumber} (rolled {value})");

        MoveToCell();
        ActivateCell();
    }

    private void ActivateCell()
    {
        Cell cell = _board.GetCellByNumber(_playerData._cellNumber);
        if (cell != null)
        {
            Debug.Log($"Activating Cell {_playerData._cellNumber}");
            cell.Activate(CurrentPawn: this);
        }
    }
}
