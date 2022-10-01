using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public TextAsset WallsCSV;
    public TextAsset EntitiesJSON;
    [Header("Level data")]
    public int TileSize = 16;
    public int PhysicalSize = 1;
    [Header("Walls")]
    public GameObject Wall;
    public Transform WallHolder;
    [Header("Entities")]
    public GameObject Bed;
    public GameObject Caretaker;
    private EntitiesInterpreter entitiesInterpreter;

    private void Start()
    {
        // Load basic data
        entitiesInterpreter = new EntitiesInterpreter(EntitiesJSON.text, TileSize);
        // Generate walls
        int[,] walls = ImportWalls(WallsCSV.text, entitiesInterpreter.Width, entitiesInterpreter.Height);
        for (int x = 0; x < entitiesInterpreter.Width; x++)
        {
            for (int y = 0; y < entitiesInterpreter.Height; y++)
            {
                if (walls[x, y] > 0)
                {
                    GameObject newWall = Instantiate(Wall, WallHolder);
                    newWall.transform.position += new Vector3(y, 0, x) * PhysicalSize;
                    newWall.SetActive(true);
                }
            }
        }
    }

    private int[,] ImportWalls(string csv, int width, int height)
    {
        int[,] result = new int[width, height];
        string[] rows = csv.Replace("\r", "").Split('\n');
        for (int y = 0; y < rows.Length - 1; y++) // Ends with newline
        {
            string row = rows[y][rows[y].Length - 1] == ',' ? rows[y].Substring(0, rows[y].Length - 1) : rows[y];
            string[] columns = row.Split(',');
            for (int x = 0; x < columns.Length; x++)
            {
                //Debug.Log("(" + x + ", " + y + "): " + columns[x]);
                result[x, y] = int.Parse(columns[x]) - 1;
            }
        }
        return result;
    }

    [System.Serializable]
    private class EntitiesInterpreter
    {
        public List<Dictionary<string, Entity>> entities;
        [SerializeField]
        private int width;
        public int Width => width / tileSize;
        [SerializeField]
        private int height;
        public int Height => height / tileSize;
        private int tileSize;

        public EntitiesInterpreter(string json, int tileSize)
        {
            this.tileSize = tileSize;
            JsonUtility.FromJsonOverwrite(json, this);
            Debug.Log(JsonUtility.ToJson(this));
        }

        [System.Serializable]
        public class Entity
        {
            public string id;
            public int x;
            public int y;
            public int width;
            public int height;
        }
    }
}