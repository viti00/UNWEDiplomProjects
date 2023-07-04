namespace FitnessProgram.Services.PartnerService
{
    using FitnessProgram.Data.Models;
    using FitnessProgram.ViewModels.Partner;

    public interface IPartnerService
    {
        public AllPartnersQueryModel GetAll(int currPage, int postPerPage, AllPartnersQueryModel query, bool isAdministrator);

        public void AddPartner(PartnerFormModel model);

        public bool EditPartner(int partnerId, PartnerFormModel model, int startRowVersion);

        public void DeletePartner(Partner partner);

        public PartnerFormModel CreateEditModel(int partnerId);

        public Partner GetPartnerById(int id);

        public bool RemovePhoto(int partnerId, int photoId);
    }
}
