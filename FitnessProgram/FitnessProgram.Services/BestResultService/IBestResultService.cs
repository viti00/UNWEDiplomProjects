namespace FitnessProgram.Services.BestResultService
{
    using FitnessProgram.Data.Models;
    using FitnessProgram.ViewModels.BestResult;

    public interface IBestResultService
    {
        public AllBestResultsQueryModel GetAll(int currPage, int postPerPage,AllBestResultsQueryModel query, bool isAdministrator);

        public BestResultDetailsModel GetDetails(int bestresultId);

        public void AddBestResult(BestResultFormModel model);

        public bool EditBestResult(int bestResultId, BestResultFormModel model, int startRowVersion);

        public BestResultFormModel CreateEditModel(int bestResultId);

        public BestResult GetBestResultById(int id);

        public void DeleteBestResult(BestResult bestResult);

        public bool RemovePhoto(int postId, int photoId);

        public bool RemoveAllPhotos(int postId);
    }
}
