public class HugeAsteroid : Asteroid
{
    public override void SetCollidedWith()
    {
        // Do not disable the visuals or collider of the huge asteroid
        hasBeenCollidedWith = true;
    }
}