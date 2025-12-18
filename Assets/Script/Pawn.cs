using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Board _board;

    private void Start()
    {
        MoveToCell();
    }

    private void MoveToCell()
    {
        Transform NewPos = _board.GetCellByNumber(_playerData._cellNumber).transform; // TODO : Get cell number
        transform.position = NewPos.position;
        transform.rotation = NewPos.rotation;

    }

    public void TryMoving(int value)
    {
        _playerData._cellNumber = _board.GetNextCellToMove(_playerData._cellNumber+value);
        MoveToCell();
        ActivateCell();
    }

    private void ActivateCell()
    {
        Cell cell = _board.GetCellByNumber(_playerData._cellNumber);
        cell.Activate(CurrentPawn:this);
    }
}
