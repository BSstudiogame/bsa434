using UnityEngine;
using System.Collections.Generic;

public class ChestController : MonoBehaviour
{
    public GameObject[] lootPrefabs; // ������ �������� ����
    public Transform lootSpawnPoint; // ����� ������ ����
    public bool isOpen = false; // ����, �����������, ������ �� ����
    public int maxLootItems = 3; // ������������ ���������� ���������� ���������

    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;

            // �������� ��������� ���
            for (int i = 0; i < Random.Range(1, maxLootItems + 1); i++) // �������� �� 1 �� maxLootItems ���������
            {
                int randomIndex = Random.Range(0, lootPrefabs.Length);
                Instantiate(lootPrefabs[randomIndex], lootSpawnPoint.position, Quaternion.identity);
            }

            // �������� �������� (�������������)
            //  animator.SetTrigger("Open"); // ��������� �������� �������� 

            // �������� ������ (�������������)
            // AudioSource.PlayOneShot(openSound); // ����������� ���� ��������
        }
    }
}

public class LootableItem : MonoBehaviour
{
    public Inventory playerInventory; // ������ �� ������ ��������� ������

    private void OnTriggerEnter(Collider other)
    {
        // ���� ����� ��������������� � ���������
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            // ��������� ������� � ���������
            playerInventory.AddItem(gameObject);

            // ���������� �������
            Destroy(gameObject);
        }
    }
}

public class Inventory : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>(); // ������ ��������� � ���������

    public void AddItem(GameObject item)
    {
        items.Add(item);
        // �������������� ������:
        // - ����������� �������� � UI-���������
        // - �������� ������� ���������� � ���������
    }
}

public class PlayerController : MonoBehaviour
{
    public Inventory inventory; // ������ �� ������ ��������� 
    public float interactionDistance = 2f; // ��������� ��������������

    void Update()
    {
        // �������������� � �������
        if (Input.GetKeyDown(KeyCode.E)) // ������� E ��� ��������������
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
            {
                if (hit.collider.TryGetComponent<ChestController>(out ChestController chest))
                {
                    chest.OpenChest();
                }
            }
        }
    }
}