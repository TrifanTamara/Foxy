using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Domain.Entities.UserRelated
{
    public enum MiniLevels : byte
    {
        Lesson,
        Seed1, //5 min
        Seed2, //10 min
        Leaf1, //6z
        Leaf2, //2w
        Bloom, //1month
        Flourised
    }

    public enum PeriodLevels // in minutes
    {
        Time0 = 0,
        Time11 = 30, //5 min
        Time12 = 60, //10 min
        Time21 = 8640, //6z
        Time22 = 20160, //2w
        Time31 = 43200, //1month
        Lev4
    }

    public enum GrandLevels : byte
    {
        Lesson = 0, //0
        Seed = 2, //1<=x<=2
        Leaf = 4, //3<=x<=4
        Bloom = 5, //5
        Flourished = 6 //6
    }


    public class VocabularItem
    {
        private VocabularItem()
        {
            // EF Core    
        }

        public Guid VocabularItemId { get; private set; }
        
        public Guid UserId { get; private set; }
        public Guid VocabularTemplateId { get; set; }

        public string MeaningNote { get; private set; }
        public string ReadingNote { get; private set; }

        public string UserSynonyms { get; private set; }

        public int CurrentStrike { get; private set; }
        public int LongestStrike { get; private set; }

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
            var instance = new VocabularItem() { VocabularItemId = Guid.NewGuid() };
            instance.Update(userId, vocabularId);
            instance.Update(DateTime.MaxValue);
            instance.Update("", "", false, "");
            instance.Update(0, 0, 0, 0, true, DateTime.MaxValue);
            instance.Update(lockedComponents, 0);
            return instance;
        }
        public void Update(Guid user, Guid vocabular)
        {
            UserId = user;
            VocabularTemplateId = vocabular;
        }

        public void Update(DateTime unlockTime)
        {
            UnlockTime = unlockTime;
        }

        public void Update(string meaningNote, string readingNote, bool favorite, string synonyms)
        {
            MeaningNote = meaningNote;
            ReadingNote = readingNote;
            Favorite = favorite;
            UserSynonyms = synonyms;
        }

        public void Update(int currentStrike, int longestStrike, int rightAnswers, int wrongAnswers, bool lastAnswer,
            DateTime lastTimeAnswered)
        {
            CurrentStrike = currentStrike;
            LongestStrike = longestStrike;
            RightAnswers = rightAnswers;
            WrongAnswers = wrongAnswers;
            LastAnswer = lastAnswer;
            LastTimeAnswered = lastTimeAnswered;
        }

        public void Update(int lockedComponents, MiniLevels miniLevels)
        {
            LockedComponents = lockedComponents;
            CurrentMiniLevel = miniLevels;
        }
    }
}
