using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFactory : MonoBehaviour {

    public int sizeX, sizeY;
    public float cellSize;
    public MuseumCell cellPrefab;
    public MuseumPassage passagePrefab;
    public MuseumWall wallPrefab;

    private MuseumCell[,] floorPlan;
    private Vector2Int initCoordinate;

	// Use this for initialization
	public void Start ()
    {
        Generate();
	}

    public void Generate()
    {
        floorPlan = new MuseumCell[sizeX, sizeY];
        List<MuseumCell> activeCells = new List<MuseumCell>();
        GenerateInit(activeCells);
        while (activeCells.Count > 0)
        {
            GenerateNext(activeCells);
        }
    }

    void GenerateInit(List<MuseumCell> activeCells)
    {
        initCoordinate = RandomCoordinate;
        activeCells.Add(CreateCell(initCoordinate));
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
        Vector2Int nextCoordinate = currCell.gridCoordinate + direction.ToVector2Int();

        if (InCoordinateRange(nextCoordinate))
        {
            MuseumCell neighbor = GetCell(nextCoordinate);
            if (neighbor == null)
            {
                neighbor = CreateCell(nextCoordinate);
                CreatePassage(currCell, neighbor, direction);
                activeCells.Add(CreateCell(nextCoordinate));
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
        MuseumPassage passage = Instantiate(passagePrefab) as MuseumPassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MuseumPassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(MuseumCell cell, MuseumCell otherCell, CellDirection direction)
    {
        MuseumWall wall = Instantiate(wallPrefab) as MuseumWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefab) as MuseumWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    public bool InCoordinateRange(Vector2Int coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < sizeX && coordinate.y >= 0 && coordinate.y < sizeY;
    }

    public Vector2Int RandomCoordinate
    {
        get { return new Vector2Int(Random.Range(0, sizeX), Random.Range(0, sizeY)); }
    }

    MuseumCell GetCell(Vector2Int coordinate)
    {
        return floorPlan[coordinate.x, coordinate.y];
    }

    public Vector3 GridToWorldCoordinate(Vector2Int gridCoordinate)
    {
        return new Vector3(gridCoordinate.x - sizeX * 0.5f + 0.5f,
                           0f,
                           gridCoordinate.y - sizeY * 0.5f + 0.5f);
    }
}
