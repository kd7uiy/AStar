using System;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class AStarPathfinder<T> where T : class, IAstarNode<T>
{

    private OrderedBag<DataSet<T>> orderedTestList;
    private static AStarPathfinder<T> _instance;
    private List<T> visited;
    //TODO: Consider converting this to a Tuple structure, to improve speed.
    private Dictionary<T,DataSet<T>> fullDataTraveled;

    private IComparer<DataSet<T>> Comparer;

    /// <summary>
    /// Get an instance of the pathfinder. The tile should be passed as a function
    /// </summary>
    public static AStarPathfinder<T> Instance
    {
        get{
            if (_instance== null)
            {
                _instance = new AStarPathfinder<T>();
            }
            return _instance;
        }
    }

    private AStarPathfinder()
    {
        orderedTestList = new OrderedBag<DataSet<T>>();
        fullDataTraveled = new Dictionary<T, DataSet<T>>();
        visited = new List<T>();
        Comparer = Comparer<DataSet<T>>.Default;
    }

    /// <summary>
    /// Finds the distance to a given tile, given A* processing
    /// </summary>
    /// <param name="cur">The start tile</param>
    /// <param name="dest">The end tile</param>
    /// <returns></returns>
    public int DistanceTo(T cur, T dest, float tweakParam)
    {
        return PopulatePath(cur, dest, tweakParam).distTraveled;
    }

    /// <summary>
    /// Find a path from the current tile to the destination. Assumes there is a path to the destination
    /// </summary>
    /// <param name="cur">The start tile</param>
    /// <param name="dest">The end tile</param>
    /// <returns></returns>
    public List<T> FindPath(T cur, T dest,float tweakParam)
    {
        DataSet<T> curTest = PopulatePath(cur, dest, tweakParam);
        List<T> ret = new List<T>();
        while (curTest.prev != null)
        {
            ret.Insert(0, curTest.current);
            curTest = fullDataTraveled[curTest.prev];
        }
        return ret;
    }


    int[] heuristics=new int[0];
    int[] blank;
    private DataSet<T> PopulatePath(T cur, T dest, float tweakParam)
    {
        orderedTestList.Clear();
        visited.Clear();
        fullDataTraveled.Clear();

        Tuple<T, int>[] set;
        if (heuristics.Length != cur.MaxAStarIndex())
        {
            heuristics = new int[cur.MaxAStarIndex()];
            blank = new int[cur.MaxAStarIndex()];
        } else
        {
            Array.Copy(blank, heuristics, cur.MaxAStarIndex());
        }
        DataSet<T> curTest = new DataSet<T>(cur, null, 0, 0);
        fullDataTraveled.Add(cur, curTest);

        while (curTest.current != dest)
        {
            visited.Add(curTest.current);
            set = curTest.current.GetNeighborsAstarDistance(tweakParam);
            foreach (Tuple<T,int> neighbor in set)
            {
                int distanceTo = curTest.distTraveled + neighbor.Second;
                int heuristic = 0;
                if (heuristics[neighbor.First.AStarIndex()]>0)
                {
                    heuristic = heuristics[neighbor.First.AStarIndex()];
                }
                else
                {
                    heuristic = neighbor.First.GetAstarHeuristic(dest, tweakParam);
                    heuristics[neighbor.First.AStarIndex()]= heuristic;
                }

                DataSet<T> ds = new DataSet<T>(neighbor.First, curTest.current, distanceTo, distanceTo + heuristic);
                if (fullDataTraveled.ContainsKey(neighbor.First))
                {
                    //A quicker path was found to the tile
                    if (Comparer.Compare(ds, fullDataTraveled[neighbor.First]) < 0)
                    {
                        orderedTestList.Remove(fullDataTraveled[neighbor.First]);
                        fullDataTraveled[neighbor.First] = ds;
                        orderedTestList.Add(ds);
                    }
                }
                else
                {
                    fullDataTraveled.Add(neighbor.First, ds);
                    orderedTestList.Add(ds);
                }

            }
            try
            {
                curTest = orderedTestList.RemoveFirst();
            }
            catch (InvalidOperationException)
            {
                curTest = fullDataTraveled[dest];
                break;
            }
        }

        return curTest;
    }
}
