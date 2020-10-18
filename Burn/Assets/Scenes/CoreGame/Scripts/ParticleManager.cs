using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // TODO implement ParticleEffectType and a struct for each particle effect type
    public enum ParticleEffectType
    {
        Collect = 10,
        Collision = 20,
        DestructibleCollsion = 30,
        ChangeShip = 40,
        Explosion = 50
    }

    [SerializeField]
    protected ParticleEffect explosionPrefab;

    [SerializeField]
    protected ParticleEffect collisionPrefab;

    [SerializeField]
    protected ParticleEffect collectPrefab;

    [SerializeField]
    protected ParticleEffect destructableCollisionPrefab;

    [SerializeField]
    protected ParticleEffect changeShipEffectPrefab;

    [SerializeField]
    protected ParticleEffect asteroidExplosionPrefab;

    private readonly int changeShipAmount = 1;
    private ParticleEffect[] changeShipEffectPool;

    private int changeShipIndex;
    private readonly int collectAmount = 10;

    private int collectIndex;
    private ParticleEffect[] collectPool;
    private readonly int collisionAmount = 10;

    private int collisionIndex;
    private ParticleEffect[] collisionPool;

    private readonly int destructableAmount = 10;
    private ParticleEffect[] destructableCollisionPool;
    private int destructableIndex;

    private readonly int explosionAmount = 1;
    private int explosionIndex;
    private ParticleEffect[] explosionPool;

    private int asteroidExplosionIndex;
    private readonly int asteroidExplosionAmount = 10;
    private ParticleEffect[] asteroidExplosionPool;

    private ParticleEffect[] CreatePool(int count, ParticleEffect effect)
    {
        var pool = new ParticleEffect[count];
        for (var i = 0; i < count; ++i)
        {
            var obj = Instantiate(effect, transform, true);
            pool[i] = obj;
        }

        return pool;
    }

    private void Start()
    {
        explosionPool = CreatePool(explosionAmount, explosionPrefab);
        collisionPool = CreatePool(collisionAmount, collisionPrefab);
        collectPool = CreatePool(collectAmount, collectPrefab);
        destructableCollisionPool = CreatePool(destructableAmount, destructableCollisionPrefab);
        changeShipEffectPool = CreatePool(changeShipAmount, changeShipEffectPrefab);
        asteroidExplosionPool = CreatePool(asteroidExplosionAmount, asteroidExplosionPrefab);
    }

    protected void OnEnable()
    {
        CoreConnector.ParticleManager = this;
    }

    protected void OnDisable()
    {
        CoreConnector.ParticleManager = null;
    }

    // todo change it to be: ShowEffect(Effect.Collect, position)
    public void ShowCollect(Vector3 position)
    {
        var obj = collectPool[collectIndex];
        obj.Show(position);
        collectIndex++;
        collectIndex = collectIndex % collectAmount;
    }

    public void ShowExplosion(Vector3 position)
    {
        var obj = explosionPool[explosionIndex];
        obj.Show(position);
        explosionIndex++;
        explosionIndex = explosionIndex % explosionAmount;
    }

    public void ShowAsteroidExplosion(Vector3 position)
    {
        var obj = asteroidExplosionPool[asteroidExplosionIndex];
        obj.Show(position);
        asteroidExplosionIndex++;
        asteroidExplosionIndex = asteroidExplosionIndex % asteroidExplosionAmount;
    }

    public void ShowCollision(Vector3 position)
    {
        var obj = collisionPool[collisionIndex];
        obj.Show(position);
        collisionIndex++;
        collisionIndex = collisionIndex % collisionAmount;
    }

    public void ShowDestructableCollision(Vector3 position)
    {
        var obj = destructableCollisionPool[destructableIndex];
        obj.Show(position);
        destructableIndex++;
        destructableIndex = destructableIndex % collisionAmount;
    }

    public void ShowChangeShip(Vector3 position)
    {
        var obj = changeShipEffectPool[changeShipIndex];
        obj.Show(position);
        changeShipIndex++;
        changeShipIndex = changeShipIndex % changeShipAmount;
    }
}