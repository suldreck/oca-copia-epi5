using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stoneStorage : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //create one stone ofr each placeholder spot
        for (int i = 0; i < this.transform.childCount; i++)
        {//instantiate a new copy of the stone prefavb
            GameObject thestone = Instantiate(StonePrefab);
            thestone.GetComponent<PlayerStone>().StartingTile = this.StartingTile;
            thestone.GetComponent<PlayerStone>().myStoneStorage   =this;
            AddStoneToStorage(Instantiate(StonePrefab), this.transform.GetChild(i));
        }
    }
    public GameObject StonePrefab;

    public Tile StartingTile;
   public  void AddStoneToStorage(GameObject theStone, Transform thePlaceholder = null)
    {
        //find the first empty placeholder
        if (thePlaceholder == null)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Transform p = this.transform.GetChild(i);
                if (p.childCount == 0)
                {
                    thePlaceholder = p;
                    break;
                }
            }
            if (thePlaceholder == null)
            {
                Debug.LogError("we are trying to ad a stone but we dont have empty place. ");
            }
        }

        //patent the stone to the placeholder
        theStone.transform.SetParent(thePlaceholder);
        //reset the stones local position to 0,0,0
        theStone.transform.localPosition = Vector3.zero;
    }
    void Update()
    {

    }
}
