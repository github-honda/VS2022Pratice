--
-- File generated with SQLiteStudio v3.2.1 on 週一 十月 28 10:12:14 2019
--
-- Text encoding used: UTF-8
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: AspNetRoles
DROP TABLE IF EXISTS AspNetRoles;
CREATE TABLE "AspNetRoles" (
	"Id"	TEXT NOT NULL,
	"Name"	TEXT NOT NULL,
	PRIMARY KEY("Id")
);

-- Table: AspNetUserClaims
DROP TABLE IF EXISTS AspNetUserClaims;
CREATE TABLE "AspNetUserClaims" (
	"Id"	INTEGER NOT NULL,
	"UserId"	TEXT NOT NULL,
	"ClaimType"	TEXT,
	"ClaimValue"	TEXT,
	PRIMARY KEY("Id")
);

-- Table: AspNetUserLogins
DROP TABLE IF EXISTS AspNetUserLogins;
CREATE TABLE "AspNetUserLogins" (
	"LoginProvider"	TEXT NOT NULL,
	"ProviderKey"	TEXT NOT NULL,
	"UserId"	TEXT NOT NULL,
	PRIMARY KEY("LoginProvider","ProviderKey","UserId")
);

-- Table: AspNetUserRoles
DROP TABLE IF EXISTS AspNetUserRoles;
CREATE TABLE "AspNetUserRoles" (
	"UserId"	TEXT NOT NULL,
	"RoleId"	TEXT NOT NULL,
	PRIMARY KEY("UserId","RoleId")
);

-- Table: AspNetUsers
DROP TABLE IF EXISTS AspNetUsers;
CREATE TABLE "AspNetUsers" (
	"Id"	TEXT NOT NULL,
	"Email"	TEXT,
	"EmailConfirmed"	INTEGER NOT NULL,
	"PasswordHash"	TEXT,
	"SecurityStamp"	TEXT,
	"PhoneNumber"	TEXT,
	"PhoneNumberConfirmed"	INTEGER NOT NULL,
	"TwoFactorEnabled"	INTEGER NOT NULL,
	"LockoutEndTateUtc"	TEXT,
	"LockoutEnabled"	INTEGER NOT NULL,
	"AccessFailedCount"	INTEGER NOT NULL,
	"UserName"	TEXT NOT NULL,
	PRIMARY KEY("Id")
);
INSERT INTO AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndTateUtc, LockoutEnabled, AccessFailedCount, UserName) VALUES ('b709c15b-f805-44af-8237-1be89b4222a3', 'test1@some.com', 0, 'AJXCcIMNdksqvJMyLPleatRpak91jG4CW7sWYp46n7nUmAYIq053HblHTsuKWf+dgQ==', 'b190cd2c-54f9-49f1-9e05-5dcd6d3e5f0a', NULL, 0, 0, NULL, 1, 0, 'test1@some.com');

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
