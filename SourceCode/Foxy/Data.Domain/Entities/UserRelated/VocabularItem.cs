using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.UserRelated
{
    public class VocabularItem
    {
        private VocabularItem()
        {
            // EF Core    
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid VocabularTemplateId { get; private set; }
        public string Note { get; private set; }
        public int CurrentStrike { get; private set; }
        public int RightAnswers { get; private set; }
        public int WrongAnswers { get; private set; }
        public bool LastAnswer { get; private set; }
        public DateTime LastTimeAnswered { get; private set; }
        public DateTime UnlockTime { get; private set; }
        public bool Favorite { get; private set; }

        public static VocabularItem Create(Guid userId, Guid vocabularId)
        {
            var instance = new VocabularItem() { Id = Guid.NewGuid() };
            instance.Update(userId, vocabularId);
            instance.Update(DateTime.MinValue);
            instance.Update("", 0, 0, 0, false, DateTime.MinValue, false);
            return instance;
        }

        public void Update(Guid userId, Guid vocabularId)
        {
            UserId = userId;
            VocabularTemplateId = vocabularId;
        }

        public void Update(DateTime unlockTime)
        {
            UnlockTime = unlockTime;
        }

        public void Update(string note, int currentStrike, int rightAnswers, int wrongAnswers, bool lastAnswer,
            DateTime lastTimeAnswered, bool favorite)
        {
            Note = note;
            CurrentStrike = currentStrike;
            RightAnswers = rightAnswers;
            WrongAnswers = wrongAnswers;
            LastAnswer = lastAnswer;
            LastTimeAnswered = lastTimeAnswered;
            Favorite = favorite;
        }
    }
}
