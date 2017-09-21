using Priority_Queue;
using System;
using System.Text;

public class DataNode<T> : FastPriorityQueueNode, IComparable<DataNode<T>>
{

    public int index;
    public int estCost;

    public DataNode(int id, int estCost)
    {
        this.index = id;
        this.estCost = estCost;
        Priority = estCost;
    }

    public int CompareTo(DataNode<T> other)
    {
        return estCost-other.estCost;
    }


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Id: " + index );
        sb.AppendLine("estCost: " + estCost);
        return sb.ToString();
    }
}
