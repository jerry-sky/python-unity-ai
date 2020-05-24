using System;

[System.Serializable]
public class BoardUpdateModel
{
    public EntityModel[] rabbits;
    public EntityModel[] wolves;

    public string print()
    {
        string ret = String.Empty;
        if (!(rabbits is null))
        {
            ret = "Rabbits:";
            foreach (EntityModel entityModel in rabbits)
            {
                ret += "\n    " + entityModel.alive + ", " + entityModel.x + ", " + entityModel.y;
            }
        }
        if (!(wolves is null))
        {
            ret += "\nWolfs:";
            foreach (EntityModel entityModel in wolves)
            {
                ret += "\n    " + entityModel.alive + ", " + entityModel.x + ", " + entityModel.y;
            }
        }

        return ret;
    }

}
