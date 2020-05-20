# Echo

echo client/server:
- Echo-server состоит из комнат.
- Каждая комната имеет строковый идентификатор.
- При соединении с сервером клиент передает идентификатор комнаты к которой он подключается и идентификатор подключаемого клиента (осуществляет вход в комнату).
- Если комнаты не существуют на момент установки соединения, то должна быть создана новая комната с переданным идентификатором.
- После успешного входа в комнату клиент, с периодичностью раз в 100ms, начинает рассылать Echo-сообщения содержащие текст.
- Echo-сообщение из комнаты рассылается всем клиентам находящимся в ней.
- Если в комнату не поступало Echo-сообщений в течении 1 минуты, комната удаляется с сервера.