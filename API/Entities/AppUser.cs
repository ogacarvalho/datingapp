namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; } // Ao usar o nome "Id" automaticamente o migration o criará como primary key. Se não quiser, precisará adicionar um "DataAnnotation" que incluirá um [Key] em cima da coluna cujo nome não é "Id".
        public string UserName { get; set; } //Pascal Casing its a C# pattern.
    }
}