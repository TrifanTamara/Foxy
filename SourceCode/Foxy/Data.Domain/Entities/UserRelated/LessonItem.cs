using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.UserRelated
{
    public class LessonItem
    {
        private LessonItem()
        {
            // EF Core    
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid LessonTemplateId { get; private set; }
        public string Note { get; private set; }
        public DateTime UnlockTime { get; private set; }
        public bool Favorite { get; private set; }


        public static LessonItem Create(Guid userId, Guid lessonTemplateId)
        {
            var instance = new LessonItem() { Id = Guid.NewGuid() };
            instance.Update(userId, lessonTemplateId);
            instance.Update(DateTime.MinValue);
            instance.Update("", false);
            return instance;
        }

        public void Update(Guid userId, Guid lessonTemplateId)
        {
            UserId = userId;
            LessonTemplateId = lessonTemplateId;
        }

        public void Update(DateTime unlockTime)
        {
            UnlockTime = unlockTime;
        }

        public void Update(string note, bool favorite)
        {
            Note = note;
            Favorite = favorite;
        }
    }
}
