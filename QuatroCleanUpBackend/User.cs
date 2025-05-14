namespace QuatroCleanUpBackend
{
    public class User
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public byte[] AvatarPic { get; set; } = null!;
    }
}
