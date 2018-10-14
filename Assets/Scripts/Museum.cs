using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Museum : MonoBehaviour {

    public Vector2Int size;
    public float cellSize;
    public MuseumCell cellPrefab;
    public MuseumEntry entryPrefab;
    public MuseumPassage passagePrefab;
    public MuseumWall wallPrefab;
    public MuseumDisplayCase displayCasePrefab;
    public Painting paintingPrefab;
    public string[] paintings;
    

    [Range(0f, 1f)]
    public float entryProbability;
    [Range(0f, 1f)]
    public float displayCaseProbability;
    [Range(0f, 1f)]
    public float paintingDensity;

    private MuseumCell[,] floorPlan;
    private List<MuseumRoom> rooms = new List<MuseumRoom>();

    public void Generate()
    {
        floorPlan = new MuseumCell[size.x, size.y];
        List<MuseumCell> activeCells = new List<MuseumCell>();
        GenerateInit(activeCells);
        while (activeCells.Count > 0)
        {
            GenerateNext(activeCells);
        }
    }

    void GenerateInit(List<MuseumCell> activeCells)
    {
        MuseumCell cell = CreateCell(RandomCoordinate);
        cell.Initialize(CreateMuseumRoom());
        activeCells.Add(cell);
    }

    void GenerateNext(List<MuseumCell> activeCells)
    {
        int currIdx = activeCells.Count - 1;
        MuseumCell currCell = activeCells[currIdx];
        if (currCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currIdx);
            return;
        }
        CellDirection direction = currCell.RandomUninitializedDirection;
        Vector2Int coordinate = currCell.gridCoordinate + direction.ToVector2Int();

        if (InCoordinateRange(coordinate))
        {
            MuseumCell neighbor = GetCell(coordinate);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinate);
                CreatePassage(currCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else if (currCell.room == neighbor.room)
            {
                CreatePassageInSameRoom(currCell, neighbor, direction);
            }
            else
            {
                CreateWall(currCell, neighbor, direction);
            }
        }
        else
        {
            CreateWall(currCell, null, direction);
        }
    }

    private MuseumCell CreateCell(Vector2Int gridCoordinate)
    {
        MuseumCell cell = Instantiate(cellPrefab) as MuseumCell;
        floorPlan[gridCoordinate.x, gridCoordinate.y] = cell;
        cell.gridCoordinate = gridCoordinate;
        cell.name = "Museum Cell " + gridCoordinate.x + ", " + gridCoordinate.y;
        cell.transform.parent = transform;
        cell.transform.localPosition = GridToWorldCoordinate(gridCoordinate);
        return cell;
    }

    private void CreatePassage(MuseumCell cell, MuseumCell otherCell, CellDirection direction)
    {
        MuseumPassage prefab = Random.value < entryProbability ? entryPrefab : passagePrefab;
        MuseumPassage passage = Instantiate(prefab) as MuseumPassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(prefab) as MuseumPassage;
        if (passage is MuseumEntry)
        {
            otherCell.Initialize(CreateMuseumRoom());
        }
        else
        {
            otherCell.Initialize(cell.room);
        }
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreatePassageInSameRoom(MuseumCell cell, MuseumCell otherCell, CellDirection direction)
    {
        MuseumRoom room = cell.room;

        bool isDisplayCase = Random.value < displayCaseProbability;

        if (isDisplayCase)
        {
            MuseumPassage displayCase = Instantiate(displayCasePrefab) as MuseumPassage;
            displayCase.Initialize(cell, otherCell, direction);
            displayCase = Instantiate(displayCasePrefab) as MuseumPassage;
            displayCase.Initialize(otherCell, cell, direction.GetOpposite());
        }
        else
        {
            MuseumPassage passage = Instantiate(passagePrefab) as MuseumPassage;
            passage.Initialize(cell, otherCell, direction);
            passage = Instantiate(passagePrefab) as MuseumPassage;
            passage.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    private void CreateWall(MuseumCell cell, MuseumCell otherCell, CellDirection direction)
    {
        bool hasPainting = Random.value < paintingDensity;

        MuseumWall wall = Instantiate(wallPrefab) as MuseumWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefab) as MuseumWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }

        if (hasPainting)
        {
            Painting painting = Instantiate(paintingPrefab) as Painting;
            painting.Initialize(cell, direction);

            if (paintings.Length > 0)
            {
                string paintingName = paintings[(int)Random.Range(0, paintings.Length)];
                painting.LoadTexture(paintingName);
            }
        }
    }

    private MuseumRoom CreateMuseumRoom()
    {
        MuseumRoom room = ScriptableObject.CreateInstance<MuseumRoom>();
        //room.displayWallDirection = CellDirections.RandomValue;
        rooms.Add(room);
        return room;
    }

    public bool InCoordinateRange(Vector2Int coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.y >= 0 && coordinate.y < size.y;
    }

    public Vector2Int RandomCoordinate
    {
        get { return new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y)); }
    }

    MuseumCell GetCell(Vector2Int coordinate)
    {
        return floorPlan[coordinate.x, coordinate.y];
    }

    public Vector3 GridToWorldCoordinate(Vector2Int gridCoordinate)
    {
        return new Vector3((gridCoordinate.x - size.x * 0.5f) * cellSize + 0.5f,
                           0f,
                           (gridCoordinate.y - size.y * 0.5f) * cellSize + 0.5f);
    }
}
