using System;
using System.Collections.Generic;
using UnityEngine;

public static class SomeValue
{
#if UNITY_EDITOR
    public static string processName = Application.dataPath + "/Plugins/IfcToWexBim.exe";
#endif

    public static string fileName;
    public static string WexbimFileName => fileName + ".wexBIM";
    public static string BimFileName => fileName + ".ifc";


    public static IProjectData project;
    public static List<IProductData> products= new List<IProductData>();
    public static List<ISpatialData> spatialStructures = new List<ISpatialData>();
}
