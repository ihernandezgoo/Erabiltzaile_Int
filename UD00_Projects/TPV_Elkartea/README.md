# TPV_Elkartea

Proiektu honek **funtzionala den TPV (Salmenta Puntuko Terminala)** aurkezten du, UD00_Projects barruan garatua.  

---

## Instalazioa eta exekuzioa

1. Repositorioa klonatu:

```bash
git clone https://github.com/ihernandezgoo/Erabiltzaile_Int.git

cd UD00_Projects/TPV_Elkartea/TPV_Elkartea/bin/Debug/net8.0-windows/

Run TPV_Elkartea.exe

```
---

2. Datu baseak:

```bash
1. Erabiltzaileak.db
CREATE TABLE "Erabiltzaileak" (
	"Id"	INTEGER,
	"Izena"	TEXT,
	"Pasahitza"	TEXT,
	"Rola"	TEXT,
	PRIMARY KEY("Id")
)

Erabiltzaileen sortutako kontuak

Admin, Admin
Ivan, Ivan

2. Produktuak.db
CREATE TABLE "Edariak" (
	"Id"	INTEGER,
	"Nombre"	TEXT,
	"Precio"	INTEGER,
	"Stock"	INTEGER,
	"img"	TEXT,
	PRIMARY KEY("Id")
)

```