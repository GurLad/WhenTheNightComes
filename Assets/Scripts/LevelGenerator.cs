using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
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
    private LevelData levelData;

    private void Start()
    {
        // Load basic data
        levelData = LevelData.Interpret(EntitiesJSON.text, TileSize);
        // Generate walls
        int[,] walls = ImportWalls(WallsCSV.text, levelData.Width, levelData.Height);
        for (int x = 0; x < levelData.Width; x++)
        {
            for (int y = 0; y < levelData.Height; y++)
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
    private class LevelData
    {
        public Dictionary<string, List<Entity>> entities;
        [Newtonsoft.Json.JsonProperty]
        private int width;
        public int Width => width / tileSize;
        [Newtonsoft.Json.JsonProperty]
        private int height;
        public int Height => height / tileSize;
        private int tileSize;

        public static LevelData Interpret(string json, int tileSize)
        {
            LevelData levelData = new LevelData();
            levelData = JsonConvert.DeserializeObject<LevelData>(json);
            levelData.tileSize = tileSize;
            //JsonUtility.FromJsonOverwrite(json, this);
            //Debug.Log(JsonConvert.SerializeObject(levelData));
            return levelData;
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