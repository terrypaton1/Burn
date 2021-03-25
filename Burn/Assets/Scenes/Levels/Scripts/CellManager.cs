using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CellManager : MonoBehaviour
{
    [SerializeField]
    public Cell[] randomizedCells;

    [SerializeField]
    public Cell[] changeShipCells;

    [SerializeField]
    public Cell[] startCells;

    [SerializeField]
    public EndCell endCell;

    private Cell[] sortedCells;
    private int lastDisplayedCell = -1;

    protected Vector3 position;

    private readonly Random rnd = new Random();

    public void Reset()
    {
        // reset all the cells
        foreach (var cell in startCells)
        {
            cell.Reset();
        }

        foreach (var cell in randomizedCells)
        {
            cell.Reset();
        }

        foreach (var cell in changeShipCells)
        {
            cell.Reset();
        }

        endCell.Reset();
        lastDisplayedCell = -1;
        SetupLevel();
    }

    protected virtual void OnEnableFunction()
    {
        if (!Application.isPlaying)
        {
            DisplayLevelDebugCells();
            return;
        }

        RegisterLevel();
    }

    private void RegisterLevel()
    {
        if (CoreConnector.IsLevelsLoaded)
        {
            CoreConnector.Levels.RegisterLevel(this);
        }
    }

    private void DisplayLevelDebugCells()
    {
        var counter = 0;

        PositionCells(startCells, ref counter);
        PositionCells(randomizedCells, ref counter);

        // position the end cell, there is only one
        position.z = GameSettings.CellLength * counter;
        endCell.Position(position);
    }

    protected void OnEnable()
    {
        OnEnableFunction();
    }

    public void SetupLevel()
    {
        var cellList = GetRandomizedCellList(randomizedCells);
        InsertChangeShipCells(ref cellList);

        sortedCells = cellList.ToArray();
        SetNotStaticObjects();
        DisplayLevelCells();

        SetStaticObjects();
    }

    private List<Cell> GetRandomizedCellList(Cell[] cells)
    {
        var cellList = new List<Cell>(cells);
        cellList.Shuffle(rnd);
        return cellList;
    }

    private void InsertChangeShipCells(ref List<Cell> cellList)
    {
        var stepSize = CalculateChangeShipInterval();
        var counter = 0;
        var shipCounter = 0;
        var newList = new List<Cell>();

        for (int i = 0; i < cellList.Count; ++i)
        {
            var nextCell = cellList[i];
            newList.Add(nextCell);
            counter++;
            if (counter >= stepSize)
            {
                if (shipCounter < changeShipCells.Length)
                {
                    var cell = changeShipCells[shipCounter];
                    newList.Add(cell);
                    shipCounter++;
                }

                counter = 0;
            }
        }

        cellList = newList;
    }

    private int CalculateChangeShipInterval()
    {
        var value = Mathf.CeilToInt(randomizedCells.Length / (changeShipCells.Length));
        return value;
    }

    private void DisplayLevelCells()
    {
        var counter = 0;

        PositionCells(startCells, ref counter);
        PositionCells(sortedCells, ref counter);

        // position the end cell, there is only one
        position.z = GameSettings.CellLength * counter;
        endCell.Position(position);
    }

    protected void PositionCells(Cell[] cells, ref int counter)
    {
        foreach (var cell in cells)
        {
            position.z = GameSettings.CellLength * counter;
            cell.Position(position);
            counter++;
        }
    }

    public Vector3 GetLevelEndLocation()
    {
        var endLocation = endCell.GetLevelEndLocation();
        return endLocation;
    }

    public Vector3 GetFinishLineStopPosition()
    {
        var endLocation = endCell.GetFinishLineStopPosition();
        return endLocation;
    }

    public EndCell GetEndCell()
    {
        return endCell;
    }

    private void SetStaticObjects()
    {
        foreach (var cell in sortedCells)
        {
            cell.SetObjectsToStatic();
        }
    }

    private void SetNotStaticObjects()
    {
        foreach (var cell in sortedCells)
        {
            cell.SetObjectsToNoTStatic();
        }
    }

    public void UpdateLoop()
    {
        var playerPosition = CoreConnector.Player.GetCurrentPosition();
        var testPosition1 = playerPosition;

        var testCell = GetCellWithinPosition(testPosition1);
        if (testCell != null)
        {
            testCell.TestEvents();
        }

        // hide all the cells out of range, 2 ahead and 1 behind
        DisableCellsOutSideOfPositionBuffer(testPosition1, false);
    }

    private void DisableCellsOutSideOfPositionBuffer(Vector3 pos, bool forceUpdate)
    {
        var playerLoc = Mathf.FloorToInt(pos.z / GameSettings.CellLength);

        if (!forceUpdate && playerLoc == lastDisplayedCell)
        {
            return;
        }

        var amountOfStartCells = startCells.Length;
        var totalSortedCells = sortedCells.Length;
        var maxIndexOfSortedCells = amountOfStartCells + totalSortedCells;
        lastDisplayedCell = playerLoc;

        var start = playerLoc - 2;
        if (start < 0)
        {
            start = 0;
        }

        var end = playerLoc + 2;
        if (end > maxIndexOfSortedCells)
        {
            end = maxIndexOfSortedCells - 1;
        }

        for (var i = 0; i < maxIndexOfSortedCells; ++i)
        {
            var cell = GetCellAtIndex(i);
            if (i >= start && i <= end)
            {
                cell.Show();
            }
            else
            {
                cell.Hide();
            }
        }
    }

    private Cell GetCellWithinPosition(Vector3 pos)
    {
        var playerLoc = Mathf.FloorToInt(pos.z / GameSettings.CellLength);
        return GetCellAtIndex(playerLoc);
    }

    private Cell GetCellAtIndex(int index)
    {
        var amountOfStartCells = startCells.Length;
        var totalSortedCells = sortedCells.Length;
        var maxIndexOfSortedCells = amountOfStartCells + totalSortedCells;
        if (index < 0)
        {
            index = 0;
        }

        if (index < maxIndexOfSortedCells)
        {
            if (index < amountOfStartCells)
            {
                var cell = startCells[index];
                return cell;
            }
            else
            {
                // player is at one of the random cells
                var currentCell = index - amountOfStartCells;
                if (currentCell < 0)
                {
                    currentCell = 0;
                }

                //Debug.Log("currentCell:" + currentCell);
                var cell = sortedCells[currentCell];
                return cell;
            }

            // not testing events in the final cell.
        }

        // must be an event in one of the event cells.          
        return endCell;
    }

    public void DisableColliders()
    {
        foreach (var cell in startCells)
        {
            cell.DisableAllColliders();
        }

        foreach (var cell in randomizedCells)
        {
            cell.DisableAllColliders();
        }

        foreach (var cell in changeShipCells)
        {
            cell.DisableAllColliders();
        }

        endCell.DisableAllColliders();
    }

    public void EnableColliders()
    {
        foreach (var cell in startCells)
        {
            cell.EnableColliders();
        }

        foreach (var cell in randomizedCells)
        {
            cell.EnableColliders();
        }

        foreach (var cell in changeShipCells)
        {
            cell.EnableColliders();
        }

        endCell.EnableColliders();
    }

    public void DisableRenderers()
    {
        foreach (var cell in startCells)
        {
            cell.DisableRenderers();
        }

        foreach (var cell in randomizedCells)
        {
            cell.DisableRenderers();
        }

        foreach (var cell in changeShipCells)
        {
            cell.DisableRenderers();
        }

        endCell.DisableRenderers();
    }

    public void EnableRenderers()
    {
        foreach (var cell in startCells)
        {
            cell.EnableRenderers();
        }

        foreach (var cell in randomizedCells)
        {
            cell.EnableRenderers();
        }

        foreach (var cell in changeShipCells)
        {
            cell.EnableRenderers();
        }

        endCell.EnableRenderers();
    }

    public virtual LevelNames GetID()
    {
        return LevelNames.undefined;
    }

    public void RegisterShipMarkers()
    {
        var targetPosition = GetLevelEndLocation();

        // add the percent position of each ship change
        foreach (var shipCell in changeShipCells)
        {
            var shipCellPosition = shipCell.transform.position;

            var percent = shipCellPosition.z / targetPosition.z;
            CoreConnector.CameraControl.AddShipMarker(percent);
        }
    }
}