namespace Matchmaker.Server.BaseServer;

public class ClientAttributes
{
    private const uint MaxAttributes = 64;
    private class Attribute
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        public string AttributeName;
        public string AttributeValue;

        public Attribute(string name, string value)
        {
            AttributeName = name;
            AttributeValue = value;
        }
    }

    private readonly List<Attribute> _attributes = new();

    public void SetAttribute(string name, string value)
    {
        // Check if the attribute is already set
        foreach (var attr in _attributes.Where(attr => attr.AttributeName.Equals(name)))
        {
            attr.AttributeValue = value;
            return;
        }

        // If it is not
        if (_attributes.Count < MaxAttributes)
        {
            _attributes.Add(new Attribute(name, value));
        }
        else
        {
            Console.WriteLine("Failed to add attribute: Reached limit!");
        }
    }

    public string? GetAttribute(string name)
    {
        return _attributes.Where(attr => attr.AttributeName.Equals(name)).Select(attr => attr.AttributeValue).FirstOrDefault();
    }

    public void Clear()
    {
        _attributes.Clear();
    }
}