namespace QuatroCleanUpBackend.Models
{
    public class User
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public int AvatarPictureId { get; set; }
        

        public User()
        {
            //Default constructor
        }


        public override string ToString()
        {
            return $"UserId: {UserId}, RoleId: {RoleId}, Name: {Name}, Email: {Email}, Password: {Password}, CreatedDate: {CreatedDate}, AvatarPic: {AvatarPictureId}";
        }
    }
}