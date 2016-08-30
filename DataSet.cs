using System;
using System.Text;

public class DataSet<T> : IComparable<DataSet<T>>
{

    public T current;
    public int estCost;
    public T prev;
    public int distTraveled;

    public DataSet(T next, T current, int distTraveled, int estCost){
        this.current=next;
        this.estCost=estCost+distTraveled;      //Est Remaining distance+ distance already traveled
        this.prev=current;
        this.distTraveled = distTraveled;
        }

    public int CompareTo(DataSet<T> other)
    {
        return estCost-other.estCost;
    }


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Current: " + current );
        sb.AppendLine("estCost: " + estCost);
        sb.AppendLine("prev: " + prev );
        sb.AppendLine("DistTraveled: " + distTraveled);
        return sb.ToString();
    }
}
