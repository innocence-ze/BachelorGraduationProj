using System;
using System.Collections.Generic;
using UnityEngine;

public static class SomeValue
{
    public static string processName = Application.dataPath + "/Plugins/IfcToWexBim.exe";


    public static string fileName = default;
    public static string WexbimFileName => fileName + ".wexBIM";
    public static string BimFileName => fileName + ".ifc";


    public static IProjectData project;
    public static List<IElementData> Elements = new List<IElementData>();
    public static List<ISpatialData> spatialStructures = new List<ISpatialData>();

}
