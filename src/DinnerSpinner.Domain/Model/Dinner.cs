using System;
using System.Collections.Generic;

namespace DinnerSpinner.Domain.Model;

public class Dinner
{
    public string Name { get; set; }

    public Guid Id { get; set; }

    public string Link { get; set; }

    public ICollection<Ingredient> Ingredients { get; set; }

    public SpinnerRef SpinnerRef { get; set; }

    /*
     *Étapes de préparation
Préchauffez le four à 200 °C.
Déposez les saucisses dans un grand plat, salez et poivrez.
Enfournez environ 25 minutes.
Astuces et conseils pour Saucisse au four
Vous pouvez remplacer le poivre par du piment d'Espelette.
     *
     */
}
