namespace GrafikaKomputerowa.ViewModels
{
    public class ProjectDescripionsViewModel
    {
        public string Project1Description { get; } = "\u2022 Rysowanie trzech prymitywów: linii, prostokątu, okręgu,\n" +
            "\u2022 Podawanie parametrów rysowania za pomocą pola tekstowego (wpisanie parametrów w pola tekstowe i zatwierdzenie przyciskiem),\n" +
            "\u2022 Rysowanie przy użyciu myszy (definiowanie punktów charakterystycznych kliknięciami),\n" +
            "\u2022 Przesuwanie przy użyciu myszy (uchwycenie np. za krawędź i przeciągnięcie),\n" +
            "\u2022 Zmiana kształtu / rozmiaru przy użyciu myszy (uchwycenie za punkty charakterystyczne i przeciągnięcie),\n" +
            "\u2022 Zmiana kształtu / rozmiaru przy użyciu pola tekstowego (zaznaczenie obiektu i modyfikacja jego parametrów przy użyciu pola tekstowego).\n\n" +
            "Dodatkowo:\n" +
            "\u2022 Serializacja i deserializacja narysowanych obiektów (zapis i odczyt z pliku).";

        public string Project2Description { get; } = "\u2022 Wczytywanie i wyświetlanie plików graficznych w formacie PPM P3,\n" +
            "\u2022 Wczytywanie i wyświetlanie plików graficznych w formacie PPM P6,\n" +
            "\u2022 Obsługa błędów (komunikaty w przypadku nieobsługiwanego formatu pliku oraz błędów w obsługiwanych formatach plików),\n" +
            "\u2022 Wydajny sposób wczytywania plików (blokowy zamiast bajt po bajcie),\n" +
            "\u2022 Wczytywanie plików JPEG,\n" +
            "\u2022 Zapisywanie wczytanego pliku w formacie JPEG,\n" +
            "\u2022 Możliwość wyboru stopnia kompresji przy zapisie do JPEG,\n" +
            "\u2022 Skalowanie liniowe kolorów,\n" +
            "\u2022 Proszę nie używać gotowych bibliotek do wczytywania plików PPM.";

        public string Project3Description { get; } = "a. Konwersja przestrzeni barw:\n" +
            "  \u2022 Dwie możliwości wyboru koloru przez użytkownika: RGB orac CMYK (narzędzia do wyboru koloru mogą być wzorowane na popularnych programach graficznych),\n" +
            "  \u2022 Wybór koloru powinny odbywać się zarówno za pomocą myszy, jak i poprzez wprowadzenie poszczególnych wartości w pola tekstowe,\n" +
            "  \u2022 Wybrany kolor powinien zostać zaprezentowany oraz przekonwertowany na drugi format, tzn. na CMYK przy wyborze RGB oraz na RGB przy wyborze CMYK,\n" +
            "  \u2022 Wartości wybrane przez użytkownika oraz po konwersji powinny zostać wyświetlone.\n\n" +
            "b. Rysowanie kostki RGB:\n" +
            "  \u2022 Kostka RGB powinna zostać narysowana w trójwymiarze,\n" +
            "  \u2022 Użytkownik powinien mieć możliwość obracania kostką,\n" +
            "  \u2022 Pokrycie kostki kolorami powinno odbywać się przy użyciu odpowiednich wzorów.";
    }
}
