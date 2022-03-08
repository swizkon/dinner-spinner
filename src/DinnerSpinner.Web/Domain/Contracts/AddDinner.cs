using System.Collections.Generic;

namespace DinnerSpinner.Api.Domain.Contracts;

public class AddDinner
{
    public string SpinnerId { get; set; }

    public string Name { get; set; }

    public List<string> Ingredients { get; set; }
}
