namespace Cafe
{
    public class UserModel
    {
        public int? Id { get; set; }
        public string? Login { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        //public int? ActiveShift { get; set; }
        public int? ActiveShiftId { get; set; }
        public ShiftModel? ActiveShift { get; set; }
        public string? Status { get; set; }
        public string? HashedPwd { get; set; }
    }
}