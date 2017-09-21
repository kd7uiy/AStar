using Priority_Queue;
using System;
using System.Text;

public class DataSet<T> where T : class, IAstarNode<T>
{
    public T current;
    public int prevId;
    public int distTraveled;

    public DataNode<T> node { get; protected set; }

    public DataSet(T current, int prevId, int distTraveled, int estCost)
    {
        this.current = current;
        node = new DataNode<T>(current.AStarIndex, estCost);
        Repopulate(prevId, distTraveled);
    }

    public void Repopulate(int prevId, int distTraveled)
    {
        this.prevId = prevId;
        this.distTraveled = distTraveled;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Current: " + current );
        sb.AppendLine("prev: " + prevId );
        sb.AppendLine("DistTraveled: " + distTraveled);
        return sb.ToString();
    }
}
