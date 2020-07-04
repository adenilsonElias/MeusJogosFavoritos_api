using System.ComponentModel.DataAnnotations;

public class User_Games {
    [Key]
    public int id_User {get;set;}
    public UserModel user {get;set;}
    [Key]
    public int id_game {get;set;}
    public Games games {get;set;}
}