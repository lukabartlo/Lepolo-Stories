using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class AgentData 
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] Dictionary<EDirection, Sprite> sprites;

    [Range(0f, 1f)]   
    [SerializeField] private float madness;

    //new line
    public Vector3 lastDirection;
    public bool needToChangeSpriteToPray = false;

    public Roles role;

    public void AssignSprites(ref Dictionary<EDirection, Sprite> _sprites, SpriteRenderer _spriteRenderer)
    {
        sprites = _sprites;
        spriteRenderer = _spriteRenderer;
    }

    //public void SetSprite(Vector2 direction)
    //{
    //    if (direction.y < 0) {
    //        spriteRenderer.sprite = sprites[EDirection.South];
    //        return;
    //    } else if (direction.y > 0){
    //        spriteRenderer.sprite = sprites[EDirection.North];
    //        return;
    //    } else if (direction.x < 0) {
    //        spriteRenderer.sprite = sprites[EDirection.West];
    //        return;
    //    } else if (direction.x > 0) {
    //        spriteRenderer.sprite = sprites[EDirection.East];
    //    }
    //}

    public void SetSprite(Vector3 direction)
    {
        lastDirection = direction;

        if (madness < 0.5f)
        {
            if (direction.x > -0.5 && direction.x < 0.5 && direction.z < 0) {
                spriteRenderer.sprite = sprites[EDirection.South];

                return;
            } else if (direction.x > -0.5 && direction.x < 0.5 && direction.z > 0) {
                spriteRenderer.sprite = sprites[EDirection.North];
                return;
            } else if (direction.x < -0.5) {
                spriteRenderer.sprite = sprites[EDirection.West];
                return;
            } else if (direction.x > 0.5) {
                spriteRenderer.sprite = sprites[EDirection.East];
            }
        }
        else
        {
            if ( madness > 0.99f)
            {
              
                if (direction.x > -0.5 && direction.x < 0.5 && direction.z < 0)
                {
                    spriteRenderer.sprite = sprites[EDirection.SouthCrazy];
                    return;
                }
                else if (direction.x > -0.5 && direction.x < 0.5 && direction.z > 0)
                {
                    spriteRenderer.sprite = sprites[EDirection.North];
                    return;
                }
                else if (direction.x < -0.5)
                {
                    spriteRenderer.sprite = sprites[EDirection.WestCrazy];
                    return;
                }
                else if (direction.x > 0.5)
                {
                    spriteRenderer.sprite = sprites[EDirection.EastCrazy];
                }
            }
            else
            {
                if (direction.x > -0.5 && direction.x < 0.5 && direction.z < 0)
                {
                    spriteRenderer.sprite = sprites[EDirection.SouthHalfCrazy];
                    return;
                }
                else if (direction.x > -0.5 && direction.x < 0.5 && direction.z > 0)
                {
                    spriteRenderer.sprite = sprites[EDirection.North];
                    return;
                }
                else if (direction.x < -0.5)
                {
                    spriteRenderer.sprite = sprites[EDirection.WestHalfCrazy];
                    return;
                }
                else if (direction.x > 0.5)
                {
                    spriteRenderer.sprite = sprites[EDirection.EastHalfCrazy];
                }
            }
        }
    }

    public void SetSpriteAction()
    {
        if (needToChangeSpriteToPray)
        {
            if (lastDirection.x < 0)
            {
                spriteRenderer.sprite = sprites[EDirection.PrayLeft];
            }
            else
            {
                spriteRenderer.sprite = sprites[EDirection.PrayRight];
            }
        }
    }

    public void SetHealth(int newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, maxHealth);
    }

    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMadness(float newMadness)
    {
        madness = Mathf.Clamp01(newMadness);
    }

    public float GetMadness()
    {
        return madness;
    }
}

