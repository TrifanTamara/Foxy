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

        public Guid QuestionItemId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid QuestionTemplateId { get; private set; }
        
        public int RightAnswers { get; private set; }
        public int WrongAnswers { get; private set; }

        public static QuestionItem Create(Guid userId, Guid questionId)
        {
            var instance = new QuestionItem() { QuestionItemId = Guid.NewGuid() };
            instance.Update(userId, questionId);
            instance.Update(0, 0);
            return instance;
        }

        public void Update(Guid userId, Guid questionId)
        {
            UserId = userId;
            QuestionTemplateId = questionId;
        }
        
        public void Update(int rightAnswers, int wrongAnswers)
        {
            RightAnswers = rightAnswers;
            WrongAnswers = wrongAnswers;
        }
    }
}
