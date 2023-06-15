Vovas Sosas, [2023/06/16 6:51]
Создайте базу данных MS sql server и обеспечьте хранение в базе данных:
1)услуги ОТК (наименование, стоимость, код услуги, срок выполнения, среднее отклонение);
2)данные клиентов (ЮЛ – название компании, адрес, ИНН, р/с, БИК, ФИО руководителя, ФИО контактного лица, телефон контактного лица, e-mail, пароль; для ФЛ – e-mail, пароль, ФИО, дата рождения, серия и номер паспорта, телефон);
3)заказ (дата создания, услуги, которые входят в заказ, статус заказа, статус услуги в заказе, время выполнения заказа (в часах), стоимость заказа);
4)оказанная услуга (услуга, когда и кем были проведены исследования);
5)данные лаборантов и контролеров ОТК (логин, пароль, ФИО, последняя дата и время входа, набор услуг, которые он может оказывать, проведенные исследования);
6)данные других сотрудников ОТК (логин, пароль, ФИО, последняя дата и время входа).
При организации хранения данных вам необходимо учесть запрет на полное удаление данных, реализовав возможность отправки данных в архив. Кроме того, необходимо учесть, что данные о заказе не могут быть отправлены в архив, если не выполнена хотя-бы одна услуга в заказе.

BithovenGPT, [2023/06/16 6:51]
CREATE TABLE Services (
  ServiceID int PRIMARY KEY,
  ServiceName varchar(50),
  Price money,
  ExecutionPeriod int,
  Deviation float
);
CREATE TABLE Clients (
  ClientID int PRIMARY KEY,
  CompanyName varchar(50),
  Address varchar(100),
  INN varchar(12),
  BankAccount varchar(20),
  BIC varchar(9),
  ChiefName varchar(50),
  ContactName varchar(50),
  ContactPhone varchar(20),
  ContactEmail varchar(50),
  Password varchar(50),
  ClientType varchar(5) CHECK (ClientType IN ('UL', 'FL'))
);
CREATE TABLE Orders (
  OrderID int PRIMARY KEY,
  CreatedDate datetime,
  Status varchar(20),
  CompletionTime int,
  TotalCost money,
  CONSTRAINT CK_Order CHECK (Status IN ('Created', 'In progress', 'Cancelled', 'Completed')),
  ClientID int FOREIGN KEY REFERENCES Clients(ClientID)
);
CREATE TABLE OrderServices (
  OrderID int FOREIGN KEY REFERENCES Orders(OrderID),
  ServiceID int FOREIGN KEY REFERENCES Services(ServiceID),
  Status varchar(20),
  CONSTRAINT CK_OrderService CHECK (Status IN ('Pending', 'In progress', 'Completed'))
);
CREATE TABLE CompletedServices (
  ServiceID int FOREIGN KEY REFERENCES Services(ServiceID),
  OrderID int FOREIGN KEY REFERENCES Orders(OrderID),
  LabTechID int FOREIGN KEY REFERENCES LabTechs(EmployeeID),
  ControlID int FOREIGN KEY REFERENCES Controllers(EmployeeID),
  CompletionDate datetime,
  CONSTRAINT unq_completed UNIQUE (ServiceID, OrderID)
);
CREATE TABLE Employees (
  EmployeeID int PRIMARY KEY,
  Login varchar(50),
  Password varchar(50),
  FullName varchar(50),
  LastLogin datetime
);
CREATE TABLE LabTechs (
  EmployeeID int FOREIGN KEY REFERENCES Employees(EmployeeID),
  ServicesAssigned varchar(100),
  CONSTRAINT CK_LabTech CHECK (ServicesAssigned IN ('Service 1', 'Service 2', 'Service 3'))
);
CREATE TABLE Controllers (
  EmployeeID int FOREIGN KEY REFERENCES Employees(EmployeeID),
  CompletedServices varchar(100)
);
CREATE TABLE ArchivedOrders (
  OrderID int FOREIGN KEY REFERENCES Orders(OrderID),
  ArchivedDate datetime,
  CONSTRAINT CK_ArchivedOrder CHECK (Status IN ('Cancelled', 'Completed'))
);
CREATE TABLE ArchivedServices (
  ServiceID int FOREIGN KEY REFERENCES Services(ServiceID),
  ArchivedDate datetime,
  CONSTRAINT CK_ArchivedService CHECK (Status IN ('Pending', 'Completed'))
);








