using Data.Domain.Entities.UserRelated;
using System;
using System.Collections.Generic;

namespace Data.Domain.Entities
{
    public class User
    {
        private User()
        {
            // EF Core    
        }

        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int Level { get; private set; }
        public Guid ImageId { get; private set; }

        public int LessonSize { get; private set; }

        public List<VocabularItem> VocabularItems { get; set; }

        public static User Create(string name, string email, string password, string description)
        {
            var instance = new User { UserId = Guid.NewGuid(), CreatedAt = DateTime.Now};
            ImageEntity image = ImageEntity.Create(null);
            instance.Update(name, email, password, description, 0, image.Id);
            return instance;
        }

        public void Update(string name, string email, string password, string description, int level, Guid imageId)
        {
            Username = name;
            Email = email;
            Password = password;
            Description = description;
            Update(level);
            ImageId = imageId;
            LessonSize = 5;
        }

        public void Update(int level)
        {
            Level = level;
        }
    }
}
