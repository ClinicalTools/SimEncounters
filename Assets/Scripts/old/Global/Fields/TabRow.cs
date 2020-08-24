
public class TabRow : TabGroup
{
    protected override int Compare(TabGroup child1, TabGroup child2)
    {
        if (child1.transform.position.x < child2.transform.position.x)
            return -1;
        else if (child1.transform.position.x > child2.transform.position.x)
            return 1;
        else if (child1.transform.position.y > child2.transform.position.y)
            return -1;
        else if (child1.transform.position.y < child2.transform.position.y)
            return 1;
        else
            return 0;
    }
}
