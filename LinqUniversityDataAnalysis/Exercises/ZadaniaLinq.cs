using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    ///     Zadanie:
    ///     Wyszukaj wszystkich studentów mieszkających w Warsaw.
    ///     Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///     SQL:
    ///     SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    ///     FROM Studenci
    ///     WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci
            .Where(student => student.Miasto == "Warsaw")
            .Select(student => $"{student.NumerIndeksu} | {student.Imie} {student.Nazwisko} | {student.Miasto}");
    }

    /// <summary>
    ///     Zadanie:
    ///     Przygotuj listę adresów e-mail wszystkich studentów.
    ///     Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///     SQL:
    ///     SELECT Email
    ///     FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci
            .Select(student => student.Email);
    }

    /// <summary>
    ///     Zadanie:
    ///     Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    ///     Zwróć numer indeksu i pełne imię i nazwisko.
    ///     SQL:
    ///     SELECT NumerIndeksu, Imie, Nazwisko
    ///     FROM Studenci
    ///     ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci
            .OrderBy(student => student.Nazwisko)
            .ThenBy(student => student.Imie)
            .Select(student => $"{student.NumerIndeksu} | {student.Imie} {student.Nazwisko}");
    }

    /// <summary>
    ///     Zadanie:
    ///     Znajdź pierwszy przedmiot z kategorii Analytics.
    ///     Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///     SQL:
    ///     SELECT TOP 1 Nazwa, DataStartu
    ///     FROM Przedmioty
    ///     WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var subject = DaneUczelni.Przedmioty
            .FirstOrDefault(subject => subject.Kategoria == "Analytics");

        if (subject == null) return ["Brak przedmiotu z kategorii Analytics."];

        return [$"{subject.Nazwa} | start: {subject.DataStartu:dd-MM-yyyy}"];
    }

    /// <summary>
    ///     Zadanie:
    ///     Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    ///     Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///     SQL:
    ///     SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    ///     ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var anyInactiveEnrollment = DaneUczelni.Zapisy
            .Any(enrollment => !enrollment.CzyAktywny);

        return [$"Czy istnieje nieaktywny zapis: {(anyInactiveEnrollment ? "Tak" : "Nie")}"];
    }

    /// <summary>
    ///     Zadanie:
    ///     Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    ///     Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///     SQL:
    ///     SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    ///     THEN 1 ELSE 0 END
    ///     FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        var doesEveryoneHaveCathedral = DaneUczelni.Prowadzacy
            .All(lecturer => !string.IsNullOrWhiteSpace(lecturer.Katedra));

        return [$"Czy wszyscy prowadzący mają katedrę: {(doesEveryoneHaveCathedral ? "Tak" : "Nie")}"];
    }

    /// <summary>
    ///     Zadanie:
    ///     Policz, ile aktywnych zapisów znajduje się w systemie.
    ///     SQL:
    ///     SELECT COUNT(*)
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var activeEnrollmentsCount = DaneUczelni.Zapisy
            .Count(enrollment => enrollment.CzyAktywny);

        return [$"Liczba aktywnych zapisów: {activeEnrollmentsCount}"];
    }

    /// <summary>
    ///     Zadanie:
    ///     Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///     SQL:
    ///     SELECT DISTINCT Miasto
    ///     FROM Studenci
    ///     ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci
            .Select(student => student.Miasto)
            .Distinct()
            .OrderBy(city => city);
    }

    /// <summary>
    ///     Zadanie:
    ///     Zwróć trzy najnowsze zapisy na przedmioty.
    ///     W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///     SQL:
    ///     SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    ///     FROM Zapisy
    ///     ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(enrollment => enrollment.DataZapisu)
            .Take(3)
            .Select(enrollment =>
                $"{enrollment.DataZapisu:dd-MM-yyyy} | studentId: {enrollment.StudentId} | przedmiotId: {enrollment.PrzedmiotId}");
    }

    /// <summary>
    ///     Zadanie:
    ///     Zaimplementuj prostą paginację dla listy przedmiotów.
    ///     Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///     SQL:
    ///     SELECT Nazwa, Kategoria
    ///     FROM Przedmioty
    ///     ORDER BY Nazwa
    ///     OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        var pageSize = 2;
        var pageNumber = 2;

        return DaneUczelni.Przedmioty
            .OrderBy(subject => subject.Nazwa)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(subject => $"{subject.Nazwa} | {subject.Kategoria}");
    }

    /// <summary>
    ///     Zadanie:
    ///     Połącz studentów z zapisami po StudentId.
    ///     Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///     SQL:
    ///     SELECT s.Imie, s.Nazwisko, z.DataZapisu
    ///     FROM Studenci s
    ///     JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                student => student.Id,
                enrollment => enrollment.StudentId,
                (student, enrollment) => $"{student.Imie} {student.Nazwisko} | {enrollment.DataZapisu:dd-MM-yyyy}"
            );
    }

    /// <summary>
    ///     Zadanie:
    ///     Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    ///     Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///     SQL:
    ///     SELECT s.Imie, s.Nazwisko, p.Nazwa
    ///     FROM Zapisy z
    ///     JOIN Studenci s ON s.Id = z.StudentId
    ///     JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Studenci
            .SelectMany(student => DaneUczelni.Zapisy
                .Where(enrollment => enrollment.StudentId == student.Id)
                .Join(
                    DaneUczelni.Przedmioty,
                    enrollment => enrollment.PrzedmiotId,
                    subject => subject.Id,
                    (_, subject) => $"{student.Imie} {student.Nazwisko} | {subject.Nazwa}"
                )
            );
    }

    /// <summary>
    ///     Zadanie:
    ///     Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///     SQL:
    ///     SELECT p.Nazwa, COUNT(*)
    ///     FROM Zapisy z
    ///     JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    ///     GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Zapisy
            .Join(
                DaneUczelni.Przedmioty,
                enrollment => enrollment.PrzedmiotId,
                subject => subject.Id,
                (_, subject) => subject.Nazwa
            )
            .GroupBy(subjectName => subjectName)
            .Select(group => $"{group.Key} | liczba zapisów: {group.Count()}");
    }

    /// <summary>
    ///     Zadanie:
    ///     Oblicz średnią ocenę końcową dla każdego przedmiotu.
    ///     Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///     SQL:
    ///     SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    ///     FROM Zapisy z
    ///     JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    ///     WHERE z.OcenaKoncowa IS NOT NULL
    ///     GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .Where(enrollment => enrollment.OcenaKoncowa.HasValue)
            .Join(
                DaneUczelni.Przedmioty,
                enrollment => enrollment.PrzedmiotId,
                subject => subject.Id,
                (enrollment, subject) => new { Name = subject.Nazwa, Grade = enrollment.OcenaKoncowa!.Value }
            )
            .GroupBy(record => record.Name)
            .Select(group => $"{group.Key} | średnia: {group.Average(x => x.Grade):0.00}");
    }

    /// <summary>
    ///     Zadanie:
    ///     Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    ///     W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///     SQL:
    ///     SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    ///     FROM Prowadzacy pr
    ///     LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    ///     GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                lecturer => lecturer.Id,
                subject => subject.ProwadzacyId,
                (lecturer, subjects) =>
                    $"{lecturer.Imie} {lecturer.Nazwisko} | liczba przedmiotów: {subjects.Count()}");
    }

    /// <summary>
    ///     Zadanie:
    ///     Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    ///     Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///     SQL:
    ///     SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    ///     FROM Studenci s
    ///     JOIN Zapisy z ON s.Id = z.StudentId
    ///     WHERE z.OcenaKoncowa IS NOT NULL
    ///     GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(enrollment => enrollment.OcenaKoncowa.HasValue),
                student => student.Id,
                enrollment => enrollment.StudentId,
                (student, enrollment) => new
                    { Name = student.Imie, Surname = student.Nazwisko, Grade = enrollment.OcenaKoncowa!.Value }
            )
            .GroupBy(record => new { record.Name, record.Surname })
            .Select(group => $"{group.Key.Name} {group.Key.Surname} | najwyższa ocena: {group.Max(x => x.Grade):0.0}");
    }

    /// <summary>
    ///     Wyzwanie:
    ///     Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    ///     Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///     SQL:
    ///     SELECT s.Imie, s.Nazwisko, COUNT(*)
    ///     FROM Studenci s
    ///     JOIN Zapisy z ON s.Id = z.StudentId
    ///     WHERE z.CzyAktywny = 1
    ///     GROUP BY s.Imie, s.Nazwisko
    ///     HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(enrollment => enrollment.CzyAktywny),
                student => student.Id,
                enrollment => enrollment.StudentId,
                (student, _) => new { Name = student.Imie, Surname = student.Nazwisko }
            )
            .GroupBy(record => new { record.Name, record.Surname })
            .Where(group => group.Count() > 1)
            .Select(group => $"{group.Key.Name} {group.Key.Surname} | aktywne przedmioty {group.Count()}");
    }

    /// <summary>
    ///     Wyzwanie:
    ///     Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///     SQL:
    ///     SELECT p.Nazwa
    ///     FROM Przedmioty p
    ///     JOIN Zapisy z ON p.Id = z.PrzedmiotId
    ///     WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    ///     GROUP BY p.Nazwa
    ///     HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return DaneUczelni.Przedmioty
            .Where(subject => subject.DataStartu is { Year: 2026, Month: 4 })
            .GroupJoin(
                DaneUczelni.Zapisy,
                subject => subject.Id,
                enrollment => enrollment.PrzedmiotId,
                (subject, enrollments) =>
                {
                    var enrollmentList = enrollments.ToArray();

                    return new
                    {
                        Name = subject.Nazwa,
                        HasAnyEnrollments = enrollmentList.Any(),
                        AreAllGradesEmpty = enrollmentList.All(zapis => !zapis.OcenaKoncowa.HasValue)
                    };
                })
            .Where(record => record.HasAnyEnrollments && record.AreAllGradesEmpty)
            .Select(record => record.Name);
    }

    /// <summary>
    ///     Wyzwanie:
    ///     Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    ///     Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///     SQL:
    ///     SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    ///     FROM Prowadzacy pr
    ///     LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    ///     LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    ///     WHERE z.OcenaKoncowa IS NOT NULL
    ///     GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        return DaneUczelni.Prowadzacy
            .Select(lecturer =>
                {
                    var grades = DaneUczelni.Przedmioty
                        .Where(subject => subject.ProwadzacyId == lecturer.Id)
                        .Join(
                            DaneUczelni.Zapisy.Where(enrollment => enrollment.OcenaKoncowa.HasValue),
                            subject => subject.Id,
                            enrollment => enrollment.PrzedmiotId,
                            (_, enrollment) => enrollment.OcenaKoncowa!.Value)
                        .ToList();

                    return grades.Count == 0
                        ? $"{lecturer.Imie} {lecturer.Nazwisko} | średnia ocen: brak ocen"
                        : $"{lecturer.Imie} {lecturer.Nazwisko} | średnia ocen: {grades.Average():0.00}";
                }
            );
    }

    /// <summary>
    ///     Wyzwanie:
    ///     Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    ///     Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///     SQL:
    ///     SELECT s.Miasto, COUNT(*)
    ///     FROM Studenci s
    ///     JOIN Zapisy z ON s.Id = z.StudentId
    ///     WHERE z.CzyAktywny = 1
    ///     GROUP BY s.Miasto
    ///     ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(enrollment => enrollment.CzyAktywny),
                student => student.Id,
                enrollment => enrollment.StudentId,
                (student, _) => student.Miasto
            )
            .GroupBy(city => city)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Select(group => $"{group.Key} | aktywne zapisy: {group.Count()}");
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}