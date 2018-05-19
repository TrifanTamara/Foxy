﻿using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public Guid Id { get; private set; }

        [ForeignKey("User")]
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        [ForeignKey("VocabularTemplate")]
        public Guid VocablarId { get; set; }
        public VocabularTemplate Vocabular { get; private set; }

        public string MeaningNote { get; private set; }
        public string ReadingNote { get; private set; }

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

        public static VocabularItem Create(User user, VocabularTemplate vocabular, int lockedComponents)
        {
            var instance = new VocabularItem() { Id = Guid.NewGuid() };
            instance.Update(user, vocabular);
            instance.Update(DateTime.MaxValue);
            instance.Update("", "", false);
            instance.Update(0, 0, 0, 0, true, DateTime.MaxValue);
            instance.Update(lockedComponents, 0);
            return instance;
        }

        public void Update(User user, VocabularTemplate vocabular)
        {
            User = user;
            Vocabular = vocabular;
        }

        public void Update(DateTime unlockTime)
        {
            UnlockTime = unlockTime;
        }

        public void Update(string meaningNote, string readingNote, bool favorite)
        {
            MeaningNote = meaningNote;
            ReadingNote = readingNote;
            Favorite = favorite;
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
