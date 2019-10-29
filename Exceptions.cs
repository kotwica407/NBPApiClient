using System;

public class EmptyListException : Exception
{
    public EmptyListException(){}

    public EmptyListException(string message) : base(message){}
}