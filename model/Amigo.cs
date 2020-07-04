
public class Amigo{
    public int userId {get;set;}
    public UserModel user {get;set;}
    public int amigoId {get;set;}
    public UserModel amigo {get;set;}

    public Amigo(){
        this.user = new UserModel();
        this.amigo = new UserModel();
    }

}