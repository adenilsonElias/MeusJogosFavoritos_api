using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserModel {
    [Key]
    public int id {get;set;}
    [Required]
    public string name {get;set;}
    [Required]
    public string email {get;set;}
    [Required]
    public string password {get;set;}
    public List<User_Games> favoritos {get;set;}
    [InverseProperty("creator")]
    public List<Games> criados {get;set;}
    public List<Amigo> amigos {get;set;}
    public List<Amigo> amigoDe {get;set;}

    public UserModel(){
        this.favoritos = new List<User_Games>();
        this.criados = new List<Games>();
        this.amigoDe = new List<Amigo>();
        this.amigos = new List<Amigo>();
    }
}