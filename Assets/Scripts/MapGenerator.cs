using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    private ObjectPool tilePool;
    private ObjectPool crystalPool;
    public GameObject Tile;
    private float tileSizeX = 1;
    private float tileSizeZ = 1;
    public int direction { get; private set; } = Const.RIGHT;

    private float posX = 0;
    private float posY = 0;
    private float posZ = 0;

    private int crystalPos = 1;
    public bool randomCrystals = false;



    public MapGenerator(ObjectPool tilePool, ObjectPool crystalPool, Vector3 startPos, Vector3 tileSize)
    {
        this.tilePool = tilePool;
        this.crystalPool = crystalPool;
        posX = startPos.x;
        posY = startPos.y;
        posZ = startPos.z;
        tileSizeX = tileSize.x;
        tileSizeZ = tileSize.z;
    }

    public void GenerateStartSector()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                GenerateTile(posX + tileSizeX * i, posZ + tileSizeZ * j);
            }
        }

        posX += tileSizeX;
        posZ += tileSizeZ;
    }

    public void GenerateNextSector(int length)
    {
        if (direction == Const.RIGHT)
        {
            if (randomCrystals)
                crystalPos = Random.Range(0, 6);
            else
                crystalPos++;
        }

        for (int i = 0; i < length; i++)
        {
            if (direction == Const.RIGHT)
                posX += tileSizeX;
            else if (direction == Const.LEFT)
                posZ += tileSizeZ;
            var t = GenerateTile(posX, posZ);
            if (i == 0) t.GetComponent<Tile>().FirstInSector = true;

            if (direction == Const.RIGHT)
            {
                if (i == crystalPos % length)
                    GenerateCrystal(posX, posZ);
            }
        }
        direction = (direction + 1) % 2;

    }

    private GameObject GenerateTile(float posX, float posZ)
    {
        return tilePool.SpawnObject(new Vector3(posX, 0, posZ));
    }

    private GameObject GenerateCrystal(float posX, float posZ)
    {
        return crystalPool.SpawnObject(new Vector3(posX, 0.75f, posZ));
    }
}
