using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;
    //private List<Cell> _cellsList;

    public Cell GetCellByNumber(int number) //"Cell" c'est l'output, GetCellByNumber c'est la fonction, "Int Number" est l'intput
    {
        return _cells[number];
    }

    public int GetNextCellToMove(int cellNumber)
    {
        return cellNumber % _cells.Length;
    }
}
