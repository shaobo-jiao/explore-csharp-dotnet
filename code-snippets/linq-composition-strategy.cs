using System.Text.RegularExpressions;

string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };
// 1. remove vowel from names; 
// 2. filter names where length still >= 3;
// 3. order by name;

// fluent syntax
IEnumerable<string> query1 = names
    .Select(n => Regex.Replace(n, "[aeiou]", ""))
    .Where(n => n.Length >= 3)
    .OrderBy(n => n);

foreach(string name in query1)
    Console.WriteLine(name + ", ");
Console.WriteLine("---------");

// query expression - progressive
IEnumerable<string> query2 = 
    from n in names
    select Regex.Replace(n, "[aeiou]", "");

query2 = 
    from n in query2
    where n.Length >= 3
    orderby n
    select n;

foreach(string name in query2)
    Console.WriteLine(name + ", ");
Console.WriteLine("---------");

// query expression - into keyword
IEnumerable<string> query3 = 
    from n in names
    select Regex.Replace(n, "[aeiou]", "")
    into n2
    where n2.Length >= 3
    orderby n2
    select n2;
foreach(string name in query3)
    Console.WriteLine(name + ", ");
Console.WriteLine("---------");

// query expression - wrapping queries
IEnumerable<string> query4 = 
    from n1 in (
        from n2 in names
        select Regex.Replace(n2, "[aeiou]", "")
    )
    where n1.Length >= 3
    orderby n1
    select n1;
foreach(string name in query4)
    Console.WriteLine(name + ", ");
Console.WriteLine("---------");
