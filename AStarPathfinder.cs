using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Threading;

public class AStarPathfinder<T> where T : class, IAstarNode<T>
{

    private const int MUTEX_TIMEOUT = 5000;

    private FastPriorityQueue<DataNode<T>> orderedTestList;
    private static AStarPathfinder<T> _instance;
    private DataSet<T>[] fullDataTraveled=null;

    private Mutex mut = new Mutex();

    private IComparer<DataNode<T>> Comparer;

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
        orderedTestList = new FastPriorityQueue<DataNode<T>>(16);
        Comparer = Comparer<DataNode<T>>.Default;
    }

    /// <summary>
    /// Finds the distance to a given tile, given A* processing
    /// </summary>
    /// <param name="cur">The start tile</param>
    /// <param name="dest">The end tile</param>
    /// <returns></returns>
    public int DistanceTo(T cur, T dest, float tweakParam)
    {
        if (mut.WaitOne(MUTEX_TIMEOUT))
        {
            int ret= PopulatePath(cur, dest, tweakParam).distTraveled;
            mut.ReleaseMutex();
            return ret;
        } else
        {
            throw new TimeoutException("Failed to obtain mutex lock");
        }
    }

    /// <summary>
    /// Find a path from the current tile to the destination. Assumes there is a path to the destination
    /// </summary>
    /// <param name="cur">The start tile</param>
    /// <param name="dest">The end tile</param>
    /// <returns></returns>
    public List<T> FindPath(T cur, T dest,float tweakParam)
    {
        if (mut.WaitOne(MUTEX_TIMEOUT))
        {
            DataSet<T> curTest = PopulatePath(cur, dest, tweakParam);
            List<T> ret = new List<T>();
            while (curTest.prevId != -1)
            {
                ret.Insert(0, curTest.current);
                curTest = fullDataTraveled[curTest.prevId];
            }
            mut.ReleaseMutex();
            return ret;
        }
        else
        {
            throw new TimeoutException("Failed to obtain mutex lock");
        }
    }

    /// <summary>
    /// Populates the PathInfo from the current tile to the destination. Assumes there is a path to the destination
    /// </summary>
    /// <param name="cur">The start tile</param>
    /// <param name="dest">The end tile</param>
    /// <returns>Populated PathInfo</returns>
    public PathInfo<T> FindPathInfo(T cur, T dest, float tweakParam)
    {
        if (mut.WaitOne(MUTEX_TIMEOUT))
        {
            DataSet<T> curTest = PopulatePath(cur, dest, tweakParam);
            List<T> path = new List<T>();
            List<int> distance = new List<int>();
            while (curTest.prevId != -1)
            {
                distance.Add(curTest.distTraveled);
                path.Insert(0, curTest.current);
                curTest = fullDataTraveled[curTest.prevId];
            }
            distance.Reverse();
            PathInfo<T> ret = new PathInfo<T>(path, distance.ToArray());
            mut.ReleaseMutex();
            return ret;
        }
        else
        {
            throw new TimeoutException("Failed to obtain mutex lock");
        }  
    }

    int[] heuristics=new int[0];
    int[] blankHeuristics;
    DataSet<T>[] blankTraveled;
    private DataSet<T> PopulatePath(T cur, T dest, float tweakParam)
    {
        orderedTestList.Clear();
        if (heuristics.Length != cur.MaxAStarIndex)
        {
            fullDataTraveled = new DataSet<T>[cur.MaxAStarIndex];
            blankTraveled = new DataSet<T>[cur.MaxAStarIndex];
        } else
        {
            Array.Copy(blankTraveled, fullDataTraveled, cur.MaxAStarIndex);
        }

        Tuple<T, int>[] set;
        if (heuristics.Length != cur.MaxAStarIndex)
        {
            heuristics = new int[cur.MaxAStarIndex];
            blankHeuristics = new int[cur.MaxAStarIndex];
        } else
        {
            Array.Copy(blankHeuristics, heuristics, cur.MaxAStarIndex);
        }
        DataSet<T> curTest = new DataSet<T>(cur, -1, 0, 0);
        fullDataTraveled[cur.AStarIndex]= curTest;

        int heuristic;
        int distanceTo;
        while (curTest.current.AStarIndex != dest.AStarIndex)
        {
            set = curTest.current.GetNeighborsAstarDistance(tweakParam);
            foreach (Tuple<T,int> neighbor in set)
            {
                distanceTo = curTest.distTraveled + neighbor.Second;

                if (heuristics[neighbor.First.AStarIndex]>0)
                {
                    heuristic = heuristics[neighbor.First.AStarIndex];
                }
                else
                {
                    heuristic = neighbor.First.GetAstarHeuristic(dest, tweakParam);
                    heuristics[neighbor.First.AStarIndex]= heuristic;
                }

                if (fullDataTraveled[neighbor.First.AStarIndex]!=null)
                {
                    //A quicker path was found to the tile
                    if (distanceTo + heuristic< fullDataTraveled[neighbor.First.AStarIndex].node.Priority)
                    {
                        orderedTestList.UpdatePriority(fullDataTraveled[neighbor.First.AStarIndex].node, distanceTo + heuristic);
                        fullDataTraveled[neighbor.First.AStarIndex].Repopulate(curTest.current.AStarIndex, distanceTo);
                    }
                }
                else
                {
                    fullDataTraveled[neighbor.First.AStarIndex] = new DataSet<T>(neighbor.First, curTest.current.AStarIndex, distanceTo, distanceTo + heuristic);
                    Enqueue(fullDataTraveled[neighbor.First.AStarIndex].node);
                }

            }
            curTest = fullDataTraveled[orderedTestList.Dequeue().index];
        }
        return curTest;
    }

    private void Enqueue(DataNode<T> ds)
    {
        if (orderedTestList.Count==orderedTestList.MaxSize)
        {
            orderedTestList.Resize(orderedTestList.MaxSize * 2);
        }
        orderedTestList.Enqueue(ds, ds.Priority);
    }
}
