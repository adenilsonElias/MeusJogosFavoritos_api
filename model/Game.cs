
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

[Table("games")]
public class Games {
    [Key]
    public int id {get;set;}
    [Required]
    public string name {get;set;}
    [Required]
    public string produces {get;set;}
    public List<User_Games> favorite {get;set;}
    int creatorUserId{get;set;}
    public UserModel creator {get;set;}

    public Games(){
        this.favorite = new List<User_Games>();
        this.creator = new UserModel();
    }
}