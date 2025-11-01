namespace XADAD7112_Application.Models.Account
{
    public class Dtos
    {

        public class AccountCreateDto : User {}

        public class AccountLoginDto
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        public class AccountUpdateDto
        {
            public User user { get; set; }
        }

        public class AccountDeleteDto
        {
            public int Id { get; set; }
        }

    }
}
