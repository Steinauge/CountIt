namespace CountIt.Core.Models;

public class SectionItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "Neuer Abschnitt";
    public List<CounterItem> Items { get; set; } = new();
}