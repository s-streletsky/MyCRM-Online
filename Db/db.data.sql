BEGIN TRANSACTION;

INSERT INTO Countries (Id, Name) VALUES (1, 'Украина');
INSERT INTO Countries (Id, Name) VALUES (2, 'Молдова');
INSERT INTO Countries (Id, Name) VALUES (3, 'Польша');
INSERT INTO Currencies (id, code) VALUES (1, 'EUR');
INSERT INTO Currencies (id, code) VALUES (2, 'USD');
INSERT INTO Currencies (id, code) VALUES (3, 'UAH');
INSERT INTO ExchangeRates (id, date, currency_id, value) VALUES (1, '01.01.2000', 3, 1.0);
INSERT INTO OrderStatuses (id, name) VALUES (1, 'Готов');
INSERT INTO OrderStatuses (id, name) VALUES (2, 'К отправке');
INSERT INTO OrderStatuses (id, name) VALUES (3, 'Оплачен полностью');
INSERT INTO OrderStatuses (id, name) VALUES (4, 'НОВЫЙ');
INSERT INTO OrderStatuses (id, name) VALUES (5, 'Выставлен счёт');
INSERT INTO OrderStatuses (id, name) VALUES (6, 'Оплачен частично');
INSERT INTO OrderStatuses (id, name) VALUES (7, 'Отправлен');
INSERT INTO ShippingMethods (Id, Name) VALUES (1, 'Новая почта');
INSERT INTO ShippingMethods (Id, Name) VALUES (2, 'Укрпочта');
INSERT INTO ShippingMethods (Id, Name) VALUES (3, 'Самовывоз');

COMMIT TRANSACTION;