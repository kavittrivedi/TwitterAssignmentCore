namespace TwitterAssignmentApi.Dtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string loginId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
