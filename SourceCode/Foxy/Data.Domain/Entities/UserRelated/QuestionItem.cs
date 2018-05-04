using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.UserRelated
{
    public class QuestionItem
    {
        private QuestionItem()
        {
            // EF Core    
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid QuestionId { get; private set; }
        
        public int RightAnswers { get; private set; }
        public int WrongAnswers { get; private set; }

        public DateTime UnlockTime { get; private set; }

        public static QuestionItem Create(Guid userId, Guid questionId)
        {
            var instance = new QuestionItem() { Id = Guid.NewGuid() };
            instance.Update(userId, questionId);
            instance.Update(DateTime.MaxValue);
            instance.Update(0, 0);
            return instance;
        }

        public void Update(Guid userId, Guid questionId)
        {
            UserId = userId;
            QuestionId = questionId;
        }

        public void Update(DateTime unlockTime)
        {
            UnlockTime = unlockTime;
        }

        public void Update(int rightAnswers, int wrongAnswers)
        {
            RightAnswers = rightAnswers;
            WrongAnswers = wrongAnswers;
        }
    }
}
