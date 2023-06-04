using System;
using System.ComponentModel.DataAnnotations;

public interface IStoring
{
    public List<Drug> Drugs { get; set; }
    void Add(Drug drug);
    void Remove(Drug drug);
}

