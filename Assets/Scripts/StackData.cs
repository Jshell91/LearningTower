
using System;
using System.Collections.Generic;

[System.Serializable]
public class StackData
{
    public List<Block> Blocks { get; set; }

    public StackData(List<Block> blocks)
    {
        Blocks = blocks;
    }

    public StackData()
    {
        Blocks = new List<Block>();
    }
}

[System.Serializable]
public class Block : IComparable<Block>
{
    public int id { get; set; }
    public string subject { get; set; }
    public string grade { get; set; }
    public int mastery { get; set; }
    public string domainid { get; set; }
    public string domain { get; set; }
    public string cluster { get; set; }
    public string standardid { get; set; }
    public string standarddescription { get; set; }

    // Implementing this CompareTo, we can control the behavior or List.Sort().
    public int CompareTo(Block other)
    {
        // First we compare if domains are equal.
        if (domain != other.domain) return domain.CompareTo(other.domain);
        // Then we compare cluster info.
        if (cluster != other.cluster) return cluster.CompareTo(other.cluster);
        // And last we compare the standard ID.
        return standardid.CompareTo(other.standardid);
    }
}

