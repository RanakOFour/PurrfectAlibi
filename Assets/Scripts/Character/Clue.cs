public class Clue
{
    public bool isKnown;
    private string Information;

    public Clue(string info)
    {
        isKnown = false;
        Information = info;
    }

    public string Peek()
    {
        return Information;
    }

    public string Discover()
    {
        if(!isKnown)
        {
            isKnown = true;
        }

        return Information;
    }
}