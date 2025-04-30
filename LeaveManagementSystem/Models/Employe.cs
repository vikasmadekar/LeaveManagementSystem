using System.ComponentModel.DataAnnotations;

public class Employe
{
    [Key]
    public int EmployeId { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public int Password { get; set; }

    public string Role { get; set; }

    public string Department { get; set; }

    public string Designation { get; set; }

    public string RefreshToken { get; set; } = " "; 


    public DateTime RefreshTokenExpiryTime { get; set; }
}
