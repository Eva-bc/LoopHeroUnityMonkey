using UnityEngine;

public class Cell : MonoBehaviour, ICellActivable
{
    public virtual void Activate(Pawn CurrentPawn)
    {
        throw new System.NotImplementedException();
    }
}
