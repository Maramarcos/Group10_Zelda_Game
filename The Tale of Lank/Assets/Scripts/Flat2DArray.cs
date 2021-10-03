using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Unity serializer doesn't serialize multidimensional arrays. Use for serializing to xml

/*
 *Visualization:
 *RowLength = 4
 *ColumnLength = 3
 *
 *2D array
 *  | 0   1   2   3
 *--|---------------
 *0 | Xa  Xb  Xc  Xd
 *1 | Ya  Yb  Yc  Yd
 *2 | Za  Zb  Zc  Zd
 *
 *Flat 2D array
 *  0   1   2   3   4   5   6   7   8   9   10  11
 *  Xa  Xb  Xc  Xd  Ya  Yb  Yc  Yd  Za  Zb  Zc  Zd
 *
 *
 *[2][1] == Yc <==> [(RowLength*1)+2] == [(4*1)+2] == [4+2] == [6] == Yc. Yc == Yc
 *[3][2] == Zd <==> [(RowLength*2)+3] == [(4*2)+3] == [8+3] == [11] == Zd. Zd == Zd
*/
[Serializable]
public class Flat2DArray<T> : IEnumerable
{
    [SerializeField]
    private T[] array;
    [SerializeField]
    private int columnLength;
    [SerializeField]
    private int rowLength;

    public Flat2DArray(int columnLength, int rowLength)
    {
        this.array = new T[columnLength * rowLength];
        this.columnLength = columnLength;
        this.rowLength = rowLength;
    }

    public T this[int x, int y]
    {
        get
        {
            return array[(rowLength * y) + x];
        }
        set
        {
            array[(rowLength * y) + x] = value;            
        }
    }

    public T this[int i]
    {
        get
        {
            return array[i];
        }
        set
        {
            array[i] = value;
        }
    }

    public int GetLength(int dim)
    {
        if(dim == 0)
        {
            return rowLength;
        }
        else if(dim == 1)
        {
            return columnLength;
        }
        else
        {
            throw new Exception("Error: Invalid provided dimension size when getting length of array");
        }
    }

    public int GetFlatLength()
    {
        return array.Length;
    }

    public IEnumerator GetEnumerator()
    {
        return array.GetEnumerator();
    }
}