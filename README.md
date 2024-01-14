# Реализация авторизации через Keycloak в REST API
## Описание
Этот проект расширяет функциональность предыдущей лабораторной работы 
по созданию REST API, добавляя механизм авторизации через Keycloak. 
Для этого был создан класс AuthHandler, который 
обрабатывает и проверяет JWT токены, полученные от сервера Keycloak.

## Функционал
- Проверка наличия заголовка Authorization в запросе.
- Извлечение значения токена из заголовка.
- Получение списка прав из кэша или запрос к Keycloak для получения информации о пользователе.
- Разбор токена и проверка списка прав.
- Добавлены декораторы, атрибуты и аналогичные средства 
в ASP.NET для ограничения запросов с заголовками POST, PUT, DELETE 
только от авторизованных пользователей с указанной ролью.

## Add Task
![](/Screens/Add_task.png)
## Add Task (No access)
![](/Screens/Add_task_no_access.png)

## Change Task
![](/Screens/Change_task.png)
## Change Task (No access)
![](/Screens/Change_task_no_access.png)

## Create User
![](/Screens/Add_user.png)
## Create User (No access)
![](/Screens/Add_user_no_access.png)

## Delete Task
![](/Screens/Delete_task.png)
## Delete Task (No access)
![](/Screens/Delete_task_no_access.png)

## Get Task User
![](/Screens/Get_list_tasks_by_user_id.png)
## Get Task User (No access)
![](/Screens/Get_list_tasks_by_user_no_access.png)

## Request to keycloak for get token
![](/Screens/Request_keycloak_for_get_token.png)

