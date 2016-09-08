using Priority_Queue;
using System;
using System.Collections.Generic;

public class AStarPathfinder<T> where T : class, IAstarNode<T>
{

    private FastPriorityQueue<DataSet<T>> orderedTestList;
    private static AStarPathfinder<T> _instance;
    private List<T> visited;
    private DataSet<T>[] fullDataTraveled=null;

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
        orderedTestList = new FastPriorityQueue<DataSet<T>>(16);
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
            curTest = fullDataTraveled[curTest.prev.AStarIndex()];
        }
        return ret;
    }


    int[] heuristics=new int[0];
    int[] blank;
    private DataSet<T> PopulatePath(T cur, T dest, float tweakParam)
    {
        orderedTestList.Clear();
        visited.Clear();
        fullDataTraveled = new DataSet<T>[cur.MaxAStarIndex()];

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
        fullDataTraveled[cur.AStarIndex()]= curTest;

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
                if (fullDataTraveled[neighbor.First.AStarIndex()]!=null)
                {
                    //A quicker path was found to the tile
                    if (Comparer.Compare(ds, fullDataTraveled[neighbor.First.AStarIndex()]) < 0)
                    {
                        orderedTestList.Remove(fullDataTraveled[neighbor.First.AStarIndex()]);
                        fullDataTraveled[neighbor.First.AStarIndex()] = ds;
                        Enqueue(ds);
                    }
                }
                else
                {
                    fullDataTraveled[neighbor.First.AStarIndex()] = ds;
                    Enqueue(ds);
                }

            }
            try
            {
                curTest = orderedTestList.Dequeue();
            }
            catch (InvalidOperationException)
            {
                curTest = fullDataTraveled[dest.AStarIndex()];
                break;
            }
        }

        return curTest;
    }

    private void Enqueue(DataSet<T> ds)
    {
        if (orderedTestList.Count==orderedTestList.MaxSize)
        {
            orderedTestList.Resize(orderedTestList.MaxSize * 2);
        }
        orderedTestList.Enqueue(ds, ds.Priority);
    }
}
