namespace Tippr.Application.DTOs.Groups
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string JoinCode { get; set; } = string.Empty;
        public int TournamentId { get; set; }
        public int MemberCount { get; set; }
        public bool IsAdmin { get; set; } // Är den inloggade användaren admin?
    }
}
