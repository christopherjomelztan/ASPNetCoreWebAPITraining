using ASPNetCoreWebAPITraining.Models;

namespace ASPNetCoreWebAPITraining.Database
{
  public static class SeedData
  {
    public static readonly Person[] Persons =
    [
      new() { Id = 1, FirstName = "Ada", LastName = "Lovelace" },
      new() { Id = 2, FirstName = "Grace", LastName = "Hopper" },
      new() { Id = 3, FirstName = "Alan", LastName = "Turing" },
      new() { Id = 4, FirstName = "Margaret", LastName = "Hamilton" },
      new() { Id = 5, FirstName = "Linus", LastName = "Torvalds" }
    ];
  }
}
