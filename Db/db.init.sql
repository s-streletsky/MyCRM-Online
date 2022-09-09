BEGIN TRANSACTION;

-- Table: Clients
CREATE TABLE Clients (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Date TEXT NOT NULL, Name TEXT NOT NULL, Nickname TEXT, Phone TEXT, Email TEXT, CountryId INTEGER REFERENCES Countries (Id), City TEXT, Address TEXT, ShippingMethodId INTEGER REFERENCES ShippingMethods (Id), PostalCode TEXT, Notes TEXT);

-- Table: Countries
CREATE TABLE Countries (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Name TEXT NOT NULL);
INSERT INTO Countries (Id, Name) VALUES (1, 'Украина');
INSERT INTO Countries (Id, Name) VALUES (2, 'Молдова');
INSERT INTO Countries (Id, Name) VALUES (3, 'Польша');

-- Table: Currencies
CREATE TABLE Currencies (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, code TEXT);
INSERT INTO Currencies (id, code) VALUES (1, 'EUR');
INSERT INTO Currencies (id, code) VALUES (2, 'USD');
INSERT INTO Currencies (id, code) VALUES (3, 'UAH');

-- Table: ExchangeRates
CREATE TABLE ExchangeRates (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, date TEXT, currency_id INTEGER REFERENCES currencies (id) NOT NULL, value REAL);
INSERT INTO ExchangeRates (id, date, currency_id, value) VALUES (1, '01.01.2000', 3, 1.0);

-- Table: Manufacturers
CREATE TABLE Manufacturers (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL);

-- Table: Orders
CREATE TABLE Orders (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, date TEXT NOT NULL, client_id INTEGER REFERENCES clients (id), status_id INTEGER REFERENCES OrderStatuses (id), notes TEXT);

-- Table: OrdersItems
CREATE TABLE OrdersItems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, order_id INTEGER REFERENCES orders (id) NOT NULL, stock_item_id INTEGER REFERENCES StockItems (id) NOT NULL, quantity REAL, price REAL, discount REAL, total REAL, profit REAL, expenses REAL, exchange_rate REAL);

-- Table: OrderStatuses
CREATE TABLE OrderStatuses (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL);
INSERT INTO OrderStatuses (id, name) VALUES (1, 'Готов');
INSERT INTO OrderStatuses (id, name) VALUES (2, 'К отправке');
INSERT INTO OrderStatuses (id, name) VALUES (3, 'Оплачен полностью');
INSERT INTO OrderStatuses (id, name) VALUES (4, 'НОВЫЙ');
INSERT INTO OrderStatuses (id, name) VALUES (5, 'Выставлен счёт');
INSERT INTO OrderStatuses (id, name) VALUES (6, 'Оплачен частично');
INSERT INTO OrderStatuses (id, name) VALUES (7, 'Отправлен');

-- Table: Payments
CREATE TABLE Payments (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, date TEXT, client_id INTEGER REFERENCES clients (id) NOT NULL, order_id INTEGER REFERENCES Orders (id) NOT NULL, amount REAL NOT NULL, notes TEXT);

-- Table: ShippingMethods
CREATE TABLE ShippingMethods (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Name TEXT NOT NULL);
INSERT INTO ShippingMethods (Id, Name) VALUES (1, 'Новая почта');
INSERT INTO ShippingMethods (Id, Name) VALUES (2, 'Укрпочта');
INSERT INTO ShippingMethods (Id, Name) VALUES (3, 'Самовывоз');

-- Table: StockArrivals
CREATE TABLE StockArrivals (id INTEGER PRIMARY KEY NOT NULL, date TEXT, stock_item_id INTEGER REFERENCES StockItems (id), quantity REAL);

-- Table: StockItems
CREATE TABLE StockItems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL, manufacturer_id INTEGER REFERENCES manufacturers (id), description TEXT, currency_id INTEGER REFERENCES currencies (id), purchase_price REAL NOT NULL, retail_price REAL NOT NULL, quantity REAL NOT NULL);

COMMIT TRANSACTION;