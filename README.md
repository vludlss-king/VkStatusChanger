**Версия сервиса:**
1.0.1 (прототип)

**История изменений:**
[Посмотреть](https://github.com/vludlss-king/VkStatusChanger/blob/main/CHANGELOG.md)

**Краткое описание:**
Консольный сервис для автоматической смены статуса на страничке ВКонтакте по расписанию.
Статусы можно менять каждые N секунд либо по графику дат.

**Версия VK API:**
5.131

**Применённые библиотеки:**
- CommandLineParser
- Microsoft.Extensions.Hosting
- Quartz
- Serilog
- VkNet

**Тестирование:**
- xUnit
- Moq
- FluentAssertions

**Авторизация:**
Чтобы приложение выполняло свою работу корректно, ему необходимо передать параметр **AccessToken**, который можно получить при создании Standalone-приложения ВКонтакте. Передавать через команду:

```.\VkStatusChanger.Worker.exe settings auth set --access-token {value}```

**Команды: (Windows PowerShell)**
1. Сбросить настройки

```.\VkStatusChanger.Worker.exe settings reset```

2. Выбрать тип настроек (Every либо Schedule)

```.\VkStatusChanger.Worker.exe settings type set --settings-type {value}```

3. Просмотреть текущий тип настроек

```.\VkStatusChanger.Worker.exe settings type show```

4. Установить ключ доступа для VK API

```.\VkStatusChanger.Worker.exe settings auth set --access-token {value}```

5. Просмотреть текущий токен доступа для VK API

```.\VkStatusChanger.Worker.exe settings auth show```

6. Установить смену статусов по секундам

```.\VkStatusChanger.Worker.exe settings every set --statuses-texts "text1","text2","text3" --seconds 30```

7. Просмотреть текущие настройки статусов по секундам

```.\VkStatusChanger.Worker.exe settings every show```

8. Добавить расписание на смену статуса

```.\VkStatusChanger.Worker.exe settings schedule add --status-text {value} --date {value} --time {value}```

9. Редактировать расписание на смену статуса

```.\VkStatusChanger.Worker.exe settings schedule edit --id 1 --status-text {value} --date {value} --time {value}```

10. Удалить расписание на смену статуса

```.\VkStatusChanger.Worker.exe settings schedule remove --id 1```

11. Показать текущий список расписаний

```.\VkStatusChanger.Worker.exe settings schedule list```

12. Запустить

```.\VkStatusChanger.Worker.exe start```

