using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlBossFightScript : MonoBehaviour 
{
	[Header("Materials to change the appearance of the owls")]
	public Material realOwlMaterial;
	public Material fakeOwlMaterial;

	private Transform[] owlsTransforms;
	private BoxCollider[] owlsColliders;
	private MeshRenderer[] owlsMeshRenderer;
	private Transform[] batsTransforms;

	[Header("Owl Boss Parameters")]
	[Tooltip("A reference to the GameObject in the Environment layer.")]
	public GameObject owlBossDoor;
	[Tooltip("Number of times the player has to catch the real owl.")]
	public int numberOfLives = 3;
	[Tooltip("Time the real owl waits before moving to another spot.")]
	public float timeBeforeMovingOwls = 7.5f;
	private float timerBeforeMovingOwls;
	[Tooltip("The time it takes the owl to move to another spot.")]
	public float owlsMovementTime = 2.5f;
	private int realOwlIndex;

	private string playerTag;


	void Awake() 
	{
		// Find the boss bats.
		Transform batsParent = transform.parent.Find("Boss Bats Parent");
		batsTransforms = new Transform[batsParent.childCount];

		for (int i = 0; i < batsTransforms.Length; i++)
		{
			batsTransforms[i] = batsParent.GetChild(i);
		}


		// Initialize owl Lists.
		owlsTransforms = new Transform[transform.childCount];
		owlsMeshRenderer = new MeshRenderer[transform.childCount];
		owlsColliders = new BoxCollider[transform.childCount];

		for (int i = 0; i < transform.childCount; i++)
		{
			owlsTransforms[i] = transform.GetChild(i);
			owlsMeshRenderer[i] = owlsTransforms[i].GetComponent<MeshRenderer>();
			owlsColliders[i] = owlsTransforms[i].GetComponent<BoxCollider>();
		}

	}

	void Start()
	{
		playerTag = GlobalData.PlayerTag;

		StartBossFight();
	}
	
	void StartBossFight()
	{
		MoveRealOwl();
		EnableBats();
	}

	void FixedUpdate ()
	{
		if (numberOfLives > 0)
		{
			timerBeforeMovingOwls += Time.fixedDeltaTime;

			if (timerBeforeMovingOwls >= timeBeforeMovingOwls)
			{
				HideOwls();

				timerBeforeMovingOwls = -owlsMovementTime;
				Invoke("MoveRealOwl",owlsMovementTime);
			}
		}
	}

	void HideOwls()
	{
		for (int i = 0; i < owlsTransforms.Length; i++)
		{
			owlsTransforms[i].gameObject.SetActive(false);
		}
	}

	void MoveRealOwl()
	{
		realOwlIndex = Random.Range(0,owlsTransforms.Length);

		for (int i = 0; i < owlsTransforms.Length; i++)
		{
			owlsTransforms[i].gameObject.SetActive(true);

			if (i.Equals(realOwlIndex))
			{
				owlsMeshRenderer[i].material = realOwlMaterial;
			}
			else
			{
				owlsMeshRenderer[i].material = fakeOwlMaterial;
			}
		}
	}

	void DisableBats()
	{
		foreach(Transform bat in batsTransforms)
		{
			bat.gameObject.SetActive(false);
		}
	}
	void EnableBats()
	{
		foreach(Transform bat in batsTransforms)
		{
			bat.gameObject.SetActive(true);
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(playerTag))
		{
			for( int i = 0; i < owlsColliders.Length; i++ )
			{
				if (owlsColliders[i].bounds.Intersects(other.bounds))
				{
					if (realOwlIndex.Equals(i))
					{
						HideOwls();
						numberOfLives--;

						if (numberOfLives > 0)
						{
							timerBeforeMovingOwls = -owlsMovementTime;
							Invoke("MoveRealOwl",owlsMovementTime);
						}
						else
						{
							FinishBossFight();
						}

					}
					else
					{
						owlsTransforms[i].gameObject.SetActive(false);
					}
				}
			}
		}
	}


	void FinishBossFight()
	{
		DisableBats();
		owlBossDoor.SetActive(false);
		this.gameObject.SetActive(false);
	}
}
