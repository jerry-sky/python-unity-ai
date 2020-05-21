using UnityEngine;

[System.Serializable]
public class EntityModel
{
    public bool alive;
    public int x;
    public int y;

    public Vector2 Pos()
    {
        return new Vector2(x, y);
    }

    public EntityModel(int x, int y, bool alive)
    {
        this.x = x;
        this.y = y;
        this.alive = alive;
    }
}
