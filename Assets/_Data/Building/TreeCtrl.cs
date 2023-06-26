using UnityEngine;

public class TreeCtrl : BinBeha
{
    public LogwoodGenerator logwoodGenerator;
    public TreeLevel treeLevel;
    public WorkerCtrl choper;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTreeLevel();
        this.LoadLogwoodGenerator();
    }

    protected virtual void LoadTreeLevel()
    {
        if (this.treeLevel != null) return;
        this.treeLevel = GetComponent<TreeLevel>();
        Debug.LogWarning(transform.name + " LoadTreeLevel", gameObject);
    }

    protected virtual void LoadLogwoodGenerator()
    {
        if (this.logwoodGenerator != null) return;
        this.logwoodGenerator = GetComponent<LogwoodGenerator>();
        Debug.Log(transform.name + " LoadLogwoodGenerator", gameObject);
    }
}
