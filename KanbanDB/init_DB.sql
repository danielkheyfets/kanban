CREATE TABLE "Users" (
	"Email"	TEXT NOT NULL UNIQUE,
	"Password"	TEXT NOT NULL,
	"Nickname"	TEXT NOT NULL,
	PRIMARY KEY("Email")
);

CREATE TABLE "Columns" (
	"Email"	TEXT NOT NULL,
	"ColumnOrdinal"	INTEGER NOT NULL,
	"ColumnName"	TEXT NOT NULL,
	"MaxValue"	INTEGER NOT NULL,
	"CurrNumOfTasks"	INTEGER NOT NULL,
	PRIMARY KEY("Email","ColumnOrdinal")
);

CREATE TABLE "Tasks" (
	"Email"	TEXT NOT NULL,
	"TaskID"	INTEGER NOT NULL,
	"Title"	TEXT NOT NULL,
	"Description"	TEXT NOT NULL,
	"ColumnOrdinal"	TEXT NOT NULL,
	"CreationDate"	INTEGER NOT NULL,
	"DueDate"	INTEGER NOT NULL,
	PRIMARY KEY("Email","TaskID")
);
