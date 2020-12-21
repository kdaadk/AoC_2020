using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2020
{
    public class Day21
    {
        public static void SolvePart2()
        {
            var input = File.ReadAllLines("input.txt");
            var products = new List<Product>();

            foreach (var product in input)
            {
                var tokens = product.Split(" (contains ", StringSplitOptions.RemoveEmptyEntries);
                var ingredients = tokens[0].Split().ToHashSet();
                var allergens = tokens[1].Replace(")", "").Split(", ", StringSplitOptions.RemoveEmptyEntries)
                    .ToHashSet();
                products.Add(new Product {Allergens = allergens, Ingredients = ingredients});
            }
            
            var knownAllergens = new Dictionary<string, HashSet<string>>();
            for (var i = 0; i < products.Count; i++)
            {
                var allergensFirst = GetAllergens(products[i], knownAllergens).ToList();
                var ingredientsFirst = GetIngredients(products[i], knownAllergens).ToList();
                for (var j = i + 1; j < products.Count; j++)
                {
                    var ingredientsIntersect = ingredientsFirst.Intersect(GetIngredients(products[j], knownAllergens));
                    var allergensIntersect = allergensFirst.Intersect(GetAllergens(products[j], knownAllergens)).ToList();
                    if (allergensIntersect.Count == 1)
                    {
                        if (!knownAllergens.ContainsKey(allergensIntersect[0]))
                            knownAllergens.Add(allergensIntersect[0], ingredientsIntersect.ToHashSet());
                        else
                            knownAllergens[allergensIntersect[0]] = knownAllergens[allergensIntersect[0]].Intersect(ingredientsIntersect).ToHashSet();
                    }
                }
            }
            while (knownAllergens.Any(x => x.Value.Count != 1))
                knownAllergens = ReduceIngredients(knownAllergens);

            var result = knownAllergens.OrderBy(x => x.Key)
                .SelectMany(x => x.Value);
            Console.WriteLine(string.Join(",", result));            
        }

        private static Dictionary<string, HashSet<string>> ReduceIngredients(Dictionary<string, HashSet<string>> knownAllergens)
        {
            if (knownAllergens.All(x => x.Value.Count != 1)) return knownAllergens;
            
            var reduce = new Dictionary<string, HashSet<string>>();
            var (correctAllergen, correctIngredients) = knownAllergens.First(x => x.Value.Count == 1);
            foreach (var (allergen, ingredients) in knownAllergens.Where(x => x.Key != correctAllergen))
                reduce[allergen] = ingredients.Where(x => x != correctIngredients.First()).ToHashSet();

            if (!reduce.ContainsKey(correctAllergen))
                reduce[correctAllergen] = correctIngredients;
            
            return reduce;
        }

        private static IEnumerable<string> GetAllergens(Product product, Dictionary<string, HashSet<string>> known) =>
            product.Allergens.Where(x => !known.Any(y => y.Value.Contains(x)));

        private static IEnumerable<string> GetIngredients(Product product, Dictionary<string, HashSet<string>> known) =>
            product.Ingredients.Where(x => !known.Select(y => y.Key).Contains(x));

        private class Product
        {
            public HashSet<string> Ingredients { get; set; }
            public HashSet<string> Allergens { get; set; }
        }
    }
}