using System;
using System.Collections.Generic;

namespace DinnerSpinner.Domain.Model
{
    public class Spinner
    {
        public Guid Id { get; set; }

        public int Version { get; set; } = 1;

        public string Name { get; set; }

        public ICollection<Dinner> Dinners { get; set; } = new List<Dinner>();

        public ICollection<UserRef> Members { get; set; } = new List<UserRef>();
    }
}