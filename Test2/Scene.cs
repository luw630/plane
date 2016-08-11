using System.Windows.Controls;


public class Scene
{
    Fortrees fortrees1 = new Fortrees();
    Fortrees fortrees2 = new Fortrees();

    public void Init(Canvas root, int ticks)
    {
        fortrees1.Init(root, fortrees2, ticks);
        fortrees2.Init(root, fortrees1, ticks, true);
    }
    public void Update(int ticks)
    {
        fortrees1.Update(ticks);
        fortrees2.Update(ticks);
    }
}
