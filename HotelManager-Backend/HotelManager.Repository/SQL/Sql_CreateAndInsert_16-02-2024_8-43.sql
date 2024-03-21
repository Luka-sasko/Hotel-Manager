CREATE TABLE IF NOT EXISTS "Role" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS "User" (
    "Id" UUID PRIMARY KEY,
    "FirstName" VARCHAR(30) NOT NULL,
    "LastName" VARCHAR(30) NOT NULL,
    "Email" VARCHAR(80) NOT NULL UNIQUE,
	"Password" VARCHAR(50) NOT NULL,
    "Phone" VARCHAR(20),
	"RoleId" UUID NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	CONSTRAINT "FK_Role_User_RoleId" 
		FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id")
);

CREATE TABLE IF NOT EXISTS "RoomType" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "Description" TEXT NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS "Room" (
    "Id" UUID PRIMARY KEY,
    "Number" INTEGER NOT NULL UNIQUE,
    "BedCount" INTEGER NOT NULL,
    "Price" MONEY NOT NULL,
    "IsAvailable" BOOLEAN NOT NULL DEFAULT TRUE,
	"ImageUrl" TEXT NOT NULL,
	"TypeId" UUID NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	CONSTRAINT "FK_RoomType_Room_TypeId" 
		FOREIGN KEY ("TypeId") REFERENCES "RoomType" ("Id")
);

CREATE TABLE IF NOT EXISTS "Review" (
    "Id" UUID PRIMARY KEY,
	"Rating" INT NOT NULL,
    "Comment" TEXT,
    "UserId" UUID NOT NULL,
    "RoomId" UUID NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	CONSTRAINT "FK_User_Review_UserId"
    	FOREIGN KEY ("UserId") REFERENCES "User" ("Id"),
	CONSTRAINT "FK_Room_Review_RoomId"
    	FOREIGN KEY ("RoomId") REFERENCES "Room" ("Id")
);

CREATE TABLE IF NOT EXISTS "Reservation" (
    "Id" UUID PRIMARY KEY,
	"CheckInDate" TIMESTAMP NOT NULL,
    "CheckOutDate" TIMESTAMP NOT NULL,
    "PricePerNight" MONEY NOT NULL,
	"UserId" UUID NOT NULL,
    "RoomId" UUID NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT "FK_User_Reservation_UserId"
    	FOREIGN KEY ("UserId") REFERENCES "User" ("Id"),
	CONSTRAINT "FK_Room_Reservation_RoomId"
    	FOREIGN KEY ("RoomId") REFERENCES "Room" ("Id")
);

CREATE TABLE IF NOT EXISTS "Discount" (
    "Id" UUID PRIMARY KEY,
    "Code" VARCHAR(25) NOT NULL,
    "Percent" INTEGER NOT NULL,
    "ValidFrom" TIMESTAMP NOT NULL,
    "ValidTo" TIMESTAMP NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS "Invoice" (
    "Id" UUID PRIMARY KEY,
    "TotalPrice" MONEY NOT NULL,
    "IsPaid" BOOLEAN NOT NULL DEFAULT FALSE,
    "ReservationId" UUID NOT NULL,
	"DiscountId" UUID NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	CONSTRAINT "FK_Reservation_Invoice_ReservationId"
    	FOREIGN KEY ("ReservationId") REFERENCES "Reservation" ("Id"),
	CONSTRAINT "FK_Discount_Invoice_DiscountId"
    	FOREIGN KEY ("DiscountId") REFERENCES "Discount" ("Id")
);

CREATE TABLE IF NOT EXISTS "Service" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
	"Description" TEXT,
    "Price" MONEY NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS "InvoiceService" (
    "Id" UUID PRIMARY KEY,
    "NumberOfService" INTEGER NOT NULL,
	"InvoiceId" UUID NOT NULL,
    "ServiceId" UUID NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	CONSTRAINT "FK_Invoice_InvoiceService_InvoiceId"
    	FOREIGN KEY ("InvoiceId") REFERENCES "Invoice" ("Id"),
	CONSTRAINT "FK_Service_InvoiceService_ServiceId"
    	FOREIGN KEY ("ServiceId") REFERENCES "Service" ("Id")
);


CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
-- Role
INSERT INTO "Role" ("Id", "Name", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    ('a1d2b16b-f670-496a-beca-5bc0179b36ba', 'Admin', 'a1d2b16b-f670-496a-beca-5bc0179b36ba', 'a1d2b16b-f670-496a-beca-5bc0179b36ba', true),
    ('0929ab09-af84-4c14-9649-b812beb80b17', 'User', 'a1d2b16b-f670-496a-beca-5bc0179b36ba', 'a1d2b16b-f670-496a-beca-5bc0179b36ba', true)
ON CONFLICT ("Id") DO NOTHING;

-- User
INSERT INTO "User" ("Id", "FirstName", "LastName", "Email", "Password", "Phone", "RoleId", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    ('73dd2485-b158-420a-86ca-599c3abba0aa', 'John', 'Doe', 'john.doe@example.com', 'password', '123456789', (SELECT "Id" FROM "Role" WHERE "Name" = 'Admin'), '73dd2485-b158-420a-86ca-599c3abba0aa', '73dd2485-b158-420a-86ca-599c3abba0aa', true),
    (uuid_generate_v4(), 'Jane', 'Doe', 'jane.doe@example.com', 'password', '987654321', (SELECT "Id" FROM "Role" WHERE "Name" = 'User'), '73dd2485-b158-420a-86ca-599c3abba0aa', '73dd2485-b158-420a-86ca-599c3abba0aa', true)
ON CONFLICT ("Email") DO NOTHING;

-- RoomType
INSERT INTO "RoomType" ("Id", "Name", "Description", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    (uuid_generate_v4(), 'Standard', 'Standard room with basic amenities', (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true),
    (uuid_generate_v4(), 'Deluxe', 'Deluxe room with additional features', (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true);

-- Room
INSERT INTO "Room" ("Id", "Number", "BedCount", "Price", "IsAvailable", "ImageUrl", "TypeId", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    (uuid_generate_v4(), 1, 2, 100, true, 'url_to_image', (SELECT "Id" FROM "RoomType" WHERE "Name" = 'Standard'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true),
    (uuid_generate_v4(), 2, 3, 150, true, 'url_to_image', (SELECT "Id" FROM "RoomType" WHERE "Name" = 'Deluxe'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true)
ON CONFLICT ("Number") DO NOTHING;

-- Review
INSERT INTO "Review" ("Id", "Rating", "Comment", "UserId", "RoomId", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    (uuid_generate_v4(), 5, 'Great experience!', (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "Room" WHERE "Number" = 1), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true),
    (uuid_generate_v4(), 4, 'Comfortable stay', (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), (SELECT "Id" FROM "Room" WHERE "Number" = 2), (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), true);

-- Reservation
INSERT INTO "Reservation" ("Id", "CheckInDate", "CheckOutDate", "PricePerNight", "UserId", "RoomId", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    (uuid_generate_v4(), '2024-03-01', '2024-03-05', 100, (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "Room" WHERE "Number" = 1), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true),
    (uuid_generate_v4(), '2024-03-10', '2024-03-15', 150, (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), (SELECT "Id" FROM "Room" WHERE "Number" = 2), (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), true);

-- Discount
INSERT INTO "Discount" ("Id", "Code", "Percent", "ValidFrom", "ValidTo", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    (uuid_generate_v4(), 'DISCOUNT10', 10, '2024-01-01', '2024-12-31', (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true),
    (uuid_generate_v4(), 'DISCOUNT20', 20, '2024-06-01', '2024-06-30', (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true);

-- Invoice
INSERT INTO "Invoice" ("Id", "TotalPrice", "IsPaid", "ReservationId", "DiscountId", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    (uuid_generate_v4(), 400, true, (SELECT "Id" FROM "Reservation" WHERE "CheckInDate" = '2024-03-01'), (SELECT "Id" FROM "Discount" WHERE "Code" = 'DISCOUNT10'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true),
    (uuid_generate_v4(), 300, false, (SELECT "Id" FROM "Reservation" WHERE "CheckInDate" = '2024-03-10'), (SELECT "Id" FROM "Discount" WHERE "Code" = 'DISCOUNT20'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), true);

-- Service
INSERT INTO "Service" ("Id", "Name", "Description", "Price", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    (uuid_generate_v4(), 'WiFi', 'High-speed internet access', 10.00, (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true),
    (uuid_generate_v4(), 'Breakfast', 'Continental breakfast', 20.00, (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true);

-- InvoiceService
INSERT INTO "InvoiceService" ("Id", "NumberOfService", "InvoiceId", "ServiceId", "CreatedBy", "UpdatedBy", "IsActive")
VALUES
    (uuid_generate_v4(), 2, (SELECT "Id" FROM "Invoice" WHERE "ReservationId"  = (SELECT "Id" FROM "Reservation" WHERE "CheckInDate" = '2024-03-01')), (SELECT "Id" FROM "Service" WHERE "Name" = 'WiFi'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'John'), true),
    (uuid_generate_v4(), 1, (SELECT "Id" FROM "Invoice" WHERE "ReservationId" = (SELECT "Id" FROM "Reservation" WHERE "CheckInDate" = '2024-03-10')), (SELECT "Id" FROM "Service" WHERE "Name" = 'Breakfast'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), (SELECT "Id" FROM "User" WHERE "FirstName" = 'Jane'), true);


