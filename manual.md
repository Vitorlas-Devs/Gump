# Gump – Specifikáció

## Előszó

A Gump egy professzionális webes és mobilos alkalmazás kedvenc receptjeink összeállítására, megtekintésére, valamint megosztására. Egyedülálló módon az elkészítés lépéseit modulokból állítja össze az alkalmazás, melyek az egyes hozzávalók elkészítését is magába foglalják. Ezek tetszés szerint alakíthatóak, kombinálhatóak egyéb receptekkel, így lehetővé téve a moduláris felhasználást. 
Reméljük ezzel a kis bevezetővel már sokak figyelmét felkeltettük és aktív felhasználói lesznek a Gump-nak.

## Szerepkörök

Alapvetően 2 féle felhasználót különböztet meg az alkalmazás. A regisztrálatlan és a regisztrált felhasználót. Regisztrált felhasználóink továbbá lehetnek olvasók, szerkesztők, moderátorok vagy szakvégzett szakácsok/séfek/cukrászok.

## Felhasználói jogok

### Regisztrálatlan felhasználók

Lehetőségük van valamennyi nyilvánosan elérhető receptet megtekinteni és telefonjukon lementeni offline felhasználásra.

### Regisztrált felhasználók

- **Olvasók**: Azon felhasználók, akik még egyetlen receptet sem töltöttek olvasóként vannak megjelölve. Nekik már van felhasználónevük, tehát a közösségi felületet is igénybe vehetik. Részt tudnak venni kihívásokban, ismerőseiket fel tudják venni a barátok listára, rendelkeznek idővonallal, ahova fényképet tudnak feltölteni elkészített ételeikről és a lementett receptjeik bármilyen okoseszközről megtekinthetik. Természetesen offline mentésre is lehetőségük van, ha majd olyan helyen szeretnék a receptet megtekinteni, ahol nem áll rendelkezésre internetkapcsolat.
- **Szerkesztők**: Az olvasók valamennyi jogával rendelkeznek, azzal a különbséggel, hogy ők már töltöttek fel legalább egy receptet a felületre. A szerkesztőket, illetve azok receptjeit is lehet értékelni, ezzel egyfajta népszerűségi sorrendet kialakítva a szerkesztők és a receptek között.
- **Szakvégzett szakácsok/séfek/cukrászok**: Ez egy speciális szerepkör, ami azon szerkesztők kiváltásága, akik rendelkeznek valamilyen szakács bizonyítvánnyal. Az ilyen szerkesztőket hiteles forrásként tekintjük és a felhasználóknak lehetőségük van szakmai segítséget kérni tőlük, ha elakadnának a sütés/főzés során. A szakvégzett szerepkört a felhasználó neve melletti pipa ikon jelzi.
- **Moderátorok**: Ezen felhasználók feladata, hogy folyamatosan ellenőrizzék az oldalra felkerül tartalmakat és oda nem illő tartalom esetén megtegyék a megfelelő intézkedéseket. Ez lehet a recept levétele vagy a receptet feltöltő felhasználó végleges kitiltása.

## Alkalmazás felépítése

Most, hogy megnéztük, kiket különböztetünk meg az oldalon tekintsük át az alkalmazás felépítését.

### Kezdőlap

A kezdőlap több szekcióra van felbontva, melyeken az alábbi kategóriák szerint lehet recepteket böngészni:

- Menü:
  + Ez egy reszponzív navigációs sáv, mely számítógépen felül, okostelefonon alul jelenik meg. Itt kap helyet a keresőmező, a regisztrációs gomb, a ki-, és bejelentkező gomb, a saját profil elérése, valamint az eddig lementett receptek megtekintése.
- Kiemelt receptek:
  +	Ebben a szekcióban a felhasználó az alábbi nézetek szerint tudja befolyásolni a megjelenő recepteket.
  +	Személyre szabott: A keresési és megtekintési előzmények alapján hasonló kategóriájú recepteket jelenít meg.
  +	Új: Időrendi sorrendben megjeleníti a recepteket.
  +	Felkapott: Olyan receptek kerülnek ide, melyeket rövid idő alatt sok felhasználó mentett le.
  +	Legjobb: Azon receptek, melyeket összességében a legtöbben mentettek le.
-	Napi ajánlás:
    + Ebben a részben az alkalmazás véletlenszerűen kiválaszt egy ételt, majd az arról szóló receptek közül a legfelkapottabbat jeleníti meg.
-	Barátok-feed:
    +	Mint, ahogy a szerepköröknél említettük, a regisztrált felhasználóknak lehetőségük van ismerőseiket felvenni a barátok listára. Itt a barátaink idővonalára feltöltött fényképek, illetve az általuk feltöltött új receptek kapnak helyet.
-	I’m feeling lucky:
    +	A napi ajánlással ellentétben ez egy konkrét receptet választ ki véletlenszerűen, függetlenül attól, hogy felkapott-e vagy sem. Abban az esetben, ha a keresőmező nem üres, akkor a benne lévő kulcsszavak alapján választ ki véletlenszerűen egy receptet.
-	Legjobb recept összeállítás:
    +	Bizonyára mindenki kíváncsi a létező legtutibb receptre. Az oldal feldob az összes receptet analizálva, egy receptet, ami jobb, mint bármelyik más.
    +	Legjobb lehet mentések alapján, értékelések alapján, vagy a bejelentkezett felhasználóknál akár személyre szabva.
    +	!Fun fact!: Egy receptre kereséskor az oldal kilistázza a legjobb recepteket, illetve feldob egy olyan receptet, melyet az oldal állított össze, a legjobb receptből, úgy, hogy annak a receptnek az alreceptjeit lecserélte azok legjobb alternatíváira.

### Saját profil

Nevéhez hűen, itt felhasználói fiókunk beállítását tudjuk megadni. Ilyen a felhasználónév, email cím, megjelenítési név, profilkép megadása, stb. Az eddig feltöltött receptjeink és fényképeink is itt jelennek meg, itt van lehetőség új receptet létrehozni, továbbá a barátok listát szerkeszteni.

- Elmentett receptek: Elmentett receptjeink jelennek meg ezen az oldalon. A rendezésüket lehet módosítani az alábbiak szerint:
  +	Legutóbb hozzáadott
  +	Legrégebben hozzáadott
  +	ABC sorrend, cím szerint növekvő
  +	ABC sorrend, cím szerint csökkentő
  +	Felkapott
  +	Legnépszerűbb

Tisztában vagyunk azzal, hogy minden ember ízlése eltérő, ezért a Gump lehetőséget biztosít a regisztrált felhasználóknak az elmentett receptek módosítására, kiegészítésére. Így biztosítjuk azt, hogy mindenki saját ízlésére alakíthassa kedvenc receptjeit.

### Receptek

Ideje néhány szót ejteni a receptek megtekintéséről és létrehozásáról.

#### Receptek megtekintése

Abban az esetben, ha sikerül kiválasztanunk egy receptet az a következők szerint fog megjelenni.

-	Recept cím
-	Kategória
-	Értékelés
-	Mentések száma
-	Elkészítési idő
-	Hozzávalók
-	Elkészítés

Mint, ahogyan az a bevezetőből kiderült a receptek modulokból épülnek fel. A Gump a modulokat kártyáknak nevezi. Az étel elkészítésének minden egyes lépése különálló kártyaként jelenik meg. Léteznek egymásba ágyazott kártyák, azokkal lehet alreceptekre bontani a receptet.

#### Segítségkérés

A felhasználók szakmai segítséget a szakvégzett szakácsoktól tudnak kérni. A szakvégzettek listája egy különálló oldalon jelenik meg. A segítségnyújtást a Community Discord szerveren biztosítjuk.

#### Recept létrehozása

Receptek létrehozásához 3 féle megoldás létezik. Ezek a Minimális, Komplex és Nyers megadási módok, amik közül az alapértelmezett a Komplex megadási mód.

Minimális megadási mód esetén a recept egyes hozzávalóinak feldolgozása nem kerül kifejtésre, helyette ki kell választani, más már létező recepteket és azokból fog összeállni az étel receptje.

A Komplex megadási mód a teljes recept leírása, ahol a szerkesztő választja ki a receptben az alrecepteket. Ilyenkor nem történik utalás más receptekre.

A klasszikus megadási mód, a Nyers. Ilyenkor nem kártyákkal történik a recept elkészítése, hanem mondatok leírásával. Ebben az esetben a program a mondatokat értelmezve felismeri az utasításokat és automatikusan összeállítja a kártyákat. A Gump működését segíthetjük azzal, hogy minden különálló utasítást külön bekezdésbe teszünk, ezzel biztosítva a megfelelő értelemzést.

Természetesen a leghatékonyabb elkészítés érdekében a három módszert együttesen is lehet alkalmazni.

#### Recept törlése

A receptek teljes törlésére akkor van lehetőség, ha a törlendő receptet más felhasználó még nem mentette el magának vagy még nem történt rá hivatkozás másik receptben. Ellenkező esetben a receptet csak archiválni lehet. Ez azt jelenti, hogy az adatbázisban továbbra is jelen lesz a recept, csak a keresésekben nem fog találatként megjelenni.

#### Recept módosítása

Az oldalon lehetőség van meglévő recepteket módosítani, ami ugyanúgy működik, mint a létrehozási felület.

## Partnerek

Monetizálás érdekében partnerségeket lehet kötni különböző szponzorokkal.

### Alapanyag árulás

A partnerek biztosíthatnak egy listát a náluk vásárolható termékekről és azuk árairól, hogy az oldalon az egyes alapanyagoknál megjelenjenek ezek a termékek és az olvasók ez alapján válogathasság össze, hogy honnan vásárolnak be a főzéshez/sütéshez. Lehetőség lenne akciókat is kimutatni. Egyszerű alapanyagoknál és recepteknél is mutatva lehet, mert nem ID-hoz, hanem névhez/címhez kötött.

Példa: Ha az alapanyagnak a neve az, hogy "tojás", akkor egy kosár ikonra kattintva az oldal kilistázza az összes helyet és az árat, ahol árulnak olyan terméket, aminek az a neve, hogy "tojás". Ha az alapanyag hivatkozik egy receptre, aminek az a címe, hogy "Pizzaszósz", akkor a partnerek "pizzaszósz"-ait fogja kilistázni.

### Reklámok

A szokásos formában is megjelenhetnek reklámok, ha egy partner/szponzor ezért fizet. Ezek a reklámok az oldalon különböző helyen jelennének meg, mikor az olvasók a receptek között böngésznek.

## Technikai adatok

### Recept létrehozás/mentés/módosítás/törlés

Minden recept **létrehozás**ánál az bekerül az adatbázisba, az író profilján pedig a `recipes` listába kerül annak az ID-ja.

Ha valaki **lement**i azt a receptet, akkor a recept `saveCount`-ja növekszik egyel (elmentés visszavonásával pedig csökken). A lementő profilján pedig bekerül a `recipes` listájába a recept ID-ja. Az oldal abból különbözteti meg, hogy csak lementve van, vagy Ő is írta, hogy megnézi a szerző ID-ját. (Ha megegyezik, akkor Ő készítette, ha nem, akkor csak lementette.)

Ha a **szerző módosít**ani szeretné a receptjét, de vannak rá hivatkozások máshol, akkor az nem az eredeti recept entry-t módosítja, hanem létrehoz egy teljesen újat, de más ID-jút. Így lesz a szerzőnek a profilján két recept, ahol az egyik a másiknak egy régebbi változata. Hogy a profilon ne jelenjenek meg ezek egymás mellett, az oldal cím alapján egybeteszi őket és csak a legújabbakat mutatja. A régebbieket meg lehet nézni, ha a recept olvasásánál kiválasztunk egy régebbi verziót. Az ID nem random, hanem egy számláló, így az újabb receptek ID-ja magasabb lesz, mint a régieké, ezért ezt könnyedén használhatjuk a receptek verzióinak nyomonküvetésére. Ha az eredeti receptre nem hivatkozik senki a saját receptjében, akkor ez a verziózás kihagyható, egyből az eredeti recept lesz módosí¤va, hogy ne foglaljon fölösleges helyet.

Ha a **szerző töröl**ni szeretné a receptjét, de vannak rá hivatkozások máshol, akkor a recept nem törlődik, hanem az `isArchived` mezője kap igaz értéket. Az archivált receptek nem jelennek meg a szerző profilján, de az arra hivatkozó receptek nem lesznek hiányosak. Ha a receptet senki se használta a sajátjában, akkor nem szükséges archiválni, az adatbázisból egyből törölhető, hogy ne foglaljon fölösleges helyet.

Ha **valaki módosít**ani szeretné más receptjét, akkor az oldal készít egy másolatot arról a receptről a módosító profiljára, amiben Ő lesz az új szerző, kap egy új ID-t is és ez az új ID bekerül az eredeti recept `forks` listájába, valamint az eredeti recept meg bekerül az új recept `originalRecipe` mezőjébe. Innentől a módosítás ugyan úgy történik, mint ahogy fentebb le lett írva a saját recept módosításánál.
