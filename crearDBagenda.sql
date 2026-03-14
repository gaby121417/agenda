CREATE TABLE "Contactos" (
	"id_contactos"	INTEGER NOT NULL,
	"nombre"	TEXT NOT NULL,
	"apellido"	TEXT,
	"telefono"	TEXT,
	"direccion"	TEXT,
	"localidad"	TEXT,
	"email"	TEXT,
	"fecha"	DATE,
	PRIMARY KEY("id_contactos" AUTOINCREMENT)
);