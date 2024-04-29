﻿using VUTIS2.BL.Models;
using VUTIS2.DAL.Entities;

namespace VUTIS2.BL.Facades;

public interface IEvaluationFacade : IFacade<EvaluationEntity, EvaluationListModel, EvaluationDetailModel>
{
    Task<IEnumerable<EvaluationListModel>> GetAsyncFromActivity(Guid Id);
}
