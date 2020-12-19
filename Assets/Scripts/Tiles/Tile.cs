using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public enum Direction
{
    X, Z
}

public class Tile : MonoBehaviour
{
    public static Tile CurrentTile;
    public static Tile PreviousTile;

    public static Action OnTilePlaced;

    public Direction Direction;
    
    [SerializeField] private AudioClip[] splitSounds = null;
    [SerializeField] private AudioClip fitSound = null;

    [SerializeField] private float moveSpeed = 2f;
   // [SerializeField] private GameObject idealHit = null;
    //[SerializeField] private ParticleSystem extendFX = null;
    private int ltId;
    
    private void OnEnable()
    {
        if (PreviousTile == null)
            return;

        CurrentTile = this;
        transform.localScale = 
            new Vector3(PreviousTile.transform.localScale.x, transform.localScale.y, PreviousTile.transform.localScale.z);
    }

    public void ApplyMovement(Direction dir)
    {
        Direction = dir;
        if (Direction == Direction.X)
            ltId = LeanTween.moveX(gameObject, 1.3f, moveSpeed).setLoopPingPong().id;
        else
            ltId = LeanTween.moveZ(gameObject, 1.3f, moveSpeed).setLoopPingPong().id;
    }

    public void Stop()
    {
        moveSpeed = 0;
        LeanTween.cancel(ltId);
        float accuracy = CalculatePlacementAccuracy();
        //if (accuracy >= -0.06f && accuracy <= 0.06f)
        if (accuracy >= -GameManager.Instance.targetAccuracy && accuracy <= GameManager.Instance.targetAccuracy)
            FitTile();
        else
            PlaceTile(accuracy);
        GameStatus.Score++;
    }
    
    private float CalculatePlacementAccuracy()
    {
        return Direction == Direction.X ?
            transform.position.x - PreviousTile.transform.position.x :
            transform.position.z - PreviousTile.transform.position.z;
    }

    private void FitTile()
    {
        GameStatus.Combo++;
        Vector3 p = PreviousTile.transform.position;
        transform.position = new Vector3(p.x, transform.position.y, p.z);
        PreviousTile = this;
        // LeanTween.alpha(idealHit, 0, .5f)
        //     .setFrom(0.9f)
        //     .setOnComplete(() => { idealHit.SetActive(false); });
        float targetPitch = 0.9f + (GameStatus.Combo > 7 ? 7 : GameStatus.Combo) * 0.05f;
        SoundManager.Instance.Vibrate(30);
        SoundManager.Instance.PlaySoundWithPitch(fitSound, targetPitch, .5f);
        // TOFIX - > HARDCODED VALUE
        int roll = UnityEngine.Random.Range(0, 11);
        if (roll <= GameStatus.Combo)
            GameStatus.Money += GameStatus.Combo;
        if (GameStatus.Combo >= 7)
            StartCoroutine(ExtendBlock(1.1f, .08f));
        else
            OnTilePlaced?.Invoke();
    }

    private void PlaceTile(float accuracy)
    {
        float maxFault = Direction == Direction.X
            ? PreviousTile.transform.localScale.x
            : PreviousTile.transform.localScale.z;
        if (Mathf.Abs(accuracy) >= maxFault)
        {
            gameObject.AddComponent<Rigidbody>();
            GameManager.Instance.EndGame();
            return;
        }
        SplitTile(accuracy);
        PreviousTile = this;
        OnTilePlaced?.Invoke();
    }
    
    private void SplitTile(float accuracy)
    {
        GameStatus.Combo = 0;
        SoundManager.Instance.PlaySound(splitSounds[UnityEngine.Random.Range(0, splitSounds.Length)], 0.6f);
        float splitDirection = accuracy > 0 ? 1f : -1f;
        float newScale = Direction == Direction.X ? 
            PreviousTile.transform.localScale.x - Mathf.Abs(accuracy) :
            PreviousTile.transform.localScale.z - Mathf.Abs(accuracy);
        float newPos = Direction == Direction.X ?
            PreviousTile.transform.position.x + (accuracy / 2f) :
            PreviousTile.transform.position.z + (accuracy / 2f);
        float partScale = Direction == Direction.X ?
            transform.localScale.x - newScale :
            transform.localScale.z - newScale;
        transform.localScale = new Vector3
        (
            Direction == Direction.X ? newScale : transform.localScale.x,
            transform.localScale.y,
            Direction == Direction.X ? transform.localScale.z : newScale
        );
        transform.position = new Vector3
        (
            Direction == Direction.X ? newPos : transform.position.x,
            transform.position.y,
            Direction == Direction.X ? transform.position.z : newPos
        );
        float edge = Direction == Direction.X ? 
            transform.position.x + (newScale / 2f * splitDirection) :
            transform.position.z + (newScale / 2f * splitDirection);
        float partPos = edge + partScale / 2f * splitDirection;
        CreateSplittedPart(partScale, partPos);
    }

    private void CreateSplittedPart(float partScale, float partPos)
    {
        GameObject part = Instantiate(gameObject);
        //GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);
        part.transform.localScale = new Vector3
        (
            Direction == Direction.X ? partScale : transform.localScale.x,
            transform.localScale.y,
            Direction == Direction.X ? transform.localScale.z : partScale
        );
        part.transform.position = new Vector3
        (
            Direction == Direction.X ? partPos : transform.position.x,
            transform.position.y,
            Direction == Direction.X ? transform.position.z : partPos
        );
        part.AddComponent<Rigidbody>();
        //part.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
       // Destroy(part, 4f);
    }

    IEnumerator ExtendBlock(float multiplier, float time)
    {
        GameStatus.Combo = 0;
        //extendFX.Play();
        Vector3 s = transform.localScale;
        Vector3 t = new Vector3(s.x * multiplier, s.y, s.z * multiplier);
        float elapsed = 0;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(s, t, elapsed / time);
            yield return null;
        }
        OnTilePlaced?.Invoke();
    }

}
