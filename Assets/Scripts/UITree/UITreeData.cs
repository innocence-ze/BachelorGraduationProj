using System;
using System.Collections.Generic;

public class UITreeData
{
    #region members && constructor

    public UITreeData Parent;
    public List<UITreeData> ChildNodes;
    public int Layer = 0;
    public string Name = string.Empty;
    public int Label;

    public UITreeData() { }
    public UITreeData(string name, int label = 0, int layer = 0)
    {
        Label = label;
        Name = name;
        Layer = layer;
        Parent = null;
        ChildNodes = new List<UITreeData>();
    }
    public UITreeData(string name, List<UITreeData> childNodes, int label = 0, int layer = 0)
    {
        Label = label;
        Name = name;
        Parent = null;
        ChildNodes = childNodes;
        if (null == ChildNodes)
            ChildNodes = new List<UITreeData>();
        Layer = layer;
        ResetChildren(this);
    }

    #endregion

    #region operator methods

    public void SetParent(UITreeData parent)
    {
        if (null != this.Parent)
            this.Parent.RemoveChild(this);
        this.Parent = parent;
        this.Layer = parent.Layer + 1;
        parent.ChildNodes.Add(this);
        ResetChildren(this);
    }

    public void AddChild(UITreeData child)
    {
        AddChild(new UITreeData[] { child });
    }

    public void AddChild(IEnumerable<UITreeData> children)
    {
        foreach (UITreeData child in children)
            child.SetParent(this);
    }

    public void RemoveChild(UITreeData child)
    {
        RemoveChild(new UITreeData[] { child });
    }

    public void RemoveChild(IEnumerable<UITreeData> children)
    {
        foreach (UITreeData child in children)
        {
            for (int i = 0; i < ChildNodes.Count; i++)
                if (child == ChildNodes[i])
                {
                    ChildNodes.Remove(ChildNodes[i]);
                    break;
                }
        }
    }

    public void ClearChildren()
    {
        ChildNodes = null;
    }

    public UITreeData FindChildren(UITreeData uiTreeData)
    {
        return ChildNodes.Find(c => c.Equals(uiTreeData));
    }

    public UITreeData FindChildren(string n)
    {
        return ChildNodes.Find(c => c.Name == n);
    }

    public UITreeData FindChildren(int label)
    {
        return ChildNodes.Find(c => c.Label == label);
    }

    private void ResetChildren(UITreeData treeData)
    {
        for (int i = 0; i < treeData.ChildNodes.Count; i++)
        {
            UITreeData node = treeData.ChildNodes[i];
            node.Parent = treeData;
            node.Layer = treeData.Layer + 1;
            ResetChildren(node);
        }
    }

    #endregion

    #region override functions

    public override bool Equals(object obj)
    {
        UITreeData other = obj as UITreeData;
        if (null == other) return false;
        return other.Name.Equals(Name) && other.Layer.Equals(Layer) && other.Label.Equals(Label);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (Parent != null ? Parent.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (ChildNodes != null ? ChildNodes.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Layer;
            hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            return hashCode;
        }
    }

    #endregion
}
