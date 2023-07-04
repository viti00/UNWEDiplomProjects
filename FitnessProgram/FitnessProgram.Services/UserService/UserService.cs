namespace FitnessProgram.Services.UserService
{
    using FitnessProgram.Data;

    public class UserService : IUserSerive
    {
        private readonly FitnessProgramDbContext context;

        public UserService(FitnessProgramDbContext context)
            => this.context = context;

        public string GetProfilePicture(string userId)
        {
            try
            {
                var user = context.Users.FirstOrDefault(x => x.Id == userId);
                int? userProfilePictureId = 0;
                if (user != null)
                {
                    userProfilePictureId = user.ProfilePictureId;
                }



                var picture = context.ProfilePhotos.FirstOrDefault(x => x.Id == userProfilePictureId);

                var profilePictureString = picture != null ? Convert.ToBase64String(picture.Bytes) : AnonymousImageConstant.AnonymousImage;

                return profilePictureString;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }
    }
}
