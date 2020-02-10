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
        uint M1 = (tmp >> 16);
        M1 = M1 ^ tmp;
        tmp = (uint)M1 * 0x12fad5c9;
        uint m2 = (tmp >> 16);
        m2 = m2 ^ tmp;
        return m2;
    }
    private int rndInt(int min, int max)
    {
        uint r = rnd();
        int res = (int)r % (max - min) + min;
        return Mathf.Abs(res);
    }
    private float rndFloat(float min, float max)
    {
        return Mathf.Abs(((float)rnd() / (float)(0x7FFFFFFF)) * (max - min) + min);
    }

    public ObjectSystem()
    { }
    public bool ObjectSystemGenerate(uint x, uint y)
    {   //Seed based on location of system
        nProcGen = (x & 0xFFFF) << 16 | (y & 0xFFFF);

        //Not all locations contain a system
        objectExists = (rndInt(0, 30) == 1);
        if (!objectExists) return false;

        //Generate Object Info
        float tmpFloat = rndFloat(0.0f, 999.0f);
        objectR = (tmpFloat % 10.0f) / 9.0f;//Units
        objectG = ((tmpFloat % 100.0f) - (tmpFloat % 10.0f)) / 99.0f; //Decimals
        objectB = ((tmpFloat % 1000.0f) - (tmpFloat % 100.0f)) / 999.0f; //Hundreds

        return true;
    }
}

public class ProceduralScrollerObjects : MonoBehaviour
{
    //public Material AvailableMaterial;
    public GameObject BasePrefab = null;
    public int NumOfInstantiatedPrefabs = 20;
    public Camera mainCamera = null;
    public float movementSpeed = 5;

    //private Vector2 positionOffset;
    private int baseRand;
    private float fPreviousOffset;
    private float fCumulative;
    private Vector2 cameraWH;
    private float fLastDisplayUpdate;
    private bool[] bIsPrefabAvailable;
    private GameObject[] AvailablePrefabs;
    private ObjectSystem obj;
    // Start is called before the first frame update
    void Start()
    {
        fPreviousOffset = 0.0f;
        fCumulative = 0.0f;
        fLastDisplayUpdate = Time.time;
        mainCamera = Camera.main;
        cameraWH = new Vector2(mainCamera.orthographicSize * 2f * mainCamera.aspect, mainCamera.orthographicSize * 2f);
        bIsPrefabAvailable = new bool[NumOfInstantiatedPrefabs];
        AvailablePrefabs = new GameObject[NumOfInstantiatedPrefabs];
        for (int i = 0; i < NumOfInstantiatedPrefabs; i++)
        {
            bIsPrefabAvailable[i] = true;
            AvailablePrefabs[i] = (GameObject)Instantiate(BasePrefab, new Vector3(0.0f, 100.0f, 0.0f), Quaternion.identity);
        }
        obj = new ObjectSystem();
        baseRand = (int)UnityEngine.Random.Range(0, 255);
        DisplayUpdate(0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       float input = 0.0f;
       if (Input.GetKey(KeyCode.A)) { input -= Time.deltaTime * movementSpeed; }
       if (Input.GetKey(KeyCode.D)) { input += Time.deltaTime * movementSpeed; }
       
       if (input != 0.0f) 
       { 
            DisplayUpdate(input);
            //deactivateOutOfScopePrefabs();
        } 
    }

    void DisplayUpdate(float deltaInput)
    {
        fCumulative += deltaInput;
        Vector3 Translation = Vector3.right;
        Translation *= deltaInput;
        mainCamera.transform.Translate(Translation);
        
        //check if not initial
        int iScreenSelectorX = 0;
        int iLimit = 12;
        if (fPreviousOffset != 0.0f)
        {
            if (fPreviousOffset < 0.0f) //Venimos de la derecha
            {
                if (deltaInput < 0.0f && fCumulative <= -1.0f) // Seguimos hacia la derecha
                {
                    iLimit = 1;
                    fCumulative = 0.0f;
                }
                else //Cambio de rumbo
                {
                    iLimit = 0;
                }
            }
            else if (fPreviousOffset > 0.0f) // Venimos de la izq
            {
                if (deltaInput > 0.0f && fCumulative >= 1.0f) // Seguimos hacia la izq
                {
                    iScreenSelectorX = 11;
                    fCumulative = 0.0f;
                }
                else
                {
                    iLimit = 0;
                }
            }
        }

        while (iScreenSelectorX < iLimit)
        { 
            for (int iScreenSelectorY = 0; iScreenSelectorY < 10; iScreenSelectorY++) //can also test instead of nSectorsY with 10
            {
                int seed1 = (int)Mathf.Abs(mainCamera.transform.position.x) + (int)iScreenSelectorX;
                int seed2= (int)Mathf.Abs(mainCamera.transform.position.y) + (int)iScreenSelectorY;
                //control that the seeds are in the ranges of [0..9] [0..999]
                if (seed1 < 0 || seed1 >= 1000)
                {
                    if (seed1 < 0) { seed1 += 1000; }
                    else { seed1 -= 1000; }
                }

                if (seed2 >= 10 || seed2 < 0)
                {
                    if (seed2 < 0) { seed2 += 10; }
                    else { seed2 -= 10; }
                }
                int i = getFirstInactiveObject();

                if (obj.ObjectSystemGenerate((uint)seed1, (uint)seed2) && i!= -1)
                {
                    bIsPrefabAvailable[i] = false;
                    Color nColor = Color.white;
                    nColor.r = obj.objectR;
                    nColor.g = obj.objectG;
                    nColor.b = obj.objectB;
                    AvailablePrefabs[i].GetComponent<Renderer>().material.SetColor("_Color", nColor);
                    //Get the position in the world space necessary to fit in the camera sectors
                    Vector3 newPos = mainCamera.transform.position;
                    newPos.x += (float)((iScreenSelectorX - 6) / 6.0f) * (cameraWH.x / 2);
                    newPos.y += (float)(((iScreenSelectorY - 5) / 5.0f) + 0.5f)*(cameraWH.y / 2);
                    newPos.z = 0.0f;
                    AvailablePrefabs[i].transform.SetPositionAndRotation(newPos, Quaternion.identity);

                }
            }
            iScreenSelectorX++;
        }
        fPreviousOffset = deltaInput;
    }

    private int getFirstInactiveObject()
    {
        int i = 0;
        while (i < NumOfInstantiatedPrefabs)
        {
            if (bIsPrefabAvailable[i]) { return i; }
            i++;
        }

        return forceGetFurthestPrefab();
    }

    private int forceGetFurthestPrefab()
    {
        int i = 0, j = 0;
        float dist = 0.0f;
        while (i < NumOfInstantiatedPrefabs)
        {
            float nDist = (Mathf.Abs(AvailablePrefabs[i].transform.position.x - mainCamera.transform.position.x));
            if (nDist > dist) { dist = nDist; j = i; }
            i++;
        }
        return j;
    }

    private bool isPrefabInCameraScope(int i)
    {
        Vector3 pos = AvailablePrefabs[i].transform.position;
        Vector3 campos = mainCamera.transform.position;
        if (Mathf.Abs(pos.x - campos.x) > cameraWH.x/2) { return true; }
        return false;
    }

    private void deactivateOutOfScopePrefabs()
    {
        for (int i = 0; i < NumOfInstantiatedPrefabs; i++)
        {
            //if (!isPrefabInCameraScope(i))
            //{
            bIsPrefabAvailable[i] = true;
            //}
        }
    }
}
