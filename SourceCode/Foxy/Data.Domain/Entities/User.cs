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
        public bool IsAdmin { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Token { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int Level { get; private set; }
        public Guid ImageId { get; private set; }

        public List<VocabularItem> VocabularItems { get; set; }

        public static User Create(string name, bool isAdmin, string email, string password, string token, string description)
        {
            var instance = new User { UserId = Guid.NewGuid(), CreatedAt = DateTime.Now};
            ImageEntity image = ImageEntity.Create(null);
            instance.Update(name, isAdmin, email, password, token, description, 1, image.Id);
            return instance;
        }

        public void Update(string name, bool isAdmin, string email, string password, string token, string description, int level, Guid imageId)
        {
            Username = name;
            IsAdmin = isAdmin;
            Email = email;
            Password = password;
            Token = token;
            Description = description;
            Update(level);
            ImageId = imageId;
        }
        public void Update(string token)
        {
            Token = token;
        }
        public void Update(int level)
        {
            Level = level;
        }
    }
}
