# Задания 7.1.х

[Остальные задания курса](https://github.com/IgorBrv/xt_net_web "Остальные задания курса")

# Задание:

7.1.1.	USERS AND AWARDS

Реализовать бизнес-логику, позволяющую работать со списком пользователей (User: Id, Name, DateOfBirth, Age): создавать и удалять их, а также запрашивать их перечень. Добавьте к логике сущность «Награда» (Award: Id, Title) и реализуйте соответствующие механизмы для добавления и запроса перечня наград. Между пользователями и наградами должна быть реализована связь многие-ко-многим (у каждого пользователя может быть несколько наград, а наградой, соответственно, может быть награждено сколько угодно пользователей). 
В качестве архитектурного шаблона применить трёхслойную архитектуру. Записи должны сохраняться на жёсткий диск в любом формате, к примеру, JSON. Для повышения производительности можно реализовать кэширование, но не обязательно.
Учтите, что использование слабого связывания в контексте трёхслойного приложения обязательно.

7.1.2.	UI

Добавить к созданной бизнес-логике «Пользователи и награды» из задания 6.1.1. визуальный интерфейс на основе ASP.NET Web Pages. 
Приложение должно позволять выполнять все те же действия, что и консольная версия.
 -	Ранее написанная архитектура не должна подвергаться изменениям;
 -	Допускается изменение ранее написанного кода с целью исправления ошибок;
 -	Работоспособность бизнес-логики не должна пострадать;

7.1.3.	CRUD

Расширить функциональность приложения до CRUD-полной: добавление, редактирование, удаление и просмотр перечня его сущностей. 
При удалении награды, выданной кому-либо, пользователю должен предоставляться выбор: удалить эту награду у всех пользователей или отказаться от запрошенной операции.
Необходимо добавить страницы и формочки, соответствующие действиям. Красотой вёрстки можете пренебречь, хотя базовая обработка ошибок и корректное отображение требуются.

7.1.4.	IMAGES *

Задание является опциональным
Реализовать возможность добавления к пользователям и наградам изображений, загружаемых пользователем самостоятельно. Предусмотреть корректное масштабирование загружаемых изображений, а также картинку-заглушку в случае отсутствия изображения.


![preview](resources/7.x.jpg)