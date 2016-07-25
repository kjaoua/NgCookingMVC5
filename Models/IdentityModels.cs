using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgCookingMVC_BackEND.Models
{
    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant plus de propriétés à votre classe ApplicationUser ; consultez http://go.microsoft.com/fwlink/?LinkID=317594 pour en savoir davantage.
    public class NgCookingUser : IdentityUser
    {


        [Required]
        public string UserSurName { get; set; }

        [Required(ErrorMessage = "Picture is Required")]
        [DataType(DataType.Upload)]
        public byte[] Picture { get; set; }

        [Required(ErrorMessage = "Level  is Required")]
        public UserLevels Level { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public DateTime Birth { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }
        public NgCookingUser()
        {
            Picture = new byte[] { };
        }
        public byte[] FileToByteArray(string fileName)
        {
            byte[] fileContent = null;

            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);

            long byteLength = new System.IO.FileInfo(fileName).Length;
            fileContent = binaryReader.ReadBytes((Int32)byteLength);
            fs.Close();
            fs.Dispose();
            binaryReader.Close();
            return fileContent;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<NgCookingUser> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;
        }
    }

    public class NgCookingDbContext : IdentityDbContext<NgCookingUser>
    {
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }
        public DbSet<Recettes> Recettes { get; set; }
        public NgCookingDbContext()
            : base("DBConnectionString", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static NgCookingDbContext Create()
        {
            return new NgCookingDbContext();
        }
         protected override void OnModelCreating(DbModelBuilder  modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<RecettesIngredients>()
               .HasKey(t => new { t.IngredientId, t.RecetteId });

            modelBuilder.Entity<RecettesIngredients>()
                .HasRequired(pt => pt.Recette)
                .WithMany(p => p.RecettesIngredients)
                .HasForeignKey(pt => pt.RecetteId);

            modelBuilder.Entity<RecettesIngredients>()
                .HasRequired(pt => pt.Ingredient)
                .WithMany(t => t.RecettesIngredients)
                .HasForeignKey(pt => pt.IngredientId);


        }

        public System.Data.Entity.DbSet<NgCookingMVC_BackEND.Models.RecettesViewModel> RecettesViewModels { get; set; }

        public System.Data.Entity.DbSet<NgCookingMVC_BackEND.Models.IngredientViewModel> IngredientViewModels { get; set; }
    }
}