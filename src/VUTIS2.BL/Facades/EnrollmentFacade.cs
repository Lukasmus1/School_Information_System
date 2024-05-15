using VUTIS2.BL.Mappers;
using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;
using VUTIS2.DAL.Mappers;
using VUTIS2.DAL.UnitOfWork;

namespace VUTIS2.BL.Facades;

public class EnrollmentFacade (IUnitOfWorkFactory unitOfWorkFactory, IEnrollmentModelMapper enrollmentModelMapper) : FacadeBase<EnrollmentEntity, EnrollmentListModel, EnrollmentDetailModel, EnrollmentEntityMapper>(unitOfWorkFactory, enrollmentModelMapper), IEnrollmentFacade
{

}
