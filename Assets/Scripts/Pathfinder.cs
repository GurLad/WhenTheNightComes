using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    private static int[,] map;
    private static Vector2Int size;

    public static void SetMap(int[,] newMap, Vector2Int newSize)
    {
        map = newMap;
        size = newSize;
    }

    public static List<Vector2Int> GetPath(Vector2Int sourceVec, Vector2Int destinationVec)
    {
        Node source = new Node(sourceVec);
        Node destination = new Node(destinationVec);

        List<Node> openSet = new List<Node>();
        openSet.Add(source);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

        Dictionary<Node, int> gScore = new Dictionary<Node, int>();
        gScore.Add(source, 0);

        Dictionary<Node, int> fScore = new Dictionary<Node, int>();
        fScore.Add(source, GetCost(source, destination));
        int count = 0;
        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) => 
                fScore.SafeGetKey(a, int.MaxValue) > fScore.SafeGetKey(b, int.MaxValue) ? 1 :
                fScore.SafeGetKey(a, int.MaxValue) < fScore.SafeGetKey(b, int.MaxValue) ? -1 : 0);
            //Debug.Log(fScore.SafeGetKey(openSet[0], int.MaxValue) + " < " + fScore.SafeGetKey(openSet[openSet.Count - 1], int.MaxValue));
            Node current = openSet[0];
            Debug.Log(current);
            if (current == destination)
            {
                return RecoverPath(cameFrom, current);
            }
            openSet.Remove(current);
            foreach (Node neighbor in current.GetNeighbors())
            {
                if (CanMove(neighbor.x, neighbor.y))
                { 
                    int tentativeScore = gScore[current] + GetDistance(current, neighbor); // No safe as the current should always have a gValue
                    if (tentativeScore < gScore.SafeGetKey(neighbor, int.MaxValue))
                    {
                        cameFrom.AddOrSet(neighbor, current);
                        gScore.AddOrSet(neighbor, tentativeScore);
                        fScore.AddOrSet(neighbor, tentativeScore + GetCost(neighbor, destination));
                        if (openSet.FindIndex(a => a == neighbor) < 0)
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
            if (count++ > 1000)
            {
                throw new System.Exception("Oof 2!");
            }
        }
        // This should be impossible
        return null;
    }

    private static List<Vector2Int> RecoverPath(Dictionary<Node, Node> cameFrom, Node current)
    {
        List<Node> totalPath = new List<Node>();
        totalPath.Add(current);
        int count = 0;
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
            if (count++ > 1000)
            {
                throw new System.Exception("Oof 2!");
            }
        }
        // Reverse & convert path
        List<Vector2Int> reversed = new List<Vector2Int>();
        for (int i = totalPath.Count - 1; i >= 0; i--)
        {
            reversed.Add(totalPath[i].ToVector2Int());
        }
        return reversed;
    }

    private static int GetCost(Node pos, Node destination)
    {
        return pos.GetDistance(destination);
    }

    private static int GetDistance(Node current, Node neighbor)
    {
        return 1; // No need to calculate, we know it's always 1
    }

    private static bool CanMove(int x, int y)
    {
        if (x < 0 || y < 0 || x >= size.x || y >= size.y)
        {
            return true;
        }
        return map[x, y] <= 0;
    }

    private class Node
    {
        public int x;
        public int y;

        public static bool operator ==(Node a, Node b)
        {
            if ((object)a == null) return (object)b == null;
            if ((object)b == null) return (object)a == null;
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }

        public Node(Vector2Int vector2Int)
        {
            x = vector2Int.x;
            y = vector2Int.y;
        }

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public List<Node> GetNeighbors()
        {
            List<Node> result = new List<Node>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i == 0 || j == 0) && (i != 0 || j != 0))
                    {
                        result.Add(new Node(x + i, y + j));
                    }
                }
            }
            return result;
        }

        public int GetDistance(Node other)
        {
            return Mathf.Abs(other.x - x) + Mathf.Abs(other.y - y);// Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(other.x - x, 2) + Mathf.Pow(other.y + y, 2)));
        }

        public Vector2Int ToVector2Int()
        {
            return new Vector2Int(x, y);
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}
