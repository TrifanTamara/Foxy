using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.UserRelated
{
    public class FormularItem
    {
        private FormularItem()
        {
            // EF Core    
        }

        public Guid FormularItemId { get; private set; }

        public Guid UserId { get; private set; }
        public Guid FormularTemplateId { get; private set; }

        public string Note { get; private set; }
        public float AverageScore { get; private set; }
        public int TimesAnswered { get; private set; }
        
        public bool Favorite { get; private set; }


        public static FormularItem Create(Guid userId, Guid lessonTemplateId)
        {
            var instance = new FormularItem() { FormularItemId = Guid.NewGuid() };
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
