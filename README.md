Консольный сервис для автоматической смены статуса на страничке ВКонтакте.

Статусы можно менять каждые N секунд либо по графику дат.

Пример настройки settings.json
```
{
    "Every": {
        "StatusesTexts": [
            "Статус (автосмена 1)",
            "Статус (автосмена 2)",
            "Статус (автосмена 3)"
        ],
        "Seconds": 60
    },
    "Schedule": {
        "Items": [
            {
                "StatusText": "Статус (автосмена 1)",
                "Date": "2024-01-11",
                "Time": "05:22:00"
            },
            {
                "StatusText": "Статус (автосмена 2)",
                "Date": "2024-01-11",
                "Time": "05:16:30"
            },
            {
                "StatusText": "Статус (автосмена 3)",
                "Date": "2024-01-11",
                "Time": "05:17:00"
            }
        ]
    }
}
```

Заполнить можно только одно из двух полей: Every либо Schedule.

Every.StatusesTexts хранит массив статусов, которые будут меняться по очердеи каждые Every.Seconds секунд.

Schedule.Items хранит массив, каждый элемент которого содержит StatusText (какой статус установить), Date (в какую дату начать выполнение смены статуса), Time (в какое время начать по указанной Date дате)

Авторизация:
Чтобы приложение выполняло свою работу корректно, ему необходимо передать параметр AccessToken через аргументы командной строки при запуске сервиса:
--access-token {token}
