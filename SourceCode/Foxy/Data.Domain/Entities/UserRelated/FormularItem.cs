using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.UserRelated
{
    public class FormItem
    {
        private FormItem()
        {
            // EF Core    
        }

        public Guid FormItemId { get; private set; }

        public Guid UserId { get; private set; }
        public Guid FormularTemplateId { get; private set; }

        public string Note { get; private set; }
        public float AverageScore { get; private set; }
        public int TimesAnswered { get; private set; }
        
        public bool Favorite { get; private set; }


        public static FormItem Create(Guid userId, Guid lessonTemplateId)
        {
            var instance = new FormItem() { FormItemId = Guid.NewGuid() };
            instance.Update(userId, lessonTemplateId);
            instance.Update("", 0, 0, false);
            return instance;
        }

        public void Update(Guid userId, Guid formularId)
        {
            UserId = userId;
            FormularTemplateId = formularId;
        }
        
        public void Update(string note, float averageScore, int timesAnswered, bool favorite)
        {
            Note = note;
            Favorite = favorite;
            AverageScore = averageScore;
            TimesAnswered = timesAnswered;
        }
    }
}
