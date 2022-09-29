# Gump - Felhasználói kézikönyv
Funkciók, leírások, tanácsok az appot használóknak.

## Előszó
Csapatunk célja egy olyan webes és mobilos alkalmazás készítése, mely lehetővé teszi a receptek moduláris összeállítását, megtekintését és megosztását.

## Főoldal
A főoldalon egyből látni lehet néhány kiemelt receptet, napi ajánlást, barátok feed-et és egy menü sávot, ahonnan az oldal többi részét is el lehet érni.

### Kiemelt receptek
Alapértelmezetten személyre szabott recept ajánlások, de átváltható más nézetekre is:
- **Új**: A legújabb receptek
- **Felkapott**: Sok mentést szerző receptek
- **Legjobb**: Legtöbbet elmentett receptek

### Napi ajánlás
Egy véletlenszerűen kivásztott recept kap egy saját kis részt a főoldalon, ahol a legjobb találatokat ajánlja fel. Bejelentkezett felhasználóknál [személyre szabott ajánlás](#személyre-szabott-ajánlás) is elérhető.

### Barátok-feed
Itt látható, hogy aznap melyik barát, milyen ételt készít/készített. A barátok egymás mentéseit is megtekinthetik, valamint küldhetnek egymásnak recepteket. Csak bejelentkezett felhasználóknak érhető el.

### Menü sáv
Reszponzív sáv - számítógépen felül, mobilon alul jelenik meg - innen lehet receptekre keresni, ki-bejelentkeyni/regisztrálni, saját profilt megtekinteni, vagy a mentések között böngészni.

### I'm feeling lucky
Az eddig leírt receptböngészési megoldásokon túl lehet használni az 'I'm feeling lucky' ('Jó napom van') gombot, ami egy véletlenszerűen kiválasztott receptet dob fel.

## Receptek
Minden, amit a receptek működéséről tudni kell, az lentebb olvasható

### Olvasás
Egy recept megnyitásakor az oldal összegyűjt mindent, és egyetlen receptként jeleníti meg az olvasónak. Az egyes szakaszok (alreceptek) bezárhatóak, kinyithatóak, menthetőek, stb.

Egy kisegítő funkció, hogy a lap görgetése megoldható mutogatással, ha van kamera az eszközön.

### Szerkesztés
3 féle megoldás lehetséges:
1. **Minimális**: Minden recept a lehető legegyszerűbb utasítás sorból és a lehető legkevesebb hozzávalóból áll. Hozzávalóknak már létező recepteket kell megadni, azok ID-ként lesznek elmentve és betöltésnél azok hozzávalóit jeleníti meg az oldal. Az utasításokhoz kötelező kártyákként kerülnek a beimportált receptek.
2. **Komplex**: A rendelkezésre álló eszközök segítségével az író a teljes receptet írja meg, azt szekciókra tördelve, amit az oldal szed szét több különböző receptre és optimalizálja az első pont szerint. A hozzávalókba megadja az összes alapanyagot felsorolva. Az elkészítés menetét kártyákkal kell összeállítani: Minden lépés egy új kártya. Egy kártyán belül új kártyákat is létre lehet hozni, ilyenkor egy nevet kell adni a nagy kártyának (egy önálló receptnek kell minősülnie → beállítások gombbal egyéb recept beállításokat lehet hozzáadni, pl. labelek). Hozzávaló hivatkozásokat kell tenni a lépésekbe, amiben megadható, hogy milyen mennyiségben, mit kell használni (Külön gomb, de a szövegből megpróbál rájönni az oldal, hogy mit ír a szerző).
3. **Nyers**: Hozzávalók ugyanúgy működnek, mint az előző két esetben, de az utasítások megírása már kártyák nélkül történik. Egy átlagos receptleírás adható meg, amiből az oldal magától próbál akár összefűzött kártyákat alkotni. Segíthetjük az oldal munkáját markdown-nal, hogy például az egyes szekciókat megfelelően jelöljük.
*A megoldásokat lehet vegyíteni, a leghatékonyabb szeerkesztési élményért.*

### Személyre szabott ajánlás
Az elmentett receptek alapján az oldal olyanokat keres, amikre olyan felhasználók is kerestek, akik hasonló recepteket mentettek el.

#### Legjobb recept összerakás
Egy receptre kereséskor az oldal olyanokat is feldob, amiket maga állított össze a meglévőkből ajánlások illetve mentések alapján a következőképpen: Kereséskor megkeresi a legjobb receptet, majd annak az alreceptjeit kicseréli a legjobb olyan receptekre, amik megegyező névvel rendelkeznek, mint azok. Rekurzívan végigmegy az összes alrecepten, majd kiad egy olyat, ami valószínűleg tökéletesen megfelel az olvasó számára.

## To be continued
Ezek még csak a kezdeti tervek. További hasznos funkciókra lehet még számítani...
