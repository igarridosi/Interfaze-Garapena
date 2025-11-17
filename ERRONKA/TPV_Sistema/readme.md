# TPV Elkartea

Mahaigaineko TPV (Terminal Punt de Venda) aplikazio osoa, elkarte gastronomiko bat kudeatzeko diseinatua. Aplikazioa C# eta WPF teknologiekin garatuta dago, MVVM (Model-View-ViewModel) arkitektura-eredua jarraituz.

---

## 🚀 Ezaugarri Nagusiak | Features

*   **Erabiltzaile Kudeaketa:** Bi rol desberdin: administratzailea eta erabiltzaile arrunta.
*   **Administrazio Panel Osoa:**
    *   Erabiltzaileen sorrera, aldaketa eta ezabaketa (CRUD).
    *   Produktuen inbentarioaren kudeaketa (CRUD).
    *   Elkarteko mahaien kudeaketa (CRUD).
    *   Egindako salmenta guztien erregistro zehatza.
*   **TPV Interfaze Intuitiboa:**
    *   Produktuen hautaketa bisuala.
    *   Kantitate anitzak sartzeko zenbaki-teklatua.
    *   BEZaren kalkulu automatikoa.
    *   Salmentak gorde eta stocka denbora errealean eguneratu.
*   **Ticket Inprimaketa:** Salmenta bakoitzaren ondoren, ticketa PDF formatuan sortu eta inprimatzeko aukera.
*   **Mahaien Erreserba Sistema:** Erabiltzaileek mahaiak erreserbatu ditzakete data eta otordu zehatzetarako.

---

## 🛠️ Erabilitako Teknologiak | Technologies Used

*   **Lengoaia | Language:** C# 12
*   **Framework:** .NET 8
*   **Interfazea | UI Framework:** Windows Presentation Foundation (WPF)
*   **Arkitektura | Architecture:** Model-View-ViewModel (MVVM)
*   **Datu Sarbidea | Data Access:** Entity Framework Core 8
*   **Datu-basea | Database:** SQLite
*   **PDF Sortzea | PDF Generation:** QuestPDF

---

## 📋 Nola Jarri Martxan | Getting Started

Proiektu hau zure ordenagailuan exekutatzeko, jarraitu pauso hauek:

### Aurrebaldintzak | Prerequisites

*   [Visual Studio 2022](https://visualstudio.microsoft.com/es/vs/) (Community edizioa nahikoa da).
*   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
*   [Git](https://git-scm.com/).

### Instalazioa | Installation

1.  **Errepositorioa Klonatu:**
    ```sh
    git clone https://github.com/igarridosi/Interfaze-Garapena/new/main/ERRONKA/TPV_Sistema
    ```

2.  **Ireki Proiektua Visual Studion:**
    *   Ireki Visual Studio.
    *   Hautatu "Open a project or solution".
    *   Nabigatu klonatu duzun karpetara eta ireki `.sln` fitxategia.

3.  **Lehen Konpilazioa:**
    *   Proiektua ireki ondoren, Visual Studio-k beharrezko NuGet paketeak automatikoki berreskuratuko ditu.
    *   Sakatu **F5** edo joan "Build" -> "Build Solution" menura proiektua konpilatzeko eta exekutatzeko.

### Lehen Erabilera | First Use

Aplikazioa lehen aldiz exekutatzen duzunean, `elkartea.db` izeneko SQLite datu-base fitxategi bat automatikoki sortuko da exekuzio-karpetan (`bin/Debug/...`).

Datu-base hau hasieran hutsik egongo da. Aplikazioa probatzeko, administratzaile gisa sartu eta erabiltzaileak, produktuak eta mahaiak sortu behar dituzu.

#### Proba Kredentzialak | Test Credentials

Hasteko, `admin` erabiltzailea sortu dezakezu administratzaile paneletik edo zuzenean datu-basean sartuz [DB Browser for SQLite](https://sqlitebrowser.org/) bezalako tresna batekin.

*   **Administratzailea:**
    *   **Erabiltzailea:** `admin`
    *   **Pasahitza:** `admin`
*   **Erabiltzaile Arrunta:**
    *   **Erabiltzailea:** `user`
    *   **Pasahitza:** `user`

---

## 📄 Lizentzia | License

Proiektu hau MIT Lizentziapean banatzen da. Ikusi `LICENSE` fitxategia xehetasun gehiagorako.

## ✒️ Egilea | Author

*   **Ibai Garrido** - [GitHub Profila](https://github.com/igarridosi)
