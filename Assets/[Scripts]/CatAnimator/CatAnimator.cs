using System;
using UnityEngine;

public class CatAnimator : MonoBehaviour
{
    private static readonly int CatSlapTrigger = Animator.StringToHash("CatSlap");
    public Action CatSlapComplete;
    [SerializeField]
    private PlayAreaController playController;
    [SerializeField]
    private PowerupPack powerupPackController;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Vector2 leftOffset;
    [SerializeField]
    private Vector2 rightOffset;
    [SerializeField]
    AudioClip catSlapAudio;

    // Start is called before the first frame update
    void Start()
    {
        playController.TileSpawned += OnTileSpawned;
        powerupPackController.PowerPackEarned += OnPackGained;
    }

    private void OnTileSpawned(Tile tile)
    {
        if (tile.playerAdded) return;
        PlayAnimation(tile.GetComponent<RectTransform>().position);
    }

    private void OnPackGained()
    {
        Vector2 targetPos = powerupPackController.gameObject.GetComponent<RectTransform>().position;
        Vector2 customPos = new Vector2(targetPos.x+0.9f, targetPos.y+1.7f);
        PlayAnimation(customPos);
    }

    private void PlayAnimation(Vector2 targetPosition)
    {
        // Figure out if tile position is left or right of the screen
        if (targetPosition.x < 0)
        {
            Vector3 scale = animator.transform.localScale;
            if (scale.x > 0)
            {
                scale.x *= -1;
            }
            animator.transform.localScale = scale;
            animator.transform.position = targetPosition + leftOffset;
        }
        else
        {
            Vector3 scale = animator.transform.localScale;
            if (scale.x < 0)
            {
                scale.x *= -1;
            }
            animator.transform.localScale = scale;
            animator.transform.position = targetPosition + rightOffset;
        }
        animator.Play(CatSlapTrigger, -1, 0f);
    }


    public void AnimationReachedTarget()
    {
        CatSlapComplete?.Invoke();
        SoundController.Instance.PlaySoundEffect(catSlapAudio, 3f);
    }
}
