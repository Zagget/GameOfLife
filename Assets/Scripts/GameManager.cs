using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [Range(0, 50)]
    public int speed = 1;

    [Range(0, 100)]
    public int spawnChancePercentage = 15;

    [Range(100, 1000000)]
    public int totalCells = 180;

    [Header("Grid & Tiles")]
    public Grid grid;
    public Tilemap tilemap;
    public Tile aliveTile;
    public Tile deadTile;

    bool[,] cells;
    bool[,] futureCells;
    bool newGame;
    
    Vector3Int tilePos;
    
    float cellSize;
    float updateDelay, timeSinceLastUpdate;
    float width, height;
    
    int columns, rows;

    void Start()
    {
        Application.targetFrameRate = 30;

        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;

        NewGame();
    }


    public void NewGame()
    {
        newGame = true; 
        ClearTiles();

        CalculateGridSize();
        AdjustGridPosition();

        GenerateCells();
        newGame = false;
    }


    void CalculateGridSize()
    {
        float aspectRatio = width / height;

        columns = Mathf.FloorToInt(Mathf.Sqrt(totalCells * aspectRatio));
        rows = Mathf.FloorToInt(totalCells / (float)columns);

        float cellWidth = width / columns;
        float cellHeight = height / rows;

        cellSize = Mathf.Min(cellWidth, cellHeight);

        grid.cellSize = new Vector3(cellSize, cellSize, 0);
    }


    void AdjustGridPosition()
    {
        Vector3 center = new Vector3(-(columns * cellSize) * 0.5f, -(rows * cellSize) * 0.5f, 0);
        grid.transform.position = center;
    }


    void GenerateCells()
    {
        cells = new bool[columns, rows];
        futureCells = new bool[columns, rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                bool isAlive = Random.Range(0, 100) < spawnChancePercentage;
                cells[x, y] = isAlive;

                tilePos = new Vector3Int(x, y, 0);

                if (isAlive)
                {
                    tilemap.SetTile(tilePos, aliveTile);
                }
                else
                {
                    tilemap.SetTile(tilePos, deadTile);
                }
                tilemap.SetTransformMatrix(tilePos, Matrix4x4.Scale(new Vector3(cellSize, cellSize, 1)));
            }
        }
    }


    void Update()
    {
        if (!newGame)
        {
            CheckSpeed();

            timeSinceLastUpdate += Time.deltaTime;

            if (timeSinceLastUpdate >= updateDelay)
            {
                timeSinceLastUpdate = 0f;

                CheckCellStatus();
                UpdateCells();
            }
        }
    }


    void CheckSpeed()
    {
        if (speed > 0)
        {
            updateDelay = 1f / speed;
        }
        else
        {
            updateDelay = float.MaxValue;
        }
    }


    void CheckCellStatus()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int aliveNeighbors = CountAliveNeighbours(x, y);

                if (cells[x, y])
                {
                    if (aliveNeighbors < 2 || aliveNeighbors > 3)
                    {
                        futureCells[x, y] = false;
                    }
                    else
                    {
                        futureCells[x, y] = true;
                    }
                }
                else
                {
                    if (aliveNeighbors == 3)
                    {
                        futureCells[x, y] = true;
                    }
                    else
                    {
                        futureCells[x, y] = false;
                    }
                }
            }
        }
    }


    int CountAliveNeighbours(int x, int y)
    {
        int aliveNeighbours = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int checkX = x + i;
                int checkY = y + j;

                if (checkX >= 0 && checkY >= 0 && checkX < columns && checkY < rows)
                {
                    if (cells[checkX, checkY])
                    {
                        aliveNeighbours++;
                    }
                }
            }
        }
        return aliveNeighbours;
    }


    void UpdateCells()
    {
        Tile tileToSet;
        
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                tilePos = new Vector3Int(x, y, 0);

                if (cells[x, y] != futureCells[x, y])
                {
                    if (futureCells[x, y])
                    {
                        tileToSet = aliveTile;
                    }
                    else
                    {
                        tileToSet = deadTile;
                    }
                    tilemap.SetTile(tilePos, tileToSet);
                }
            }
        }
        var temp = cells;
        cells = futureCells;
        futureCells = temp;
    }


    public void ClearTiles()
    {
        tilemap.ClearAllTiles();
    }

    public void SetSpeed(int sliderSpeed)
    {
        speed = sliderSpeed;
    }

    public void SetSpawnChance(int sliderSpawnChance)
    {
        spawnChancePercentage = sliderSpawnChance;
    }

    public void SetTotalCells(int sliderTotalCells)
    {
        totalCells = sliderTotalCells;
    }
}