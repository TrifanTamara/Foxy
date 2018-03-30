using System;

namespace Data.Domain.Entities
{
    public class ImageEntity
    {
        private ImageEntity()
        {
            //EF Core
        }
        public Guid Id { get; private set; }
        public byte[] Image { get; private set; }

        public static ImageEntity Create(byte[] image)
        {
            var instance = new ImageEntity { Id = Guid.NewGuid()};
            instance.Update(image);
            return instance;
        }

        public void Update(byte[] image)
        {
            Image = image;
        }
    }
}
