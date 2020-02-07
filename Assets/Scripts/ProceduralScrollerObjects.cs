using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSystem
{
    public bool objectExists = false;
    //associate a prefab or something
    public float objectR = 0.0f;
    public float objectG = 0.0f;
    public float objectB = 0.0f;

    private uint nProcGen = 0;

    private uint rnd()
    {
        nProcGen += 0xe120fc15;
        uint tmp;
        tmp = (uint)nProcGen * 0x4a39b70d;
        uint M1 = (tmp >> 32) ^ tmp;
        tmp = (uint)M1 * 0x12fad5c9;
        uint m2 = (tmp >> 32) ^ tmp;
        return m2;
    }
    private int rndInt(int min, int max)
    {
        return ((int)rnd() % (max - min)) + min;
    }
    private float rndFloat(float min, float max)
    {
        return ((float)rnd() / (float)(0x7FFFFFFF)) * (max - min) + min;
    }

    public ObjectSystem()
    { }
    public void ObjectSystemGenerate(uint x, uint y)
    {   //Seed based on location of system
        nProcGen = (x & 0xFFFF) << 16 | (y & 0xFFFF);

        //Not all locations contain a system
        objectExists = (rndInt(0, 20) == 1);
        if (!objectExists) return;

        //Generate Object Info
        float tmpFloat = rndFloat(0.0f, 999.0f);
        objectR = (tmpFloat % 10.0f) / 9.0f;//Units
        objectG = ((tmpFloat % 100.0f) - (tmpFloat % 10.0f)) / 99.0f; //Decimals
        objectB = ((tmpFloat % 1000.0f) - (tmpFloat % 100.0f)) / 999.0f; //Hundreds

    }
}

public class ProceduralScrollerObjects : MonoBehaviour
{
    public Material AvailableMaterial;
    public GameObject AvailablePrefab;

    private Vector2 positionOffset;
    private Vector2 cameraWH;
    private float fLastDisplayUpdate;
    private ObjectSystem[] InstantiatedObjectSystems;
    // Start is called before the first frame update
    void Start()
    {
        positionOffset = new Vector2(0.0f, 0.0f);
        fLastDisplayUpdate = Time.time;
        cameraWH = new Vector2(Camera.main.orthographicSize * 2f, Camera.main.orthographicSize * 2f * Camera.main.aspect);
        InstantiatedObjectSystems = new ObjectSystem[10];
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime <= 0.0001f) return;
    }

    bool DisplayUpdate()
    {
        float fCurrentTime = Time.time;
        float fElapsedTime = fCurrentTime - fLastDisplayUpdate;
        if (fElapsedTime <= 0.0001f) return true;

        fLastDisplayUpdate = fCurrentTime;

        //we assume the offset is already previously calculated when it receives input

        int nSectorsX = (int)(cameraWH.x / 10);
        int nSectorsY = (int)(cameraWH.y / 1000);

        for (int iScreenSelectorX = 0; iScreenSelectorX < nSectorsX; iScreenSelectorX++)
        {
            for (int iScreenSelectorY = 0; iScreenSelectorY < nSectorsY; iScreenSelectorY++)
            {
                uint seed1 = (uint)positionOffset.x + (uint)iScreenSelectorX;
                uint seed2 = (uint)positionOffset.y + (uint)iScreenSelectorY;
                int i = getFirstInactiveObject();
                InstantiatedObjectSystems[i].ObjectSystemGenerate(seed1, seed2);
                //  TODO
                //  MODIFY THE ASSOCIATED OBJECT WITH THE PROCEDURAL VALUES
            }

        }

        return false;
    }

    private int getFirstInactiveObject()
    {
        int i = 0;
        while (i < 10)
        {
            if (InstantiatedObjectSystems[i].objectExists)
            {
                return i;
            }
            i++;
        }
        return 0; //TEMPORARY, DO SOMETHING BETTER AND THE THINK ON HOW MANY OBJS PER SCREEN AND HOW TO DEACTIVATE THEM
    }
}
