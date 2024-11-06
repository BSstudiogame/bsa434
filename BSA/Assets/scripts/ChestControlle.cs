using UnityEngine;
using System.Collections.Generic;

public class ChestController : MonoBehaviour
{
    public GameObject[] lootPrefabs; // Массив префабов лута
    public Transform lootSpawnPoint; // Точка спавна лута
    public bool isOpen = false; // Флаг, указывающий, открыт ли ящик
    public int maxLootItems = 3; // Максимальное количество выпадающих предметов

    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;

            // Выпадаем случайный лут
            for (int i = 0; i < Random.Range(1, maxLootItems + 1); i++) // Выпадаем от 1 до maxLootItems предметов
            {
                int randomIndex = Random.Range(0, lootPrefabs.Length);
                Instantiate(lootPrefabs[randomIndex], lootSpawnPoint.position, Quaternion.identity);
            }

            // Анимация открытия (необязательно)
            //  animator.SetTrigger("Open"); // Запускаем анимацию открытия 

            // Звуковой эффект (необязательно)
            // AudioSource.PlayOneShot(openSound); // Проигрываем звук открытия
        }
    }
}

public class LootableItem : MonoBehaviour
{
    public Inventory playerInventory; // Ссылка на скрипт инвентаря игрока

    private void OnTriggerEnter(Collider other)
    {
        // Если игрок взаимодействует с предметом
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            // Добавляем предмет в инвентарь
            playerInventory.AddItem(gameObject);

            // Уничтожаем предмет
            Destroy(gameObject);
        }
    }
}

public class Inventory : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>(); // Список предметов в инвентаре

    public void AddItem(GameObject item)
    {
        items.Add(item);
        // Дополнительная логика:
        // - Отображение предмета в UI-инвентаре
        // - Создание эффекта добавления в инвентарь
    }
}

public class PlayerController : MonoBehaviour
{
    public Inventory inventory; // Ссылка на скрипт инвентаря 
    public float interactionDistance = 2f; // Дальность взаимодействия

    void Update()
    {
        // Взаимодействие с ящиками
        if (Input.GetKeyDown(KeyCode.E)) // Нажатие E для взаимодействия
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