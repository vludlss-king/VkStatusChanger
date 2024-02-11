**Версия сервиса:**
1.0.2 (прототип)

**История изменений:**
[Посмотреть](https://github.com/vludlss-king/VkStatusChanger/blob/main/CHANGELOG.md)

**Краткое описание:**
Консольный сервис для автоматической смены статуса на страничке ВКонтакте по расписанию.
Статусы можно менять каждые N секунд либо по графику дат.

**Версия VK API:**
5.131

**Фреймворк и язык:**
- .NET 7
- C# 11

**Применённые библиотеки:**
- CommandLineParser
- FluentValidation
- Quartz
- Serilog
- VkNet
- Scrutor
- Microsoft.Extensions.Hosting

**Тестирование:**
- xUnit
- Moq
- FluentAssertions

**Авторизация:**
Чтобы приложение выполняло свою работу корректно, ему необходимо передать токен **AccessToken**, который можно получить после создания Standalone-приложения ВКонтакте.
Токен можно получить при совершения запроса по ссылке:

```https://oauth.vk.com/authorize?client_id={идентификатор_твоего_приложения}&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=status&response_type=token&v={версия_api_вконтакте}```

Он вернётся в адресную строку браузера, если приложение было создано правильно и тебе были выданы права на доступ к "Статус".

Токен передавать через команду:

```.\VkStatusChanger.Worker.exe settings auth set --access-token {value}```

**Как запустить?**

Запустить можно через команду

```.\VkStatusChanger.Worker.exe start```

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

