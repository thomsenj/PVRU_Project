using System;


public enum ResourceType
{
    Wood,
    Stone
}

public class InvalidResourceTypeException : Exception
{
    public InvalidResourceTypeException() : base("Ein ungültiger Wochentag wurde angegeben.") { }

    public InvalidResourceTypeException(string message) : base(message) { }

    public InvalidResourceTypeException(string message, Exception innerException) 
        : base(message, innerException) { }
}

