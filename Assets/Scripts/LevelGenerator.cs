using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    public TextAsset WallsCSV;
    public TextAsset EntitiesJSON;
    [Header("Level data")]
    public int TileSize = 16;
    public int PhysicalSize = 1;
    [Header("Walls")]
    public Transform WallHolder;
    public GameObject Wall;
    public GameObject Void;
    [Header("Entities")]
    public Transform EntityHolder;
    public GameObject Bed;
    public GameObject Caretaker;
    public GameObject Player;
    public GameObject Object;
    [Header("Minimap")]
    public Camera MinimapCamera;
    public RenderTexture MinimapRenderer;
    public RawImage MinimapUI;
    private LevelData levelData;
    private int[,] walls;

    private void Start()
    {
        // Load basic data
        levelData = LevelData.Interpret(EntitiesJSON.text, TileSize);
        // Generate walls
        walls = ImportWalls(WallsCSV.text, levelData.Width, levelData.Height);
        for (int x = 0; x < levelData.Width; x++)
        {
            for (int y = 0; y < levelData.Height; y++)
            {
                switch (walls[x,y])
                {
                    case 1: // Wall
                        GameObject newWall = Instantiate(Wall, WallHolder);
                        newWall.transform.position += new Vector2Int(x, y).To3D() * PhysicalSize;
                        newWall.SetActive(true);
                        break;
                    case 2: // Void
                        GameObject newVoid = Instantiate(Void, WallHolder);
                        newVoid.transform.position += new Vector2Int(x, y).To3D() * PhysicalSize;
                        newVoid.SetActive(true);
                        break;
                    case 3: // Pathfinding blocker - aka floor
                    default:
                        break;
                }
            }
        }
        // Generate objects
        foreach (List<Entity> entities in levelData.entities.Values)
        {
            foreach (Entity entity in entities)
            {
                GameObject entityObject = null;
                Vector2Int pos = new Vector2Int(entity.x / TileSize, entity.y / TileSize);
                switch (entity.id)
                {
                    case "BedV":
                    case "BedH":
                        entityObject = Instantiate(Bed, EntityHolder);
                        // Rotate base on direction of nearest wall
                        int rotation = 0;
                        if (entity.customFields.ContainsKey("Rotation") && (int)entity.customFields["Rotation"] >= 0)
                        {
                            rotation = (int)entity.customFields["Rotation"];
                        }
                        else
                        {
                            if (entity.id == "BedV")
                            {
                                if (SafeGetWall(pos.x, pos.y - 1) > 0)
                                {
                                    rotation = 270;
                                }
                                else
                                {
                                    entityObject.transform.position += new Vector3(PhysicalSize, 0, 0);
                                    rotation = 90;
                                }
                            }
                            else // if (entity.id == "BedH")
                            {
                                if (SafeGetWall(pos.x - 1, pos.y) > 0)
                                {
                                    rotation = 180;
                                }
                                else
                                {
                                    entityObject.transform.position += new Vector3(0, 0, PhysicalSize);
                                    rotation = 0;
                                }
                            }
                        }
                        entityObject.transform.Rotate(new Vector3(0, rotation, 0));
                        // For pathfinding
                        walls[pos.x, pos.y] = 2;
                        if (entity.id == "BedV")
                        {
                            walls[pos.x, pos.y + 1] = 2;
                        }
                        else // if (entity.id == "BedH")
                        {
                            walls[pos.x + 1, pos.y] = 2;
                        }
                        break;
                    case "Caretaker":
                        entityObject = Instantiate(Caretaker, EntityHolder);
                        break;
                    case "Player":
                        entityObject = Instantiate(Player, EntityHolder);
                        break;
                    case "Object":
                        entityObject = Instantiate(Object, EntityHolder);
                        InteractableObject interactableObject = entityObject.GetComponent<InteractableObject>();
                        interactableObject.CaretakerPos = entity.customFields["CaretakerTarget"].ToVector2Int();
                        interactableObject.Init((int)entity.customFields["Type"]);
                        break;
                    default:
                        throw new System.Exception("What");
                }
                entityObject.transform.position += pos.To3D() * PhysicalSize;
                entityObject.SetActive(true);
            }
        }
        // Init pathfinder
        Pathfinder.SetMap(walls, new Vector2Int(levelData.Width, levelData.Height));
        // Move minimap
        MinimapCamera.transform.position = new Vector3(levelData.Height / 2.0f - 0.5f, 0, levelData.Width / 2.0f - 0.5f);
        MinimapCamera.orthographicSize = levelData.Height / 2.0f;
        MinimapRenderer = Instantiate(MinimapRenderer);
        MinimapRenderer.Release();
        MinimapRenderer.width = levelData.Width * TileSize * 4;
        MinimapRenderer.height = levelData.Height * TileSize * 4;
        MinimapRenderer.Create();
        MinimapUI = MinimapUI != null ? MinimapUI : FindObjectOfType<RawImage>(); // Unity can't use ??...
        MinimapUI.texture = MinimapCamera.targetTexture = MinimapRenderer;
        MinimapUI.rectTransform.sizeDelta = new Vector2(MinimapUI.rectTransform.sizeDelta.y * ((float)levelData.Width / levelData.Height), MinimapUI.rectTransform.sizeDelta.y);
    }

    private int SafeGetWall(int x, int y)
    {
        if (x < 0 || y < 0 || x >= levelData.Width || y >= levelData.Height)
        {
            return 0;
        }
        return walls[x, y];
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
    }

    [System.Serializable]
    private class Entity
    {
        public string id;
        public int x;
        public int y;
        public int width;
        public int height;
        public Dictionary<string, EntityField> customFields;
    }

    [System.Serializable]
    private class EntityField
    {
        public int cx;
        public int cy;
        public System.Int64 intData;

        public EntityField() { }
        public EntityField(System.Int64 data) { intData = data; }

        public Vector2Int ToVector2Int()
        {
            return new Vector2Int(cx, cy);
        }

        public static implicit operator EntityField(System.Int64 i) =>
            new EntityField(i);
        public static implicit operator System.Int64(EntityField ef) =>
            ef.intData;
    }
}