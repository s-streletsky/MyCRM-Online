BEGIN TRANSACTION;

-- Table: Clients
CREATE TABLE Clients (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Date TEXT NOT NULL, Name TEXT NOT NULL, Nickname TEXT, Phone TEXT, Email TEXT, CountryId INTEGER REFERENCES Countries (Id), City TEXT, Address TEXT, ShippingMethodId INTEGER REFERENCES ShippingMethods (Id), PostalCode TEXT, Notes TEXT);

-- Table: Countries
CREATE TABLE Countries (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Name TEXT NOT NULL);

-- Table: Currencies
CREATE TABLE Currencies (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, code TEXT);

-- Table: ExchangeRates
CREATE TABLE ExchangeRates (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, date TEXT, currency_id INTEGER REFERENCES currencies (id) NOT NULL, value REAL);

-- Table: Manufacturers
CREATE TABLE Manufacturers (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL);

-- Table: Orders
CREATE TABLE Orders (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, date TEXT NOT NULL, client_id INTEGER REFERENCES clients (id), status_id INTEGER REFERENCES OrderStatuses (id), notes TEXT);

-- Table: OrdersItems
CREATE TABLE OrdersItems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, order_id INTEGER REFERENCES orders (id) NOT NULL, stock_item_id INTEGER REFERENCES StockItems (id) NOT NULL, quantity REAL, price REAL, discount REAL, total REAL, profit REAL, expenses REAL, exchange_rate REAL);

-- Table: OrderStatuses
CREATE TABLE OrderStatuses (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL);

-- Table: Payments
CREATE TABLE Payments (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, date TEXT, client_id INTEGER REFERENCES clients (id) NOT NULL, order_id INTEGER REFERENCES Orders (id) NOT NULL, amount REAL NOT NULL, notes TEXT);

-- Table: ShippingMethods
CREATE TABLE ShippingMethods (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Name TEXT NOT NULL);

-- Table: StockArrivals
CREATE TABLE StockArrivals (id INTEGER PRIMARY KEY NOT NULL, date TEXT, stock_item_id INTEGER REFERENCES StockItems (id), quantity REAL);

-- Table: StockItems
CREATE TABLE StockItems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT NOT NULL, manufacturer_id INTEGER REFERENCES manufacturers (id), description TEXT, currency_id INTEGER REFERENCES currencies (id), purchase_price REAL NOT NULL, retail_price REAL NOT NULL, quantity REAL NOT NULL);

COMMIT TRANSACTION;