using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
	// ランダムで表示したいマテリアル情報
	[SerializeField] Material[] materials;

	private void Awake()
	{
		// 乱数設定
		int rand;
		rand = Random.Range(0, materials.Length);

		// マテリアルをランダムに設定
		GetComponent<SkinnedMeshRenderer>().material = new Material(materials[rand]);
	}


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}