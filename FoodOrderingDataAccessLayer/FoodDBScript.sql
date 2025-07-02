USE [master]
GO
IF (EXISTS (SELECT name FROM master.dbo.sysdatabases 
WHERE ('[' + name + ']' = N'FoodDB' OR name = N'FoodDB')))
DROP DATABASE FoodDB

CREATE DATABASE FoodDB;
GO
USE FoodDB;
GO

CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO Roles (RoleName) VALUES
('Admin'),
('Customer'),
('Support');


CREATE TABLE Users (
    FullName NVARCHAR(100) NOT NULL,
    Email  NVARCHAR(100) CONSTRAINT pk_EmailId PRIMARY KEY,
    Gender NVARCHAR(10) NOT NULL,
    [Password] NVARCHAR(255) NOT NULL,
    RoleId INT NOT NULL,
    PhoneNumber NVARCHAR(20), 
    [Address] NVARCHAR(255),
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

INSERT INTO Users(FullName, Email,Gender, [Password],RoleId, PhoneNumber, Address) VALUES
('Ramesh','ramesh031@admin.com','M','ramesh123',1,'7896325412','123 Oak Street'),
('Aarav Sharma', 'aarav.sharma@example.com','M', 'password123',2,'9876543210','456 pine street'),
('Meera Iyer', 'meera.iyer@example.com', 'F','pass456',2,'9123456780','Hyderabad'),
('Rajesh','rajesh1312@admin.com','M','rajesh1312',1,'7531598526','vizag'),
('Praneeth','praneethsurya031@gmail.com','M','Praneeth@123',2,'8317536029','Mysore'),
('Saahithi','saa1902@support.com','F','saa123',3,'7412369852','Nellore'),
('Rohan Das', 'rohan.das@example.com','M', 'qwerty789',2,'9988776655','Anathapur'),
('Suresh','suresh031@support.com','M','suresh123',3,'8521479634','Kadapa');

 CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL
);

INSERT INTO Categories (CategoryName) VALUES
('Starters'),
('Main Course'),
('Desserts'),
('Beverages');

CREATE TABLE Cuisine (
    CuisineId INT PRIMARY KEY IDENTITY(1,1),
    CuisineName NVARCHAR(100) NOT NULL UNIQUE
);

INSERT INTO Cuisine (CuisineName) VALUES
('Indian'),
('Chinese');


 CREATE TABLE MenuItems (
    MenuItemId INT PRIMARY KEY IDENTITY(1,1),
    CategoryId INT NOT NULL,
    CuisineId INT NOT NULL,
    ItemName NVARCHAR(100) NOT NULL UNIQUE,
    Price DECIMAL(10,2) NOT NULL,
    ImageUrl NVARCHAR(255) NOT NULL,
    IsVegetarian BIT NOT NULL DEFAULT 1, -- 0 for Non-Vegetarian, 1 for Vegetarian
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    FOREIGN KEY (CuisineId) REFERENCES Cuisine(CuisineId)
    
);

INSERT INTO MenuItems (CategoryId, CuisineId, ItemName, Price,ImageUrl) VALUES
(1, 2,'Spring Rolls', 120.00,'/Images/spring-rolls.jpg'),
(2, 1,'Butter Chicken', 250.00,'/Images/butter-chicken.jpg'),
(2, 1, 'Paneer Tikka Masala', 220.00,'/Images/panner-tikka-masala.jpg' ),
(3, 1, 'Gulab Jamun', 80.00, '/Images/gulab-jamun.jpg'),
(4, 1, 'Mango Lassi', 60.00, '/Images/mango-lassi.jpg');

UPDATE MenuItems SET IsVegetarian = 0 WHERE ItemName = 'Butter Chicken'; -- Non-Veg
-- All others are Veg by default


CREATE TABLE CartDetails (
    CartId INT  PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) NOT NULL ,
    MenuItemId INT NOT NULL,
    ItemName NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    Price DECIMAL(10, 2) NOT NULL DEFAULT 0
    FOREIGN KEY (Email) REFERENCES Users(Email),
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(MenuItemId),
   
);

INSERT INTO CartDetails (Email,MenuItemId,ItemName ,Quantity,Price) VALUES
('aarav.sharma@example.com', 2,'Butter Chicken', 1,250.00); -- Aarav orders Butter Chicken



CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) NOT NULL,
    MenuItemId INT NOT NULL,
     ItemName NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    Price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (Email) REFERENCES Users(Email),
    FOREIGN KEY ( MenuItemId) REFERENCES MenuItems( MenuItemId),     
);

INSERT INTO OrderItems (Email, Quantity, Price,MenuItemId,ItemName) VALUES

('aarav.sharma@example.com', 2, 60.00,4,'Mango Lassi');

GO

CREATE TABLE Issues
(
    IssueId INT PRIMARY KEY IDENTITY(1,1),
    OrderItemId INT NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    IssueDescription NVARCHAR(255) NOT NULL,
    IssueStatus NVARCHAR(50) NOT NULL DEFAULT 'Open',
    FOREIGN KEY (Email) REFERENCES Users(Email),
    FOREIGN KEY (OrderItemId)  REFERENCES OrderItems(OrderItemId)
);

INSERT INTO Issues (OrderItemId,Email,IssueDescription, IssueStatus) VALUES
(1,'aarav.sharma@example.com','Order not received', 'Open');



CREATE TABLE Ratings
(
	RatingId INT PRIMARY KEY IDENTITY(1,1),
	Email NVARCHAR(100) NOT NULL,
	MenuItemId INT NOT NULL,
    ItemName NVARCHAR(100) NOT NULL,
	RatingValue DECIMAL CHECK (RatingValue BETWEEN 1 AND 5),
    OrderItemId INT NOT NULL,
    FOREIGN KEY (OrderItemId) REFERENCES OrderItems(OrderItemId),
	FOREIGN KEY (Email) REFERENCES Users(Email),
	FOREIGN KEY (MenuItemId) REFERENCES MenuItems(MenuItemId)
);
CREATE TABLE Bookings (
  BookingId INT PRIMARY KEY IDENTITY,
  Email NVARCHAR(100) NOT NULL,
  BookingDate DATE,
  BookingTime TIME,
  Guests INT,
  CheckedIn BIT DEFAULT 0,
  FOREIGN KEY (Email) REFERENCES Users(Email),
);


select * from OrderItems    
select * from MenuItems
select * from Ratings
