using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class Vector2di
{
    public int x { get; private set; }
    public int y { get; private set; }

    public Vector2di(int t1, int t2)
    {
        x = t1;
        y = t2;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(Vector2di)) return false;

        return Equals((Vector2di)obj);
    }

    public bool Equals(Vector2di obj)
    {
        return this.x == obj.x && this.y == obj.y;
    }

    public override string ToString()
    {
        return "X: " + this.x + " - Y:" + this.y;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (x.GetHashCode()) ^ y.GetHashCode();
        }
    }
}

public class ChunkManager : MonoBehaviour {

    public GameObject chunkPrefab;
    public GameObject[] buildingList;

    public GameObject road = null;
    public GameObject roadCrossway = null;
    public GameObject groundPlane = null;

    public int mapXSize = 100;
    public int mapYSize = 100;

    public int blockXsize = 11;
    public int blockYsize = 4;
    public float density = 2f;
    public int scale = 10;

    Transform cameraPosition;

    Dictionary<Vector2di, GameObject> chunkMap;
    List<Vector2di> chunkView;
    Vector2di viewPosition = null;

    void Start()
    {
        chunkMap = new Dictionary<Vector2di, GameObject>();

        cameraPosition = GameObject.Find("OVRPlayerController").transform;

        buildingList = Resources.LoadAll<GameObject>("TestBuilding");
        this.chunkView = new List<Vector2di>();
        /*while (xIndex < mapXSize)
        {
            while (yIndex < mapYSize)
            {
                buildChunk(new Vector2di(xIndex, yIndex));

                yIndex++;
            }
            xIndex++;
            yIndex = 0;
        }*/


        InvokeRepeating("updateChunkView", 0.0f, 0.1f);
    }

    void updateChunkView()
    {
        List<Vector2di> newChunkView;
        Vector2di newViewPosition;

        newViewPosition = get2DPositionFrom3d(this.cameraPosition.position);
        if (viewPosition == null || viewPosition.x != newViewPosition.x || viewPosition.y != newViewPosition.y)
        {
            viewPosition = newViewPosition;
            newChunkView = getChunkRadius(viewPosition);

            foreach (Vector2di chunkPosition in newChunkView)
            {
                if (chunkMap.ContainsKey(chunkPosition))
                {
                    if (chunkMap[chunkPosition].activeSelf == true)
                    {
                        this.chunkView.Remove(chunkPosition);
                    }
                    else
                    {
                        chunkMap[chunkPosition].SetActive(true);
                    }
                }
                else
                {
                    buildChunk(chunkPosition);
                }
            }

            foreach (Vector2di chunkPosition in this.chunkView)
            {
                chunkMap[chunkPosition].SetActive(false);
            }

            this.chunkView = newChunkView;
        }
    }
    
    void buildChunk(Vector2di positionChunk)
    {
        Vector3 positionToBuild;
        GameObject newChunk;

        positionToBuild = new Vector3(positionChunk.x * (blockXsize + 1) * scale, 0, positionChunk.y * (blockYsize + 1) * scale);
        //print("New chunk at " + positionToBuild);
        newChunk = Instantiate(chunkPrefab, positionToBuild, Quaternion.identity, gameObject.transform);
        chunkMap.Add(positionChunk, newChunk);
        newChunk.GetComponent<Chunk>().launchGeneration(this.buildingList, this.road, this.roadCrossway, this.groundPlane, blockXsize, blockYsize, scale, density);
    }

    List<Vector2di> getChunkRadius(Vector2di fromPosition)
    {
        List<Vector2di> newChunkInView = new List<Vector2di>();
        //373 305 249
        int radius = 6;

        for (var zCircle = -radius; zCircle <= radius; zCircle++)
        {
            for (var xCircle = -radius; xCircle <= radius; xCircle++)
            {
                if (xCircle * xCircle + zCircle * zCircle < radius * radius)
                {
                    newChunkInView.Add(new Vector2di(xCircle + fromPosition.x, zCircle + fromPosition.y));
                }
            }
        }

        return newChunkInView;
    }

    Vector2di get2DPositionFrom3d(Vector3 position)
    {
        int x = (int)Math.Floor(position.x / ((blockXsize + 1) * scale));
        int y = (int)Math.Floor(position.z / ((blockYsize + 1) * scale));

        return (new Vector2di(x, y));
    }

}
