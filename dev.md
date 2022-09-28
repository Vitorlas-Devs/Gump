# Fejlesztői leírás
Ebben a dokumentációban lehet olvasni az oldalon használt technológiákról, a felépítésről és a működésről.

## Használt technológiák
Csapatunk számos fejlesztői eszközöket, programozási nyelveket használ a project megvalósítása során.

### Frontend
- **VUE** - Progressive frontend framework, komponensekből építhetőek fel vele weboldalak, nagyobb projektekhez tökéletes. Azért választottuk, mert ez az egyik legátláthatóbb framework a piacon, és rengeteg jót lehet hallani róla.
- **Quasar** - Vue-hoz készített framework rengeteg stílusos és funkcionális elemmel.
- **Sass** - CSS kiegészítő nyelv, amivel könnyebben és gyorsabban megy a formázás.
- **TypeScript** - JavaScript superset, amivel átláthatóbb lesz a kód, gyorsabb, mint sok más alternatíva, nagyobb projektekhez tökéletes.
- **TensorFlow** - Machine Learning könyvtár frontendre.
- **Capacitor** - Cross-platform native runtime webapplikációkhoz. Ennek a segítségével készítünk a weboldalunkból native telefonos applikációt.

## Backend
- **ASP.NET** - C# programnyelvben írt backend framework. Azért választottuk, mert már jártasak vagyunk a nyelvben, így könnyedén és gyorsan megy a fejlesztés.

### Adatbázis
- **MongoDB** - Gyors, objektum-alapú adatbázis. Könnyű kiigazodni rajta és a MongoDB.Driver .Net NuGet-ről telepíthető könyvtár segítségével erőfeszítés nélkül használható a C# alapú backendünkön.

### Hosting
- **DigitalOcean** - Diákoknak beszerezhető Github Students pack tartalma. 100$ credit-ig ingyenesen használhatóak a szervereik. Kiváló sebességű. Ubuntu Linux-on fog futni a szerver.
- **Netlify** - Ingyenes, SSL certificate-et biztosító, Githubról automatikusan frissülő frontend host.

### Eszközök
- **Visual Studio Code** - Nyílt-forráskódú, cross-platform kódszerkesztő program, rengeteg kiegészítővel, így bármilyen területen helyt áll, tökéletes Vue frontend fejlesztésre.
- **Visual Studio** - A Visual Studio legfrissebb verziója, elsősorban C#-ra optimalizált IDE, rengeteg hasznos fejlesztői eszközzel, tökéletes ASP.NET backend fejlesztésre.
- **Git** - Verziókezelő szoftver a project rendszerezésére, fejlesztések átlátható rendezése.
- **Github** - Git-kezelő szolgáltatás, ahol a project tervezésétől, feladatok kiosztásán át, a kód tárolásáig és teszteléséig rengeteg hasznos segítséget nyújt a fejlesztés során.
- **Github Projects** - Átlátható felület a projektünk tervezésére, feladatok kiosztására.
- **Github Copilot** - Mivel sok új, számunkra eleinte ismeretlen technológiával kell foglalkozni, ezért hatalmas segítséget nyújt a diákoknak ingyenes VSC és VS22 kiegészítő, ami a Github-ra feltöltött kódok segítségével egészíti ki a saját kódunkat.
- **Codacy** - Tiszta kód elvek, biztonsági intézkedések, performance és sok más figyelésére alkalmas szolgáltatás.
- **Discord** - Modern platform, ahol könnyen lehet beszélgetni egymással és rendezni az ötleteket. Szinte minden kommunikáció itt történik.
- **Figma** - Csapatoknak kitalált webes design szoftver, amivel a projektünk terveit vizualizáljuk.
- **DALL-E** - Szövegből képet generáló algoritmus az OpenAI-tól. Legújabb verziójához limitált ingyenes hozzáférés szerezhető, amivel profi minőségű asseteket generálhatunk, mint például a projekt logóját.

## Workflow
Fejlesztéskor az ötleteket megbeszéljük, terveket felírjuk, feladatokat kiosztjuk a Github Issue-kkal és a Feature Branch Workflow elveit követjük. Azért választottuk ezt a workflow-t, mert így könnyen tud a csapatunk párhuzamosan dolgozni egymással, átláthatóan. Mivel az oldal nincs futtatva fejlesztés közben, így nem láttuk szükségét alkalmazni egy külön branchet a félkész fejlesztéseknek.
