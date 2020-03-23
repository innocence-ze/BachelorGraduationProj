using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectController : MonoBehaviour
{
    public string fileName;
    List<GameObject> productObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        SomeValue.fileName = "G:/HUST/GraduationProject/test/" + fileName;

        //generate Model
        ReadWexBim.ReadWexbimFile(SomeValue.WexbimFileName);
        foreach(var p in MyBimGeomorty.products)
        {
            productObjects.Add(GenerateModel.GenerateProduct(p));
        }


        //generate spatial structure
        var projData = ReadBim.GetBimSpatialStructure(SomeValue.BimFileName);
        GenerateModel.GenerateSpatialStructure(projData);

        projData.ThisGameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
