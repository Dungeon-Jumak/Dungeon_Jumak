//System
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GukbabGenerator : MonoBehaviour 
{
    //Want Cooking Number
    public int wantGukbabCount;

    //Gukbab Prefab
    [Header("±¹¹ä ÇÁ¸®ÆÕ")]
    public GameObject gukbabPrefab;

    //Base Gukbab Prefab
    [Header("±âº» ±¹¹ä ÇÁ¸®ÆÕ")]
    [SerializeField] GameObject baseGukbabPrefab;

    //Gukbab List for check gukbab location
    [SerializeField] private List<bool> gukbabList;

    //FireManager Script
    [Header("ÆÄÀÌ¾î ¸Å´ÏÀú")]
    [SerializeField] private FireManager fireManager;

    //Previous Count
    private int previousCount;

    //Index for Place Gukbab
    [Header("±¹¹äÀ» ¹èÄ¡ÇÏ±â À§ÇÑ ÀÎµ¦½º ¹è¿­")]
    [SerializeField] private Transform[] idxs;

    [Header("ÁÖ¸· ¾À")]
    [SerializeField] private JumakScene jumakScene;

    void Start()
    {
        //Initialize Variables
        previousCount = 0;
    }

    void Update()
    {
        if (!jumakScene.pause)
        {
            //Check Gukbab Presence
            CheckGukbabPresence();

            // Compare GukbabCount 
            if (fireManager.gukbabCount != previousCount)
            {
                // Only Run When gukbab count is increased
                if (fireManager.gukbabCount > previousCount)
                {
                    // Get Next Index
                    int index = GetNextAvailableIndex();

                    // Can Add Gukbab
                    if (index != -1)
                    {
                        //If sold out want gukbab count below zero,
                        //change prefab to basegukbab
                        if (wantGukbabCount <= 0) gukbabPrefab = baseGukbabPrefab;

                        //Generate Gukbab and Place Empty Index
                        GameObject newGukbab = Instantiate(gukbabPrefab, idxs[index].position, Quaternion.identity);

                        //If Want gukbab count greater than zero, decrease count
                        if (wantGukbabCount > 0) wantGukbabCount--;

                        //Change parent of newgukbab object
                        newGukbab.transform.parent = transform;

                        //Place Index Convert true
                        gukbabList[index] = true;
                    }
                }
                //Update previousCount
                previousCount = fireManager.gukbabCount;
            }
        }

    }

    // Find Next Index
    int GetNextAvailableIndex()
    {
        // Find Empty Index
        for (int i = 0; i < gukbabList.Count; i++)
        {
            if (!gukbabList[i]) 
            {
                //Return Empty Index
                return i;
            }
        }

        //Return -1 when does not exist empty index
        return -1;
    }

    // Check Gukbab Presence
    void CheckGukbabPresence()
    {
        // Search Empty Index
        for (int i = 0; i < gukbabList.Count; i++)
        {
            //Get Coliders
            Collider2D[] colliders = Physics2D.OverlapPointAll(idxs[i].transform.position);

            //Temp Variable
            bool gukbapPresent = false;
            
            //Search Colliders
            foreach (Collider2D collider in colliders)
            {
                //Detect Gukbab Convert to true
                if (collider.gameObject.tag.Contains("Gukbab"))
                {
                    gukbapPresent = true;
                    break;
                }
            }

            //Update GukbabList
            gukbabList[i] = gukbapPresent;
        }
    }
}
