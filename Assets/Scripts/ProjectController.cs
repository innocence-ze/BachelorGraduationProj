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
        ConstantValue.fileName = "G:/HUST/GraduationProject/test/" + fileName;
        ReadWexBim.ReadWexbimFile(ConstantValue.WexbimFileName);
        foreach(var p in ReadWexBim.products)
        {
            productObjects.Add(GenerateModel.GenerateProduct(p));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
