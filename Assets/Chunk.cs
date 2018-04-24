using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{

    private int blockXsize = 11;
    private int blockYsize = 4;
    private float density = 2f;
    private int scale = 10;
    private bool[,] map;

    public void launchGeneration(GameObject[] buildingList, GameObject road, GameObject roadCrossway, GameObject groundPlane, int xSize, int ySize, int scale, float density)
    {
        this.map = new bool[xSize, ySize];

        this.blockXsize = xSize;
        this.blockYsize = ySize;
        this.density = density;
        this.scale = scale;


        int randomCount = (int)(this.blockXsize * this.blockYsize * this.density);

        FillArray(map);
  
        Vector3 positionToBuild = new Vector3(blockXsize * scale / 2, 0, blockYsize * scale / 2);
        GameObject plane = Instantiate(groundPlane, positionToBuild, Quaternion.identity, gameObject.transform);
        plane.transform.localPosition = positionToBuild;
        //plane.transform.localScale = new Vector3(blockXsize * (scale / 10), 0, blockYsize * (scale / 10));

        while (randomCount > 0)
        {
            int randomX = Random.Range(0, this.blockXsize);
            int randomY = Random.Range(0, this.blockYsize);
            GameObject buildingToBuild = buildingList[Random.Range(0, buildingList.Length)];

            if (map[randomX, randomY] == false)
            {
                if (CanBuild(randomX, randomY, buildingToBuild) == true)
                    Build(randomX, randomY, buildingToBuild);
            }
            randomCount--;
        }

        buildRoads(road, roadCrossway);
		this.map = null;
    }

    private void buildRoads(GameObject road, GameObject roadCrossway)
    {
        int xIndex = 0;
        int yIndex = 0;
        Vector3 positionToBuild;
        Vector3 rotationToBuild;
        GameObject roadBuilding;

        positionToBuild = new Vector3(0, 0, 0);

        rotationToBuild = new Vector3(0, 90, 0);

        yIndex = blockYsize;
        while (xIndex < blockXsize)
        {
            positionToBuild = new Vector3(xIndex * scale + (1 * scale) / 2, 0, yIndex * scale + (1 * scale) / 2);
            roadBuilding = Instantiate(road, positionToBuild, Quaternion.Euler(rotationToBuild), gameObject.transform);
            roadBuilding.transform.localPosition = positionToBuild;
            xIndex++;
        }
        positionToBuild = new Vector3(xIndex * scale + (1 * scale) / 2, 0, yIndex * scale + (1 * scale) / 2);
        roadBuilding = Instantiate(roadCrossway, positionToBuild, Quaternion.Euler(rotationToBuild), gameObject.transform);
        roadBuilding.transform.localPosition = positionToBuild;
        rotationToBuild = new Vector3(0, 0, 0);
        yIndex = 0;
        while (yIndex < blockYsize)
        {
            positionToBuild = new Vector3(xIndex * scale + (1 * scale) / 2, 0, yIndex * scale + (1 * scale) / 2);
            roadBuilding = Instantiate(road, positionToBuild, Quaternion.Euler(rotationToBuild), gameObject.transform);
            roadBuilding.transform.localPosition = positionToBuild;
            yIndex++;
        }
    }

    private bool CanBuild(int x, int y, GameObject buildingToBuild)
    {
        int xIndex = 0;
        int yIndex = 0;

        while (xIndex < buildingToBuild.GetComponent<Building>().length)
        {
            while (yIndex < buildingToBuild.GetComponent<Building>().width)
            {
                if (x + xIndex < blockXsize && y + yIndex < blockYsize && map[x + xIndex, y + yIndex] == false)
                {
                    //print("Checked (" + (x + xIndex) + ", " + (y + yIndex) + ").");
                    yIndex++;
                }
                else
                    return false;
            }
            xIndex++;
            yIndex = 0;
        }

        return true;
    }

    private void Build(int x, int y, GameObject buildingToBuild)
    {
        int xIndex = 0;
        int yIndex = 0;

        Vector3 positionToBuild;
        GameObject newBuilding;

        while (xIndex < buildingToBuild.GetComponent<Building>().length)
        {
            while (yIndex < buildingToBuild.GetComponent<Building>().width)
            {
                map[x + xIndex, y + yIndex] = true;
                yIndex++;
            }
            xIndex++;
            yIndex = 0;
        }

        positionToBuild = new Vector3(x * scale + (buildingToBuild.GetComponent<Building>().width * scale) / 2, 0, y * scale + (buildingToBuild.GetComponent<Building>().length * scale) / 2);


        newBuilding = Instantiate(buildingToBuild, positionToBuild, Quaternion.identity, gameObject.transform);
        newBuilding.transform.localPosition = positionToBuild;
        //print("Builded a " + buildingToBuild.GetComponent<Building>().width + "X" + buildingToBuild.GetComponent<Building>().length + " in " + positionToBuild + " AKA (" + x + ", " + y + ").");
    }
        

    public static void FillArray(bool[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = false;
            }
        }
    }
}
