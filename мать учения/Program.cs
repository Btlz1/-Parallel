
string[] firstNames = ["James", "Jonathan", "Stan", "Dean", "Sam", "Shone"];
string[] lastNames = ["Winchester", "Joestar", "Lee"];
Random random = new();
var persons = Enumerable
    .Range(0, 20000000)
    .Select(_ =>
    {
        var qualification = (Qualification)random.Next(0, 6);
        var age = random.Next(18, 65);
        var firstName = firstNames[random.Next(firstNames.Length)];
        var lastName = lastNames[random.Next(lastNames.Length)];
        return new InternalPerson
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Age = age,
            Qualification = qualification.ToString("G"),
        };
    })
    .ToList();

var mapped = await Map(persons);

Console.WriteLine(mapped.Count);

async Task<List<IntegrationPerson>> Map(List<InternalPerson> source)
{
    var tasks = source.Select(person =>
    {
        return Task.Run(() =>
        {
            if (!Enum.TryParse(person.Qualification, true, out Qualification qualificationEnum))
            {
                qualificationEnum = Qualification.Junior;
            }
            return new IntegrationPerson
            {
                Id = person.Id.ToString(),
                FullName = person.FirstName + ' ' + person.LastName,
                Age = person.Age,
                Qualification = qualificationEnum
            };
        });
    }).ToList();
    
    var results = await Task.WhenAll(tasks);
    
    return results.ToList();
}

class InternalPerson
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Qualification { get; set; }
}

class IntegrationPerson
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public Qualification Qualification { get; set; }
}

enum Qualification
{
    Junior = 0,
    Middle = 1,
    Senior = 2,
    TeamLead = 3,
    TechLead = 4,
    Cto = 5
}