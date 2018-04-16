using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.UserRelated
{
    public class QuizzItem
    {
        private QuizzItem()
        {
            // EF Core    
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid QuizzTemplateId { get; private set; }
        public int CurrentStrike { get; private set; }
        public int RightAnswers { get; private set; }
        public int WrongAnswers { get; private set; }
        public bool LastAnswer { get; private set; }
        public DateTime LastTimeAnswered { get; private set; }
        public DateTime UnlockTime { get; private set; }


        public static QuizzItem Create(Guid userId, Guid quizzTemplateId)
        {
            var instance = new QuizzItem() { Id = Guid.NewGuid() };
            instance.Update(userId, quizzTemplateId);
            instance.Update(DateTime.MinValue);
            instance.Update(0, 0, 0, false, DateTime.MinValue);
            return instance;
        }

        public void Update(Guid userId, Guid quizzTemplateId)
        {
            UserId = userId;
            QuizzTemplateId = quizzTemplateId;
        }

        public void Update(DateTime unlockTime)
        {
            UnlockTime = unlockTime;
        }

        public void Update(int currentStrike, int rightAnswers, int wrongAnswers, bool lastAnswer, DateTime lastTimeAnswered)
        {
            CurrentStrike = currentStrike;
            RightAnswers = rightAnswers;
            WrongAnswers = wrongAnswers;
            LastAnswer = lastAnswer;
            LastTimeAnswered = lastTimeAnswered;
        }
    }
}
