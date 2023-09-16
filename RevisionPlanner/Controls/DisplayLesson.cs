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
    public class DisplayLesson
    {
        public Lesson Lesson { get; set; }
        public string DisplayName { get; set; }
        public string NextRevisionDisplay { get; set; }
        public Color CategoryColor { get; set; }
        public Color TextColor { get; set; }
        public bool CanRevise { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime NextRevision { get; set; }
        public int Order { get; set; }


    }

    public static class DisplayLessonConverter {
        public static DisplayLesson GetDisplay(this Lesson l, List<Category> cats, int[] revisionModel)
        {
            var last = l.LastRevised.Date;
            var date = l.CreationDate.Date;
            var i = 0;
            while(date <= last) {
                if (i < revisionModel.Length)
                    date = date.AddDays(revisionModel[i]);
                else date = date.AddDays(revisionModel.Last());
                i++;
            }
            Color color;
            bool revise = true;
            int order = 0;
            string text;
            if(date == DateTime.Now.Date)
            {
                color = Color.FromArgb("#3f944b");
                order = 1;
                text = "You have to revise this lesson today.";
            }
            else if(date < DateTime.Now.Date)
            {
                color = Color.FromArgb("#ff0000");
                text = "Late since "+ date.ToString("dd/MM/yyyy") + ". Do it now !";

            }
            else
            {
                color = Color.FromArgb("#999999");
                revise = false;
                order = 2;
                text = "Next Revision on " + date.ToString("dd/MM/yyyy")+".";
            }

            return new DisplayLesson()
            {
                DisplayName = (l.Category == 0 ? "" :(cats.Where(_=> _.Id == l.Category).FirstOrDefault().Name + " - ")) + l.Name,
                NextRevisionDisplay = text,
                NextRevision = date,
                CategoryColor = Color.FromArgb(l.Category == 0 ? "#3f944b" : cats.Where(_ => _.Id == l.Category).FirstOrDefault().Color),
                TextColor = color,
                CanRevise = revise,
                CreationDate = l.CreationDate,
                Lesson = l,
                Order = order
            };
        }

        public static List<DisplayLesson> GetDisplay(this List<Lesson> list, List<Category> cats, int[] revisionModel)
        {
            List<DisplayLesson> result = new();
            foreach (Lesson l in list) result.Add(l.GetDisplay(cats, revisionModel));

            return result.OrderBy(_ => _.Order).ToList();
        }
    }

}
