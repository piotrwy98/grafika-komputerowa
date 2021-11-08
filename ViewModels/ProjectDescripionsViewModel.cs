namespace GrafikaKomputerowa.ViewModels
{
    public class ProjectDescripionsViewModel
    {
        public string Project1Description { get; } = "• Rysowanie trzech prymitywów: linii, prostokątu, okręgu,\n" +
            "• Podawanie parametrów rysowania za pomocą pola tekstowego (wpisanie parametrów w pola tekstowe i zatwierdzenie przyciskiem),\n" +
            "• Rysowanie przy użyciu myszy (definiowanie punktów charakterystycznych kliknięciami),\n" +
            "• Przesuwanie przy użyciu myszy (uchwycenie np. za krawędź i przeciągnięcie),\n" +
            "• Zmiana kształtu / rozmiaru przy użyciu myszy (uchwycenie za punkty charakterystyczne i przeciągnięcie),\n" +
            "• Zmiana kształtu / rozmiaru przy użyciu pola tekstowego (zaznaczenie obiektu i modyfikacja jego parametrów przy użyciu pola tekstowego).\n" +
            "• Serializacja i deserializacja narysowanych obiektów (zapis i odczyt z pliku).";

        public string Project2Description { get; } = "• Wczytywanie i wyświetlanie plików graficznych w formacie PPM P3,\n" +
            "• Wczytywanie i wyświetlanie plików graficznych w formacie PPM P6,\n" +
            "• Obsługa błędów (komunikaty w przypadku nieobsługiwanego formatu pliku oraz błędów w obsługiwanych formatach plików),\n" +
            "• Wydajny sposób wczytywania plików (blokowy zamiast bajt po bajcie),\n" +
            "• Wczytywanie plików JPEG,\n" +
            "• Zapisywanie wczytanego pliku w formacie JPEG,\n" +
            "• Możliwość wyboru stopnia kompresji przy zapisie do JPEG,\n" +
            "• Skalowanie liniowe kolorów,\n" +
            "• Proszę nie używać gotowych bibliotek do wczytywania plików PPM.";

        public string Project3Description { get; } = "a. Konwersja przestrzeni barw:\n" +
            "  • Dwie możliwości wyboru koloru przez użytkownika: RGB orac CMYK (narzędzia do wyboru koloru mogą być wzorowane na popularnych programach graficznych),\n" +
            "  • Wybór koloru powinny odbywać się zarówno za pomocą myszy, jak i poprzez wprowadzenie poszczególnych wartości w pola tekstowe,\n" +
            "  • Wybrany kolor powinien zostać zaprezentowany oraz przekonwertowany na drugi format, tzn. na CMYK przy wyborze RGB oraz na RGB przy wyborze CMYK,\n" +
            "  • Wartości wybrane przez użytkownika oraz po konwersji powinny zostać wyświetlone.\n\n" +
            "b. Rysowanie kostki RGB:\n" +
            "  • Kostka RGB powinna zostać narysowana w trójwymiarze,\n" +
            "  • Użytkownik powinien mieć możliwość obracania kostką,\n" +
            "  • Pokrycie kostki kolorami powinno odbywać się przy użyciu odpowiednich wzorów.";

        public string Project4Description { get; } = "a. Przekształcenia punktowe:\n" +
            "  • Dodawanie (dowolnych podanych przez użytkownika wartości),\n" +
            "  • Odejmowanie (dowolnych podanych przez użytkownika wartości),\n" +
            "  • Mnożenie (przez dowolne podane przez użytkownika wartości),\n" +
            "  • Dzielenie (przez dowolne podane przez użytkownika wartości),\n" +
            "  • Zmiana jasności (o dowolny podany przez użytkownika poziom),\n" +
            "  • Przejście do skali szarości (na dwa sposoby).\n\n" +
            "b. Metody polepszania jakości obrazów:\n" +
            "  • Filtr wygładzający (uśredniający),\n" +
            "  • Filtr medianowy,\n" +
            "  • Filtr wykrywania krawędzi (sobel),\n" +
            "  • Filtr górnoprzepustowy wyostrzający,\n" +
            "  • Filtr rozmycie gaussowskie.";

        public string Project5Description { get; } = "a. Histogram:\n" +
            "  • Rozszerzenie histogramu,\n" +
            "  • Wyrównanie (ang. equalization) histogramu.\n\n" +
            "b. Binaryzacja:\n" +
            "  • Ręcznie przez użytkownika - użytkownik podaje próg bezpośrednio,\n" +
            "  • Procentowa selekcja czarnego (ang. Percent Black Selection),\n" +
            "  • Selekcja entropii (ang. Entropy Selection).";
    }
}
