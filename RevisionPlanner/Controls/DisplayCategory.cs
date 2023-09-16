using Microsoft.VisualBasic;
using RevisionPlanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevisionPlanner.Controls
{
    public class DisplayCategory
    {
        public Category Category { get; set; }
        public string DisplayName { get; set; }
        public string Subtitle { get; set; }

        public Color Color { get; set; }

    }

    public static class DisplayCategoryConverter {
        public static DisplayCategory GetDisplay(this Category c, List<Lesson> lessons)
        {

            return new DisplayCategory()
            {
                Category = c,
                DisplayName = c.Name,
                Subtitle = lessons.Where(_ => _.Category == c.Id).ToList().Count + " Lessons",
                Color = Color.FromArgb(c.Color)
            };
        }

        public static List<DisplayCategory> GetDisplay(this List<Category> list, List<Lesson> lessons)
        {
            List<DisplayCategory> result = new();
            foreach (Category c in list) result.Add(c.GetDisplay(lessons));

            return result;
        }
    }

}
