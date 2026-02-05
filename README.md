# InventoryApi - Raktárkészlet Kezelő Alkalmazás

Ez a projekt egy backend tesztfeladat megoldása, amely lehetővé teszi anyagok nyilvántartását és készletmozgások (bevét/kiadás) rögzítését.

## Technológiák
* **.NET 8** (ASP.NET Core Web API)
* **Entity Framework Core** (ORM)
* **SQLite** (Adatbázis)

## Funkciók
* **Anyagok kezelése:** Új anyagok felvétele, listázása.
* **Készletmozgás:** Bevételezés és Kiadás rögzítése.
* **Validáció:**
    * Nem lehet negatív mennyiséget rögzíteni.
    * **Kiadás védelem:** A rendszer nem enged kiadni több anyagot, mint amennyi a pillanatnyi készlet (a korábbi mozgások alapján számolva).
    * Nem lehet kettő ugyanolyan nevű és mértékegységű anyagot létrehozni.
* **Valós idejű készletszámítás:** A rendszer nem tárol redundáns készletadatot, hanem minden lekérdezéskor a tranzakciókból számolja ki az aktuális állományt (Event Sourcing jellegű megközelítés).

## Futtatási útmutató

### Előfeltételek
* .NET 8 SDK telepítése
* Visual Studio 2022 (ajánlott)

### Indítás Visual Studio-val
1.  Nyisd meg az `InventoryService.sln` fájlt.
2.  Nyomd meg az **F5** billentyűt (vagy a Start gombot).
3.  Az alkalmazás automatikusan létrehozza az `inventory.db` SQLite adatbázist a projekt gyökerében.
4.  A böngészőben megnyílik a **Swagger UI**, ahol tesztelhetők a végpontok.

### API Végpontok (Swagger)
Az alkalmazás indulása után a Swagger felületen (`/swagger/index.html`) érhetők el a végpontok:
* `GET /api/Materials` : Minden anyag listázása
* `POST /api/Materials`: Új anyag létrehozása.
* `POST /api/Stock/movements`: Készletmozgás (1 = Bevét, 2 = Kiadás).
* `GET /api/Stock/{materialId}`: Aktuális készlet lekérdezése.

