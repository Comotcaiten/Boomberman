using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject bombPrefab;
    public int bombRemaining;
    public int bombAmount = 1;
    public float timeFuse = 2.0f;


    public List<Vector2> bombsPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bombRemaining = bombAmount;
    }

    // Update is called once per frame
    void Update()
    {
        // Chỉ cho phép đặt đúng số lượng bomb nếu số lượng bomb (bombRemaining hết thì phải đợi hồi)
        if (bombRemaining > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlaceBomb());
        }
    }

    IEnumerator PlaceBomb() 
    {
        Vector2 placePos = transform.position;
        placePos.x = MathF.Round(placePos.x);
        placePos.y = MathF.Round(placePos.y);

        // Kiểm tra xem ở vị trí hiện tại có một trái bomb chưa
        for (int i = 0; i < bombsPos.Count; i++) {
            // Nếu có thì không được đặt nữa
            if (bombsPos[i] == placePos) {
                yield break;
            }
        }

        // Lấy 1 trái bomb ra
        bombRemaining--;

        // Set thời gian nổ của các trái bomb về sau giống nhau
        bombPrefab.GetComponent<Bomb>().timeDestory = timeFuse;

        GameObject bombObj = Instantiate(bombPrefab, placePos, bombPrefab.transform.rotation);
        Bomb bomb = bombObj.GetComponent<Bomb>();

        // Lưu lại vị trí bomb thứ n vừa mới được đặt
        bombsPos.Add(placePos);
        Debug.Log("Before: " + bombsPos);

        // Đợi thời gian nổ của các trái bomb về sau (lấy giá trị thời gian từ chính nó thay vì từ chính controller)
        yield return new WaitForSeconds(bomb.timeDestory);

        // Hồi lại một trái sau khi trái lấy ra đã nổ
        bombRemaining++;
        
        // Hủy vị trí sau khi nổ xong.
        bombsPos.Remove(placePos);
        Debug.Log("After: " + bombsPos);

    }
}
