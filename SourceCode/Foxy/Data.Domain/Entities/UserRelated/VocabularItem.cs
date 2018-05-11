using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.UserRelated
{
    public enum MiniLevels : byte
    {
        Lev0,
        Lev11, //5 min
        Lev12, //10 min
        Lev21, //6z
        Lev22, //2w
        Lev31, //1month
        Lev4
    }

    public enum PeriodLevels // in minutes
    {
        Time0 = 0,
        Time11 = 30, //5 min
        Time12 = 60, //10 min
        Lev21 = 8640, //6z
        Lev22 = 20160, //2w
        Lev31 = 43200, //1month
        Lev4
    }

    public enum GrandLevels : byte
    {
        Lesson = 0, //0
        Seed = 2, //1<=x<=2
        Bloom = 4, //3<=x<=4
        Leaf = 5, //5
        Flourished = 6 //6
    }


    public class VocabularItem
    {
        private VocabularItem()
        {
            // EF Core    
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }
        public Guid VocabularId { get; private set; }

        public string MeaningNote { get; private set; }
        public string ReadingNote { get; private set; }

        public int CurrentStrike { get; private set; }
        public int RightAnswers { get; private set; }
        public int WrongAnswers { get; private set; }
        public bool LastAnswer { get; private set; }

        public int LockedComponents { get; private set; }

        public DateTime LastTimeAnswered { get; private set; }
        public DateTime UnlockTime { get; private set; }

        public MiniLevels CurrentMiniLevel { get; private set; }

        public bool Favorite { get; private set; }

        public static VocabularItem Create(Guid userId, Guid vocabularId, int lockedComponents)
        {
            var instance = new VocabularItem() { Id = Guid.NewGuid() };
            instance.Update(userId, vocabularId);
            instance.Update(DateTime.MaxValue);
            instance.Update("", "",  0, 0, 0, false, DateTime.MaxValue, false);
            instance.Update(lockedComponents, 0);
            return instance;
        }

        public void Update(Guid userId, Guid vocabularId)
        {
            UserId = userId;
            VocabularId = vocabularId;
        }

        public void Update(DateTime unlockTime)
        {
            UnlockTime = unlockTime;
        }

        public void Update(string meaningNote, string readingNote, int currentStrike, int rightAnswers, int wrongAnswers, bool lastAnswer,
            DateTime lastTimeAnswered, bool favorite)
        {
            MeaningNote = meaningNote;
            ReadingNote = readingNote;
            CurrentStrike = currentStrike;
            RightAnswers = rightAnswers;
            WrongAnswers = wrongAnswers;
            LastAnswer = lastAnswer;
            LastTimeAnswered = lastTimeAnswered;
            Favorite = favorite;
        }

        public void Update(int lockedComponents, MiniLevels miniLevels)
        {
            LockedComponents = lockedComponents;
            CurrentMiniLevel = miniLevels;
        }
    }
}
